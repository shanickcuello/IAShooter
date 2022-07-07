using System.Collections.Generic;
using Features.Drone.Scripts;
using UnityEngine;

namespace Features.Flocking
{
    public interface IFlockBehavior
    {
        Vector3 GetDir(List<IFlockEntity> entities, IFlockEntity entity);
    }
}