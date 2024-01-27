using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    public enum State
    {
        Waiting,
        Action,
        Win,
        Fail
    }

    public State battleState;

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
                break;
            case State.Action:
                break;
            case State.Win:
                break;
            case State.Fail:
                break;
        }
    }

    public void AddToTurnQueue(BattleTurn turn)
    {
        BattleTurns.Add(turn);
    }

}
