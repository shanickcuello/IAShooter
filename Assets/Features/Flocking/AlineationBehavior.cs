using System.Collections.Generic;
using Features.Drone.Scripts;
using UnityEngine;

namespace Features.Flocking
{
    public class AlineationBehavior : MonoBehaviour, IFlockBehavior
    {
        public float alineationWeight;
        public Vector3 GetDir(List<IFlockEntity> entities, IFlockEntity entity)
        {
            var dir = Vector3.zero;
            for (int i = 0; i < entities.Count; i++)
            {
                dir += entities[i].Direction;
            }
            dir /= entities.Count;
            return dir.normalized * alineationWeight;
        }
    }
}