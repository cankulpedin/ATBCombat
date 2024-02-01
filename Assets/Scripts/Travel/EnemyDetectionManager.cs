using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDetectionManager : MonoBehaviour
{
    private float detectionRadius = 5f;
    public LayerMask playerLayer;

    private GameObject detectedPlayer;

    private float moveSpeed = 3f;

    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;

        agent.speed = moveSpeed;
    }

    void Update()
    {
        if(DetectPlayer())
        {
            FacePlayer();
            agent.SetDestination(detectedPlayer.transform.position);
        }
    }

    void FacePlayer()
    {
        float relativePosition = transform.position.x - detectedPlayer.transform.position.x;
        if(relativePosition > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        } else if(relativePosition < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private bool DetectPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                detectedPlayer = collider.gameObject;
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
