using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{

    public EnemyDataTravelMode enemyData;

    void Start()
    {
        enemyData.currentState = EnemyDataTravelMode.EnemyState.Idle;
    }

    void Update()
    {
        
    }
}
