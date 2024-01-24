using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    public enum PerformAction
    {
        WAITING,
        TAKE_ACTION,
        PERFORM_ACTION
    }

    public enum HeroGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }

    public HeroGUI HeroInput;

    public PerformAction battleState;

    public List<HandleTurns> PerformList = new List<HandleTurns>();

    public List<GameObject> HeroesInBattle = new List<GameObject>();
    public List<GameObject> EnemiesInBattle = new List<GameObject>();
    public List<GameObject> HeroesToManage = new List<GameObject>();

    private HandleTurns HeroChoice;

    void Start()
    {
        battleState = PerformAction.WAITING;

        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
    }

    void Update()
    {
        switch (battleState)
        {
            case PerformAction.WAITING:
                if(PerformList.Count > 0)
                {
                    battleState = PerformAction.TAKE_ACTION;
                }
                break;
            case PerformAction.TAKE_ACTION:
                GameObject performer = PerformList[0].AttackerGameObject;
                Debug.Log(performer == null);
                
                EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                ESM.Target = PerformList[0].TargetGameObject;
                ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                
                battleState = PerformAction.PERFORM_ACTION;
                break;
            case PerformAction.PERFORM_ACTION: 
                break;
        }
    }

    public void AddAction(HandleTurns turnInput)
    {
        PerformList.Add(turnInput);
    }

    
}
