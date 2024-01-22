using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour
{
    public HeroData Hero;

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

    public Image progressBar;

    void Start()
    {
        currentState = TurnState.PROCCESSING;
    }

    void Update()
    {
        switch (currentState) {
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
        float calculatedCooldown = currentCooldown / maxCooldown;

        progressBar.transform.localScale = new Vector3(Mathf.Clamp(calculatedCooldown,0,1),
            progressBar.transform.localScale.y,
            progressBar.transform.localScale.z);

        if(currentCooldown > maxCooldown)
        {
            currentState = TurnState.ADD_TO_LIST;
        }
    }
}
