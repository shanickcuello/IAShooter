using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBehavior : MonoBehaviour, IFlockBehavior
{
    public float leaderWeight;
    public Transform target;

    //Distance to leader 
    public float sanityDistance;
    public float radiusSanityDistance;


    public Vector3 GetDir(List<IFlockEntity> entities, IFlockEntity entity)
    {
        float weight = leaderWeight;
        float distance = Vector3.Distance(target.position, entity.Position);

        if (distance >= radiusSanityDistance + sanityDistance)
        {
            weight = leaderWeight;
        }
        else if (distance < radiusSanityDistance - sanityDistance)
        {
            weight = leaderWeight * -1;
        }


        return (target.position - entity.Position).normalized * weight;
    }

}