using System.Collections.Generic;
using UnityEngine;

namespace Features.Soldier.Scripts.Domain
{
    public interface IAIVision
    {
        List<GameObject> SearchBy(LayerMask layerMask);
    }
}