using System.Linq;
using System.Runtime.InteropServices;
using Features.Drone.Scripts.FSM;
using Features.Flocking;
using Features.LifeSystem;
using Features.Soldier.Scripts.Domain;
using Features.Soldier.Scripts.FSM;
using Features.StateMachine;
using Features.Weapons.Bullets.Code;
using UnityEngine;

namespace Features.Drone.Scripts.Domain
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AIVision))]
    public class DroneController : MonoBehaviour, IDamageable
    {
        [SerializeField] Transform leaderTransform;
        [SerializeField] private LayerMask enemyLayer;
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
        private Transform _target;

        private void Awake()
        {
            _aiVision = GetComponent<AIVision>();
            _rigidbody = GetComponent<Rigidbody>();
            _flock = GetComponent<FlockEntity>();
            _life = new Life(50, () => ChangeState(EDroneStates.Death));

            _deathState = new DroneDeathState<EDroneStates>(this);
            _droneFollowState = new DroneFollowState<EDroneStates>(this);
            _droneAttackState = new DroneAttackState<EDroneStates>();

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
            float distaceToLeader = Vector3.Distance(transform.position, leaderTransform.position);
            return distaceToLeader > minDistanceToMove;
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
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.AddForce(Vector3.up * 10);
        }

        public void SearchForEnemy()
        {
            print("Scan");
            if (!_aiVision.SearchBy(enemyLayer).Any()) return;
            _target = _aiVision.SearchBy(enemyLayer).First().transform;
            print("Found enemy");
            ChangeState(EDroneStates.Attack);
        }
    }

    public enum EDroneStates
    {
        FollowLead,
        Death,
        Attack
    }
}