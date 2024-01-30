using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionManager : MonoBehaviour
{
    private float detectionRadius = 5f;
    public LayerMask playerLayer;

    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer);
        foreach (Collider2D collider in colliders)
        {
            if(collider.CompareTag("Player"))
            {
                // TODO Chase player
            }
        }
    }
}
