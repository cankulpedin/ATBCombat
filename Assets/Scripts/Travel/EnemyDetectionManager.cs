using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyDetectionManager : MonoBehaviour
{
    public LayerMask playerLayer;

    private GameObject detectedPlayer;

    NavMeshAgent agent;

    [SerializeField]
    EnemyDataTravelMode enemyData;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;

        agent.speed = enemyData.speed;
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyData.detectionRadius, playerLayer);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            agent.isStopped = true;
            DataManager.instance.enemyId = enemyData.enemyId;
            // TODO add transiton animation
            SceneManager.LoadScene("BattleScene");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.detectionRadius);
    }
}
