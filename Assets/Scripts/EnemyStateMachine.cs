using System.Collections;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyData enemy;

    private BattleStateMachine BSM;

    public enum TurnState
    {
        PROCCESSING, // time is ticking
        CHOOSE_ACTION,
        WAITING, // idle state
        ACTION, // doing action
        DEAD
    }

    public TurnState currentState;

    private float currentCooldown = 0f;
    private float maxCooldown = 2f;
    private float animationSpeed = 5f;

    private Vector3 initialPosition;

    private bool actionStarted = false;

    public GameObject Target;

    void Start()
    {
        currentState = TurnState.PROCCESSING;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        initialPosition = transform.position;
    }

    void Update()
    {
        switch (currentState)
        {
            case TurnState.PROCCESSING:
                UpdateProgressBar();
                break;
            case TurnState.CHOOSE_ACTION:
                ChooseAction();
                break;
            case TurnState.WAITING:
                break;
            case TurnState.ACTION:
                StartCoroutine(Action());
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
            currentState = TurnState.CHOOSE_ACTION;
        }
    }

    private void ChooseAction()
    {
        HandleTurns myAttack = new HandleTurns();
        myAttack.Attacker = enemy.name;
        myAttack.AttackerGameObject = gameObject;
        myAttack.TargetGameObject = BSM.HeroesInBattle[Random.Range(0,BSM.HeroesInBattle.Count)];
        
        BSM.AddAction(myAttack);

        currentState = TurnState.WAITING;
    }

    private IEnumerator Action()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        Vector3 targetPosition = new Vector3(Target.transform.position.x + 0.5f, 
            Target.transform.position.y,
            Target.transform.position.z);
        
        while(MoveTowards(targetPosition))
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // TODO do damage

        Vector3 firstPosition = initialPosition;
        while (MoveTowards(firstPosition))
        {
            yield return null;
        }

        BSM.PerformList.RemoveAt(0);
        BSM.battleState = BattleStateMachine.PerformAction.WAITING;

        actionStarted = false;

        currentCooldown = 0f;
        currentState = TurnState.PROCCESSING;
    }

    private bool MoveTowards(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position,
            target, 
            animationSpeed * Time.deltaTime)); 
    }
}
