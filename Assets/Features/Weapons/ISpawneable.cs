using UnityEngine;

namespace Features.Weapons
{
    public interface ISpawneable
    {
        void setSpeed(float speed);
        void setLayerMask(LayerMask layerMask);
    }
}