using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.LifeSystem;
using Features.PathFinding;
using Features.PowerUps;
using Features.Soldier.Scripts.FSM;
using Features.Soldier.Scripts.FSM.States;
using Features.Soldier.Scripts.Weapons;
using Features.Weapons.Bullets.Code;
using UnityEngine;

namespace Features.Soldier.Scripts
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class SoldierController : MonoBehaviour, IDamageable
    {
        [SerializeField] PathfindingManager pathfindingManager;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private LayerMask lifeLayer;
        [SerializeField] SoldierData soldierData;
        [SerializeField] private GameObject _weaponPrefab;
        [SerializeField] private float _minimumSpeedMovement;

        public Transform target => _target;
        private float _speedMovement;
        private IAIVision _aiVision;
        private SoldierView _soldierView;
        private SoldierIdleState<ESoldierStates> _soldierIdleState;
        private SoldierPatrolState<ESoldierStates> _soldierPatrolState;
        private SoldierAttackState<ESoldierStates> _soldierAttackState;
        private SoldierDeath<ESoldierStates> _soldierDeathState;
        private SoldierSearchLifeState<ESoldierStates> _soldierSearchLife;
        private SoldierGoToFight<ESoldierStates> _soldierGoToFightState;

        private FSM<ESoldierStates> _fsm;
        private Transform _target;
        private IWeapon _weapon;
        private Coroutine _moveRoutine;
        private Life _life;
        [SerializeField] private List<Vector3> listOfPowerUps = new List<Vector3>();
        private Vector3 _fightPosition;
        private Rigidbody _rigidbody;


        protected void Awake()
        {
            Initialize();
            CreateStates();
            AddTransitionsStates();
            CreateFsm();
        }

        private void CreateFsm()
        {
            _fsm = new FSM<ESoldierStates>(_soldierIdleState);
        }

        private void AddTransitionsStates()
        {
            _soldierIdleState.AddTransitionState(ESoldierStates.Patrol, _soldierPatrolState);
            _soldierIdleState.AddTransitionState(ESoldierStates.Attack, _soldierAttackState);
            _soldierIdleState.AddTransitionState(ESoldierStates.Death, _soldierDeathState);
            _soldierIdleState.AddTransitionState(ESoldierStates.SearchLife, _soldierSearchLife);

            _soldierPatrolState.AddTransitionState(ESoldierStates.Attack, _soldierAttackState);
            _soldierPatrolState.AddTransitionState(ESoldierStates.Death, _soldierDeathState);
            _soldierPatrolState.AddTransitionState(ESoldierStates.Idle, _soldierIdleState);
            _soldierPatrolState.AddTransitionState(ESoldierStates.SearchLife, _soldierSearchLife);

            _soldierAttackState.AddTransitionState(ESoldierStates.Death, _soldierDeathState);
            _soldierAttackState.AddTransitionState(ESoldierStates.SearchLife, _soldierSearchLife);

            _soldierSearchLife.AddTransitionState(ESoldierStates.Death, _soldierDeathState);
            _soldierSearchLife.AddTransitionState(ESoldierStates.GoToFight, _soldierGoToFightState);

            _soldierGoToFightState.AddTransitionState(ESoldierStates.Attack, _soldierAttackState);
            _soldierGoToFightState.AddTransitionState(ESoldierStates.Death, _soldierDeathState);
        }

        private void CreateStates()
        {
            _soldierSearchLife = new SoldierSearchLifeState<ESoldierStates>(this, pathfindingManager, _soldierView);
            _soldierAttackState = new SoldierAttackState<ESoldierStates>(this, _soldierView);
            _soldierDeathState = new SoldierDeath<ESoldierStates>(this);
            _soldierIdleState = new SoldierIdleState<ESoldierStates>(this, _soldierView);
            _soldierPatrolState = new SoldierPatrolState<ESoldierStates>(this, _soldierView, pathfindingManager);
            _soldierGoToFightState = new SoldierGoToFight<ESoldierStates>(this, pathfindingManager, _soldierView);
        }

        private void Initialize()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _life = new Life(soldierData.life, onDeath: Death);
            _aiVision = GetComponent<AIVision>();
            _soldierView = GetComponent<SoldierView>();
            _weapon = _weaponPrefab.GetComponent<IWeapon>();
        }

        private void Death()
        {
            _soldierView.Death();
            ChangeState(ESoldierStates.Death);
        }

        private void Start()
        {
            _speedMovement = soldierData.speedMovement;
        }


        private void Update()
        {
            _fsm.OnUpdate();
        }

        public void ChangeState(ESoldierStates state)
        {
            StopMoving();
            _fsm.Transition(state);
        }

        public void MoveBy(List<Vector3> path, Action onFinishPath)
        {
            StopMoving();
            _moveRoutine = StartCoroutine(FollowPath(path: path, onFinishPath: onFinishPath));
        }

        private IEnumerator FollowPath(List<Vector3> path, Action onFinishPath)
        {
            foreach (var pos in path)
            {
                Vector3 checkPoint = pos;
                checkPoint.y = transform.position.y;

                while (GetDistanceTo(checkPoint) > 0.3f)
                {
                    transform.position =
                        Vector3.MoveTowards(transform.position, checkPoint, _speedMovement * Time.deltaTime);
                    _soldierView.LookAt(checkPoint);
                    yield return null;
                }
            }

            onFinishPath.Invoke();
        }

        private float GetDistanceTo(Vector3 checkPoint)
        {
            return Vector3.Distance(transform.position, checkPoint);
        }

        public void SearchForEnemy()
        {
            if (!_aiVision.SearchBy(enemyLayer).Any()) return;
            _target = _aiVision.SearchBy(enemyLayer).First().transform;
            ChangeState(ESoldierStates.Attack);
        }

        public void SearchForLife()
        {
            if (!_aiVision.SearchBy(lifeLayer).Any()) return;
            RememberPowerUpPosition(_aiVision.SearchBy(lifeLayer).First().transform.position);
        }

        private void RememberPowerUpPosition(Vector3 powerUp)
        {
            if (!listOfPowerUps.Contains(powerUp))
                listOfPowerUps.Add(powerUp);
        }

        public void ShootIntermittent()
        {
            _soldierView.SetAnimation(SoldierAnimations.Shoot);
            StartCoroutine(ShootIntermittentCoroutine(_target));
        }

        private IEnumerator ShootIntermittentCoroutine(Transform target)
        {
            while (true)
            {
                Shoot();
                yield return new WaitForSeconds(soldierData.timeBetweenShoots);
            }
        }

        private void Shoot()
        {
            _weapon.Fire(_target);
        }

        public void StopMoving()
        {
            if (_moveRoutine != null)
                StopCoroutine(_moveRoutine);
        }

        public void MakeDamage(Vector3 transformPosition, int damagePower)
        {
            GetDamage(damagePower);
        }

        private void GetDamage(int damagePower)
        {
            _life.DecreaseLife(damagePower);
            ShouldFindLife();
        }

        private void ShouldFindLife()
        {
            if (_life.LifeAmount <= 50 && _life.LifeAmount > 0 && listOfPowerUps.Count > 0)
            {
                ChangeState(ESoldierStates.SearchLife);
            }
        }

        public void UnableColliders()
        {
            GetComponent<CapsuleCollider>().enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<LifePowerUp>()) return;
            var liferPowerUp = other.GetComponent<LifePowerUp>();
            _life?.AddLife(liferPowerUp.Consume());
        }

        public Vector3 GetClosestLifePowerUp()
        {
            var closestPowetUp = listOfPowerUps.OrderBy(powerUp =>
                Vector3.Distance(transform.position, powerUp)).First();
            listOfPowerUps.Remove(closestPowetUp);
            return closestPowetUp;
        }

        public void SetFightPosition() => _fightPosition = transform.position;

        public Vector3 GetFightPosition() => _fightPosition;
    }
}