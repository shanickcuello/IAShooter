using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    private List<Transform> myWayPoints = new List<Transform>();

    int _currentIndex = -1;
    int _indexDirection = 1;

    public void CreatePoints()
    {
        Transform[] childrenTransform = GetComponentsInChildren<Transform>();
        foreach (Transform t in childrenTransform)
        {
            myWayPoints.Add(t);
        }
    }
    public int Count()
    {
        return myWayPoints.Count;
    }
    public Transform NextWayPoint()
    {
        Transform _currentPoint;

        _currentIndex += _indexDirection;

        if ((_indexDirection == 1) && (_currentIndex > myWayPoints.Count - 1))
        {
            _indexDirection = -1;
            _currentIndex += _indexDirection - 1;
        }

        if ((_indexDirection == -1) && (_currentIndex < 0))
        {
            _indexDirection = 1;
            _currentIndex += _indexDirection + 1;
        }

        _currentPoint = myWayPoints[_currentIndex];

        return _currentPoint;
    }
}