using System.Collections.Generic;
using UnityEngine;

namespace Features.Flocking
{
    public class LeaderBehavior : MonoBehaviour, IFlockBehavior
    {
        [HideInInspector] public Transform _target;
        public float leaderWeight;
        public float sanityDistance;
        public float radiusSanityDistance;

        public Vector3 GetDir(List<IFlockEntity> entities, IFlockEntity entity)
        {
            float weight = leaderWeight;
            float distance = Vector3.Distance(_target.position, entity.Position);

            if (distance >= radiusSanityDistance + sanityDistance)
            {
                weight = leaderWeight;
            }
            else if (distance < radiusSanityDistance - sanityDistance)
            {
                weight = leaderWeight * -1;
            }


            return (_target.position - entity.Position).normalized * weight;
        }

    }
}