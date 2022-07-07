using UnityEngine;

namespace Features.Flocking
{
    public interface IFlockEntity
    {
        Vector3 Direction { get; }
        Vector3 Position { get; }
    }
}