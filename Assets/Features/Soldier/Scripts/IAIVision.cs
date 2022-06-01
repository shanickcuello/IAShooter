using System.Collections.Generic;
using UnityEngine;

namespace Features.Soldier.Scripts
{
    public interface IAIVision
    {
        List<GameObject> SearchBy(LayerMask layerMask);
    }
}