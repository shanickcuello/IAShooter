using System.Collections;
using System.Linq;
using Features.Drone.Scripts.FSM;
using Features.Flocking;
using Features.LifeSystem;
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
        private Transform _lead;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private GameObject _weaponPrefab;
        private AIVision _aiVision;
        public float minDistanceToMove;
        MovementBehavior _movementBehavior;
        private FlockEntity _flock;

        private DroneFollowState<EDroneStates> _droneFollowState;
        private DroneDeathState<EDroneStates> _deathState;
        private DroneAttackState<EDroneStates> _droneAttackState;

        private Fsm<EDroneStates> _fsm;
        private Rigidbody _rigidbody;
        private Life _life;
        private LeaderBehavior _leaderBehavior;
        private IWeapon _weapon;

        private void Awake()
        {
            _lead = target;
            _weapon = _weaponPrefab.GetComponent<IWeapon>();
            _aiVision = GetComponent<AIVision>();
            _rigidbody = GetComponent<Rigidbody>();
            _flock = GetComponent<FlockEntity>();
            _life = new Life(50, () => ChangeState(EDroneStates.Death));
            _leaderBehavior = GetComponent<LeaderBehavior>();
            _leaderBehavior._target = target;

            _deathState = new DroneDeathState<EDroneStates>(this);
            _droneFollowState = new DroneFollowState<EDroneStates>(this);
            _droneAttackState = new DroneAttackState<EDroneStates>(this);

            _droneFollowState.AddTransitionState(EDroneStates.Death, _deathState);
            _droneFollowState.AddTransitionState(EDroneStates.Attack, _droneAttackState);

            _droneAttackState.AddTransitionState(EDroneStates.Death, _deathState);
        }

        void Start()
        {
            _movementBehavior = GetComponent<MovementBehavior>();
            _fsm = new Fsm<EDroneStates>(_droneFollowState);
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
            _life.DecreaseLife(damagePower);
        }

        private void ChangeState(EDroneStates state)
        {
            _fsm.Transition(state);
        }

        public void Shutdown()
        {
            _rigidbody.mass = 1;
            _rigidbody.useGravity = true;
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.AddForce(Vector3.up * 80);
            _weapon.ShutdownLight();
        }

        public void SearchForEnemy()
        {
            if (!_aiVision.SearchBy(enemyLayer).Any()) return;
            target = _aiVision.SearchBy(enemyLayer).First().transform;
            Debug.Log("found enemy");
            ChangeState(EDroneStates.Attack);
        }

        public void ChangeFollow()
        {
            if (target == null) return;
            _leaderBehavior._target = target;
        }

        public void ShootIntermittent()
        {
            StartCoroutine(ShootIntermittentCoroutine(target));
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
            Debug.Log("i shoot");
            _weapon.Fire(target);
        }
    }

    public enum EDroneStates
    {
        FollowLead,
        Death,
        Attack
    }
}