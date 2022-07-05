using UnityEngine;

public class PawnController : MonoBehaviour
{
    Vector3 _dir = Vector3.zero;
    EntityModel _entityModel;
    private FlockEntity flock;

    public float minDistanceToMove, maxDistanceToMove;

    [SerializeField] float distanceToMoveFromTarget;
    [SerializeField] Transform leaderTransform;

    private void Awake()
    {
        flock = GetComponent<FlockEntity>();
        minDistanceToMove = 1.3f;
        maxDistanceToMove = 2.3f;
    }
    void Start()
    {
        _entityModel = GetComponent<EntityModel>();
    }
    void Update()
    {
        _dir = flock.GetDir();

        float distaceToLeader = Vector3.Distance(transform.position, leaderTransform.position);
        if (distaceToLeader < minDistanceToMove || distaceToLeader > maxDistanceToMove)
        {
            _entityModel.Move(_dir);
        }

    }
}