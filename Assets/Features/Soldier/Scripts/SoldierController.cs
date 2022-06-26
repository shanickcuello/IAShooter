using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.PathFinding;
using Features.Soldier.Scripts.FSM;
using Features.Soldier.Scripts.FSM.States;
using Features.Soldier.Scripts.Weapons;
using Features.Weapons.Bullets.Code;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Soldier.Scripts
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class SoldierController : MonoBehaviour, IDamageable
    {
        [SerializeField] PathfindingManager pathfindingManager;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] SoldierData soldierData;
        [SerializeField] private GameObject _weaponPrefab;
        public Transform target => _target;
        private float speedMovement;
        private IAIVision _aiVision;
        private SoldierView _soldierView;
        private SoldierIdleState<ESoldierStates> _soldierIdleState;
        private SoldierPatrolState<ESoldierStates> _soldierPatrolState;
        private SoldierAttackState<ESoldierStates> _soldierAttackState;
        private SoldierDeath<ESoldierStates> _soldierDeathState;
        private FSM<ESoldierStates> _fsm;
        private Transform _target;
        private IWeapon _weapon;
        private Coroutine _currentPath;
        private Life _life;


        protected void Awake()
        {
            _life = new Life(100, Death);
            _aiVision = GetComponent<AIVision>();
            _soldierView = GetComponent<SoldierView>();
            _weapon = _weaponPrefab.GetComponent<IWeapon>();

            _soldierAttackState = new SoldierAttackState<ESoldierStates>(this, _soldierView);
            _soldierDeathState = new SoldierDeath<ESoldierStates>(this);
            _soldierIdleState = new SoldierIdleState<ESoldierStates>(this, _soldierView);
            _soldierPatrolState = new SoldierPatrolState<ESoldierStates>(this, _soldierView, pathfindingManager);
            _soldierIdleState.AddTransitionState(ESoldierStates.Patrol, _soldierPatrolState);
            _soldierIdleState.AddTransitionState(ESoldierStates.Attack, _soldierAttackState);
            _soldierIdleState.AddTransitionState(ESoldierStates.Death, _soldierDeathState);
            _soldierPatrolState.AddTransitionState(ESoldierStates.Attack, _soldierAttackState);
            _soldierPatrolState.AddTransitionState(ESoldierStates.Death, _soldierDeathState);
            _soldierPatrolState.AddTransitionState(ESoldierStates.Idle, _soldierIdleState);
            _fsm = new FSM<ESoldierStates>(_soldierIdleState);
        }

        private void Death()
        {
            _soldierView.Death();
            ChangeState(ESoldierStates.Death);
        }

        private void Start()
        {
            speedMovement = soldierData.speedMovement;
        }


        private void Update()
        {
            _fsm.OnUpdate();
        }

        public void ChangeState(ESoldierStates state)
        {
            _fsm.Transition(state);
        }

        public void MoveBy(List<Vector3> path, Action onFinishPath)
        {
            _currentPath = StartCoroutine(FollowPath(path: path, onFinishPath: onFinishPath));
        }

        private IEnumerator FollowPath(List<Vector3> path, Action onFinishPath)
        {
            foreach (var pos in path)
            {
                Vector3 checkPoint = pos;
                checkPoint.y = transform.position.y;

                while (transform.position != checkPoint)
                {
                    transform.position =
                        Vector3.MoveTowards(transform.position, checkPoint, speedMovement * Time.deltaTime);
                    _soldierView.LookAt(checkPoint);
                    yield return null;
                }
            }

            onFinishPath.Invoke();
        }

        public void SearchForEnemy()
        {
            if (!_aiVision.SearchBy(enemyLayer).Any()) return;
            _target = _aiVision.SearchBy(enemyLayer).First().transform;
            ChangeState(ESoldierStates.Attack);
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
            if (_currentPath != null)
                StopCoroutine(_currentPath);
        }

        public void MakeDamage(Vector3 transformPosition, int damagePower)
        {
            GetDamage(damagePower);
        }

        private void GetDamage(int damagePower)
        {
            _life.DecreaseLife(damagePower);
        }

        public void UnableColliders()
        {
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}