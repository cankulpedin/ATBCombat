using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour
{
    public enum State
    {
        Waiting,
        Action,
        Win,
        Fail,
        HeroRunAway
    }

    [SerializeField]
    private State battleState;

    [SerializeField]
    private List<BattleTurn> BattleTurns = new List<BattleTurn>();

    public List<GameObject> Heroes = new List<GameObject>();
    public List<GameObject> Enemies = new List<GameObject>();

    public List<GameObject> HeroQueue = new List<GameObject>();

    private void Start()
    {
        battleState = State.Waiting;

        Heroes.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        Enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    private void Update()
    {
        switch (battleState)
        {
            case State.Waiting:
                if(Enemies.Count < 1)
                {
                    battleState = State.Win;
                }
                if(Heroes.Count < 1)
                {
                    battleState = State.Fail;
                }
                if (BattleTurns.Count > 0)
                {
                    battleState = State.Action;
                }
                break;
            case State.Action:
                Action();
                break;
            case State.HeroRunAway:
            case State.Win:
                // TODO close the battle scene
                break;
            case State.Fail:
                // TODO show game over screen
                break;
        }
    }

    private void Action()
    {
        BattleTurn currentTurn = BattleTurns[0];
        GameObject turnOwner = currentTurn.TurnOwnerGameObject;
        ActionTypes turnAction = currentTurn.actionType;

        CharacterStateMachine CSM = turnOwner.GetComponent<CharacterStateMachine>();

        switch (turnAction)
        {
            case ActionTypes.Attack:
                CSM.state = CharacterStateMachine.State.Action;
                break;
            case ActionTypes.Pass:
                RemoveCurrentTurn();
                break;
        }
    }

    public BattleTurn GetFirstTurn()
    {
        return BattleTurns[0];
    }

    public void RemoveCurrentTurn()
    {
        BattleTurns.RemoveAt(0);
        battleState = State.Waiting;
    }

    public void AddToTurnQueue(BattleTurn turn)
    {
        BattleTurns.Add(turn);
        battleState = State.Action;
    }

}
