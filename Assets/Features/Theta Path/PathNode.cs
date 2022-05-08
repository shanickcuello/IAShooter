using System.Collections.Generic;
using UnityEngine;

namespace Features.Theta_Path
{
    public class PathNode : MonoBehaviour
    {
        public List<PathNode> neightbourds;
        public LayerMask mask;
        public float overlapRadius;
        public void GetNeightbors()
        {        
            var hitColliders = Physics.OverlapSphere(transform.position, overlapRadius, mask);
            foreach (var hit in hitColliders)
            {            
                PathNode neightbor = hit.GetComponent<PathNode>();
                if (neightbor)
                {
                    if (neightbor.name != name)
                        neightbourds.Add(neightbor);
                }
            }
        }
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, overlapRadius);
        }
    }
}