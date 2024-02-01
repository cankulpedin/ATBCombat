using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypointMovement : MonoBehaviour
{
    public GameObject[] waypoints;
    private int currentWaypoint = 0;

    private void Update()
    {
        if(currentWaypoint >= waypoints.Length) {
            currentWaypoint = 0; 
        }

    }

    private void MoveTowardsWaypoint()
    {
        Vector2 targetWaypointPosition = waypoints[currentWaypoint].transform.position;
        
    }
}
