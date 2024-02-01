using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionManager : MonoBehaviour
{
    private float detectionRadius = 5f;
    public LayerMask playerLayer;

    private bool playerDetected = false;

    private GameObject detectedPlayer;

    private float moveSpeed = 4f;

    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(DetectPlayer())
        {
            Vector2 direction = (detectedPlayer.transform.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
            FacePlayer();
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
