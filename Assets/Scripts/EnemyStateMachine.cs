using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyData enemy;

    public enum TurnState
    {
        PROCCESSING, // time is ticking
        ADD_TO_LIST, // Add hero to list
        WAITING, // IDLE
        SELECTING, // selecting action
        ACTION, // doing action
        DEAD
    }

    public TurnState currentState;

    private float currentCooldown = 0f;
    private float maxCooldown = 5f;

    void Start()
    {
        currentState = TurnState.PROCCESSING;
    }

    void Update()
    {
        switch (currentState)
        {
            case TurnState.PROCCESSING:
                UpdateProgressBar();
                break;
            case TurnState.ADD_TO_LIST:
                break;
            case TurnState.WAITING:
                break;
            case TurnState.SELECTING:
                break;
            case TurnState.ACTION:
                break;
            case TurnState.DEAD:
                break;
        }
    }

    private void UpdateProgressBar()
    {
        currentCooldown = currentCooldown + Time.deltaTime;

        if (currentCooldown > maxCooldown)
        {
            currentState = TurnState.ADD_TO_LIST;
        }
    }
}
