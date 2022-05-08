using System.Collections.Generic;
using UnityEngine;

namespace Features.Theta_Path
{
    public class ThetaPath : MonoBehaviour
    {
        public LayerMask obstacleMask;
        public LayerMask nodeMask;
        public float radius;
        public Vector3 offset;
        public PathNode init; 
        public PathNode finit;
        List<PathNode> _list;

        Theta<PathNode> _theta = new Theta<PathNode>();
        float playerHeight;
        public List<PathNode> PathFindingTheta(Vector3 targetPosition, Vector3 myPosition)
        {        
            init = GetClosestNodeTo(myPosition, targetPosition);
            finit = GetClosestNodeTo(targetPosition, myPosition);
            playerHeight = myPosition.y;
            _list = _theta.Run(init, Satisfies, GetNeighbours, GetCost, Heuristic, InSight);
            return _list;
        }    
        private PathNode GetClosestNodeTo(Vector3 origin, Vector3 fin)
        {        
            int index = 0;
            var hitColliders = Physics.OverlapSphere(origin, radius, nodeMask);
            PathNode closest = hitColliders[0].GetComponent<PathNode>();
            foreach (var hit in hitColliders)
            {
                PathNode node = hit.GetComponent<PathNode>();
                if (node)
                {
                    if (index == 0)
                    {
                        closest = node;
                        index = 1;
                    }

                    if (Vector3.Distance(origin, node.transform.position) < Vector3.Distance(origin, closest.transform.position)
                        && Vector3.Distance(fin, node.transform.position) < Vector3.Distance(fin, closest.transform.position))
                    {
                        closest = node;
                    }
                }
            }
            return closest;
        }
        bool InSight(PathNode gP, PathNode gC)
        {
            Vector3 dir = gC.transform.position - gP.transform.position;
            dir.y = 0;
            Vector3 origin = new Vector3(gP.transform.position.x, playerHeight, gP.transform.position.z);
            if (Physics.Raycast(origin, dir.normalized, dir.magnitude, obstacleMask))
            {
                return false;
            }       
            return true;
        }
        float Heuristic(PathNode curr)
        {
            return Vector3.Distance(curr.transform.position, finit.transform.position);
        }
        float GetCost(PathNode from, PathNode to)
        {
            return Vector3.Distance(from.transform.position, to.transform.position);
        }
        List<PathNode> GetNeighbours(PathNode curr)
        {
            return curr.neightbourds;
        }
        bool Satisfies(PathNode curr)
        {
            return curr == finit;
        }
        private void OnDrawGizmos()
        {

            Gizmos.color = Color.red;
            if (init != null)
                Gizmos.DrawSphere(init.transform.position + offset, 0.1f);
            if (finit != null)
                Gizmos.DrawSphere(finit.transform.position + offset, 0.3f);
            if (_list != null)
            {
                Gizmos.color = Color.blue;
                foreach (var item in _list)
                {
                    if (item != init && item != finit)
                        Gizmos.DrawSphere(item.transform.position + offset, 0.3f);
                }            
            }
        }
    }
}
