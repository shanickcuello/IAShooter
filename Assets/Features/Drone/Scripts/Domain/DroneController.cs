using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Drone.Scripts.FSM;
using Features.Flocking;
using Features.LifeSystem;
using Features.RouletteSystem;
using Features.Soldier.Scripts.Domain;
using Features.StateMachine;
using Features.Weapons;
using Features.Weapons.Bullets.Code;
using UnityEngine;

namespace Features.Drone.Scripts.Domain
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AIVision))]
    public class DroneController : MonoBehaviour, IDamageable
    {
        [SerializeField] Transform target;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private GameObject _weaponPrefab;
        private Transform _lead;
        private AIVision _aiVision;
        public float minDistanceToMove;
        MovementBehavior _movementBehavior;
        private FlockEntity _flock;
        
        private DroneFollowLeadState<EDroneStates> _droneFollowLeadState;
        private DroneDeathState<EDroneStates> _deathState;
        private DroneAttackState<EDroneStates> _droneAttackState;
        private DroneDefendState<EDroneStates> _droneDefendState;

        private Fsm<EDroneStates> _fsm;
        private Rigidbody _rigidbody;
        private Life _life;
        private LeaderBehavior _leaderBehavior;
        private IWeapon _weapon;
        private float _timeNotSeeingEnemy;
        private Coroutine _shootRoutine;
        private Roulette _roulette;
        private bool _defending;
        private const float TimeToFollowLeadWhenDoesntSeeEnemy = 2f;

        private void Awake()
        {
            _roulette = new Roulette();
            _lead = target;
            _weapon = _weaponPrefab.GetComponent<IWeapon>();
            _aiVision = GetComponent<AIVision>();
            _rigidbody = GetComponent<Rigidbody>();
            _flock = GetComponent<FlockEntity>();
            _life = new Life(50, () => ChangeState(EDroneStates.Death));
            _leaderBehavior = GetComponent<LeaderBehavior>();
            _leaderBehavior._target = target;

            _deathState = new DroneDeathState<EDroneStates>(this);
            _droneFollowLeadState = new DroneFollowLeadState<EDroneStates>(this);
            _droneAttackState = new DroneAttackState<EDroneStates>(this);
            _droneDefendState = new DroneDefendState<EDroneStates>(this);

            _droneFollowLeadState.AddTransitionState(EDroneStates.Death, _deathState);
            _droneFollowLeadState.AddTransitionState(EDroneStates.Attack, _droneAttackState);
            _droneFollowLeadState.AddTransitionState(EDroneStates.Defend, _droneDefendState);

            _droneAttackState.AddTransitionState(EDroneStates.Death, _deathState);
            _droneAttackState.AddTransitionState(EDroneStates.FollowLead, _droneFollowLeadState);
            _droneAttackState.AddTransitionState(EDroneStates.Defend, _droneDefendState);
            
            _droneDefendState.AddTransitionState(EDroneStates.Attack, _droneAttackState);
            _droneDefendState.AddTransitionState(EDroneStates.FollowLead, _droneFollowLeadState);
            _droneDefendState.AddTransitionState(EDroneStates.Death, _deathState);
        }

        void Start()
        {
            _movementBehavior = GetComponent<MovementBehavior>();
            _fsm = new Fsm<EDroneStates>(_droneFollowLeadState);
        }

        void Update()
        {
            _fsm.OnUpdate();
        }

        public void MoveOrStop()
        {
            if (ShouldMove())
                _movementBehavior.Move(_flock.GetDir());
            else
                UnableMovement();
        }

        private void UnableMovement()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        private bool ShouldMove()
        {
            float distaceToTarget = Vector3.Distance(transform.position, target.position);
            return distaceToTarget > minDistanceToMove;
        }

        public void MakeDamage(Vector3 transformPosition, int damagePower)
        {
            GetDamage(damagePower);
        }

        public void GetDamage(int damagePower)
        {
            if(_defending) return;
            _life.DecreaseLife(damagePower);
            DefendOrNot();
        }

        private void DefendOrNot()
        {
            Dictionary<String, int> decision = new Dictionary<string, int>();
            decision.Add("doNothing", 20);
            decision.Add("defend", (5 / _life.LifeAmount) * 100);
            
            if(_roulette.Execute(decision) == "defend")
            {
                ChangeState(EDroneStates.Defend);
            }
        }

        public void ChangeState(EDroneStates state)
        {
            _fsm.Transition(state);
        }

        public void Shutdown()
        {
            _rigidbody.mass = 1;
            _rigidbody.useGravity = true;
            _rigidbody.AddForce(Vector3.up * 8000 + Vector3.right * 1000);
            _rigidbody.constraints = RigidbodyConstraints.None;
            _weapon.ShutdownLight();
        }

        public void SearchForEnemy()
        {
            if (!IsEnemyOnSight()) return;
            target = _aiVision.SearchBy(enemyLayer).First().transform;
            ChangeState(EDroneStates.Attack);
        }

        private bool IsEnemyOnSight()
        {
            return _aiVision.SearchBy(enemyLayer).Any();
        }

        public void FollowEnemy()
        {
            if (target == null) return;
            _leaderBehavior._target = target;
        }

        public void ShootIntermittent()
        {
            _shootRoutine = StartCoroutine(ShootIntermittentCoroutine(target));
        }

        private IEnumerator ShootIntermittentCoroutine(Transform target)
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                Shoot();
            }
        }

        private void Shoot()
        {
            _weapon.Fire(gameObject.layer);
        }

        public void CheckEnemySteering()
        {
            if (_aiVision.IsInVision(target.gameObject, enemyLayer))
            {
                IncreaseTimeSeeingEnemy();
                return;
            }
            _timeNotSeeingEnemy -= 1 * Time.deltaTime;
            _timeNotSeeingEnemy = Mathf.Max(0, _timeNotSeeingEnemy);
            if (_timeNotSeeingEnemy <= 0)
            {
                ChangeState(EDroneStates.FollowLead);
            }
        }

        private void IncreaseTimeSeeingEnemy() => _timeNotSeeingEnemy = TimeToFollowLeadWhenDoesntSeeEnemy;

        public void FollowLead()
        {
            if(_lead == null) return;
            target = _lead;
            _leaderBehavior._target = _lead;
        }

        public void StopShooting()
        {
            StopCoroutine(_shootRoutine);
        }

        public void Defending()
        {
            StopShooting();
            _defending = true;
        }

        public void StopDefending()
        {
            _defending = false;
        }
    }

    public enum EDroneStates
    {
        FollowLead,
        Death,
        Attack,
        Defend
    }
}