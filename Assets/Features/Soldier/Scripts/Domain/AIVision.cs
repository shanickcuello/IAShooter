using System.Collections.Generic;
using UnityEngine;

namespace Features.Soldier.Scripts.Domain
{
    [ExecuteInEditMode]
    public class AIVision : MonoBehaviour, IAIVision
    {
        [SerializeField] protected Transform _myTransform;

        [SerializeField] protected float visionDistance = 10;
        [SerializeField] protected float visionAngle = 30;
        [SerializeField] protected float visionHeight = 1;
        [SerializeField] protected Color visionColor = Color.red;
        [SerializeField] protected int scanFrecuency = 30;
        [SerializeField] List<GameObject> gameObjectsOnSight = new List<GameObject>();
        [SerializeField] protected LayerMask obstacleLayer;

        private Collider[] _colliders = new Collider[50];
        Mesh _mesh;
        private int scanCount;
        private float scanInterval;
        private float scanTimer;

        protected virtual void Awake()
        {
            _myTransform = GetComponent<Transform>();
        }

        private void Start()
        {
            scanInterval = 1 / scanFrecuency;
        }


        public List<GameObject> SearchBy(LayerMask layerMask)
        {
            scanTimer -= Time.deltaTime;
            if (!(scanTimer <= 0))
                return new List<GameObject>();
            scanTimer = scanInterval;
            return Scan(layerMask);
        }

        private bool IsInSight(GameObject go)
        {
            var origin = transform.position;
            var destination = go.transform.position;
            var direction = destination - origin;
            if (direction.y < 0 || direction.y > visionHeight)
                return false;

            var deltaAngle = Vector3.Angle(transform.forward, direction);
            if (deltaAngle > visionAngle)
            {
                return false;
            }

            origin.y += visionHeight / 2;
            // destination.y = origin.y;
            if (Physics.Linecast(origin, destination, out var hit, obstacleLayer))
            {
                return false;
            }

            return true;
        }

        private List<GameObject> Scan(LayerMask layerGameObjectToSearch)
        {
            scanCount = Physics.OverlapSphereNonAlloc(transform.position, visionDistance, _colliders,
                layerGameObjectToSearch,
                QueryTriggerInteraction.Collide);
            gameObjectsOnSight.Clear();
            for (int i = 0; i < scanCount; i++)
            {
                GameObject obj = _colliders[i].gameObject;
                if (IsInSight(obj))
                    gameObjectsOnSight.Add(obj);
            }

            return gameObjectsOnSight;
        }

        private void OnValidate()
        {
            _mesh = CreateMesh();
            scanInterval = 1 / scanFrecuency;
        }

        private void OnDrawGizmos()
        {
            if (_mesh)
            {
                Gizmos.color = visionColor;
                Gizmos.DrawMesh(_mesh, _myTransform.position, _myTransform.rotation);
            }

            Gizmos.DrawWireSphere(transform.position, visionDistance);
            for (int i = 0; i < scanCount; i++)
            {
                if (_colliders[i] == null)
                    continue;
                Gizmos.DrawSphere(_colliders[i].transform.position, 0.2f);
            }

            Gizmos.color = Color.green;
            foreach (var go in gameObjectsOnSight)
            {
                if (go == null)
                    continue;
                Gizmos.DrawSphere(go.transform.position, go.GetComponent<Collider>().bounds.extents.magnitude);
            }
        }

        Mesh CreateMesh()
        {
            var mesh = new Mesh();

            var segments = 10;
            var numberOfTriangles = (segments * 4) + 2 + 2;
            var numberOfVertices = numberOfTriangles * 3;

            var vertices = new Vector3[numberOfVertices];
            var triangles = new int[numberOfVertices];

            var bottomCenter = Vector3.zero;
            var bottomLeft = Quaternion.Euler(0, -visionAngle, 0) * Vector3.forward * visionDistance;
            var bottomRight = Quaternion.Euler(0, visionAngle, 0) * Vector3.forward * visionDistance;

            var topCenter = bottomCenter + Vector3.up * visionHeight;
            var topRight = bottomRight + Vector3.up * visionHeight;
            var topLeft = bottomLeft + Vector3.up * visionHeight;

            int vert = 0;
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = topLeft;

            vertices[vert++] = topLeft;
            vertices[vert++] = topCenter;
            vertices[vert++] = bottomCenter;

            vertices[vert++] = bottomCenter;
            vertices[vert++] = topCenter;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomCenter;

            float currentAngle = -visionAngle;
            float deltaAngle = (visionAngle * 2) / segments;
            for (int i = 0; i < segments; i++)
            {
                bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * visionDistance;
                bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * visionDistance;

                topRight = bottomRight + Vector3.up * visionHeight;
                topLeft = bottomLeft + Vector3.up * visionHeight;

                vertices[vert++] = bottomLeft;
                vertices[vert++] = bottomRight;
                vertices[vert++] = topRight;

                vertices[vert++] = topRight;
                vertices[vert++] = topLeft;
                vertices[vert++] = bottomLeft;


                vertices[vert++] = topCenter;
                vertices[vert++] = topLeft;
                vertices[vert++] = topRight;
                vertices[vert++] = bottomCenter;
                vertices[vert++] = bottomRight;
                vertices[vert++] = bottomLeft;
                currentAngle += deltaAngle;
            }


            for (var i = 0; i < numberOfVertices; i++)
            {
                triangles[i] = i;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}