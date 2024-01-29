using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStateMachine : MonoBehaviour
{
    public enum State
    {
        Processing, // hero & enemy
        WaitingForTurn, // hero
        SelectingAction,  // hero
        SelectingTarget, // hero
        WaitingForAct, // hero & enemy
        Action, // hero & enemy
        WaitingForOtherActionFinish, // hero & enemy
        Death // hero & enemy
    }

    public State state;

    public CharacterData characterData;

    private float CurrentCooldown = 0f;
    private float MaxCooldown;
    private float AnimationSpeed = 5f;

    public Image ProgressBar;

    public GameObject Panel;
    public GameObject SelectionTriangle;
    private GameObject SelectionTriangleClone;

    public ActionTypes ActionType; // this is only for hero, to arrange menus
    public ActionTypes[] Targetables = { ActionTypes.Attack, ActionTypes.Magic };

    private string CharacterType;

    private BattleStateMachine BSM;

    private int TargetIndex = 0;

    private Vector3 InitialPosition;

    private bool ActionStarted = false;

    public Animator animator;

    private void Start()
    {
        MaxCooldown = characterData.BaseCooldown;
        state = State.Processing;
        ActionType = ActionTypes.Wait;

        animator = GetComponent<Animator>();

        InitialPosition = transform.position;

        CharacterType = gameObject.tag;

        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Processing:
                Processing();
                break;
            case State.WaitingForTurn:
                CheckForTurn();
                break;
            case State.SelectingAction:
                SelectingAction();
                break;
            case State.SelectingTarget:
                SelectTarget();
                break;
            case State.WaitingForAct:
                break;
            case State.Action:
                StartCoroutine(Action());
                break;
        }
    }

    private IEnumerator Action()
    {
        if (ActionStarted)
        {
            yield break;
        }

        ActionStarted = true;

        BattleTurn currentTurn = BSM.GetFirstTurn();
        GameObject target = currentTurn.targetGameObject;
        ActionTypes turnAction = currentTurn.actionType;

        switch (turnAction)
        {
            case ActionTypes.Attack:
                animator.SetBool("attacking", true);

                while (MoveTowards(target.transform.position)) yield return null;

                animator.SetBool("attacking", false);

                target.GetComponent<CharacterStateMachine>().characterData.HP -= characterData.baseDamage;

                animator.SetBool("attacking", true);

                yield return new WaitForSeconds(1f);

                while (MoveTowards(InitialPosition)) yield return null;

                animator.SetBool("attacking", false);

                break;
        }

        BSM.RemoveCurrentTurn();

        ActionStarted = false;
        CurrentCooldown = 0f;
        state = State.Processing;
    }

    private bool MoveTowards(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, AnimationSpeed * Time.deltaTime));
    }

    private void Processing()
    {
        if (SelectionTriangleClone != null)
        {
            Destroy(SelectionTriangleClone);
            SelectionTriangleClone = null;
        }

        CurrentCooldown += Time.deltaTime;

        float calculatedCooldown = CurrentCooldown / MaxCooldown;

        if (CharacterType == "Hero")
        {
            ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calculatedCooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        }

        if (CurrentCooldown > MaxCooldown)
        {
            if (CharacterType == "Hero")
            {
                BSM.HeroQueue.Add(gameObject);
                state = State.WaitingForTurn;
            }
            else if (CharacterType == "Enemy")
            {
                SelectEnemyAction();
            }
        }
    }

    private void SelectEnemyAction()
    {
        int randomIndex = UnityEngine.Random.Range(1, Enum.GetNames(typeof(ActionTypes)).Length);
        ActionTypes selectedAction = (ActionTypes)randomIndex;

        if (selectedAction == ActionTypes.Pass)
        {
            state = State.Processing;
            return;
        }
        else if (Array.Exists(Targetables, element => element == selectedAction))
        {
            int randomTargetIndex = UnityEngine.Random.Range(0, BSM.Heroes.Count);
            GameObject randomTarget = BSM.Heroes[randomTargetIndex];

            BattleTurn turn = new BattleTurn();
            turn.TurnOwnerName = characterData.name;
            turn.TurnOwnerGameObject = gameObject;
            turn.targetGameObject = randomTarget;
            turn.actionType = selectedAction;
            BSM.AddToTurnQueue(turn);
        }

        state = State.WaitingForAct;
        return;
    }

    private void CheckForTurn()
    {
        if (BSM.HeroQueue[0].Equals(gameObject))
        {
            state = State.SelectingAction;
        }
    }

    private void SelectingAction()
    {
        if (CharacterType == "Hero")
        {
            Panel.gameObject.SetActive(true);
        }
    }

    private void ChangeSelectionTrianglePosition(Vector3 position, float elementSizeY)
    {
        SelectionTriangleClone.transform.position = new Vector3(position.x, position.y + elementSizeY, position.z);
    }

    private void SelectTarget()
    {
        List<GameObject> targetsList = new List<GameObject>();
        targetsList.AddRange(BSM.Enemies);
        targetsList.AddRange(BSM.Heroes);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit.collider != null)
            {
                TargetIndex = targetsList.FindIndex(element => element.gameObject == hit.collider.gameObject);
                Vector3 targetLocation = targetsList[TargetIndex].transform.position;

                ChangeSelectionTrianglePosition(targetLocation, targetsList[TargetIndex].GetComponent<BoxCollider2D>().bounds.size.y);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            TargetIndex = TargetIndex == targetsList.Count - 1 ? 0 : TargetIndex + 1;
            Vector3 targetLocation = targetsList[TargetIndex].transform.position;

            ChangeSelectionTrianglePosition(targetLocation, targetsList[TargetIndex].GetComponent<BoxCollider2D>().bounds.size.y);

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TargetIndex = TargetIndex == 0 ? targetsList.Count - 1 : TargetIndex - 1;
            Vector3 targetLocation = targetsList[TargetIndex].transform.position;

            ChangeSelectionTrianglePosition(targetLocation, targetsList[TargetIndex].GetComponent<BoxCollider2D>().bounds.size.y);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            BattleTurn newTurn = new BattleTurn();
            newTurn.TurnOwnerName = characterData.name;
            newTurn.TurnOwnerGameObject = gameObject;
            newTurn.targetGameObject = targetsList[TargetIndex];
            newTurn.actionType = ActionType;

            BSM.AddToTurnQueue(newTurn);
            BSM.HeroQueue.RemoveAt(0);

            Destroy(SelectionTriangleClone, 0.5f);
            SelectionTriangleClone = null;

            TargetIndex = 0;
            state = State.WaitingForAct;
            Panel.gameObject.SetActive(false);
            ActionType = ActionTypes.Wait;
        }
    }

    public void SetNewState(ActionTypes action, MagicTypes magic= MagicTypes.Null)
    {
        if (ActionType == ActionTypes.Wait)
        {
            ActionType = action;

            if (Array.Exists(Targetables, element => element == action))
            {
                GameObject initialTarget = BSM.Enemies[0];
                Vector3 initialTargetLocation = initialTarget.transform.position;
                SelectionTriangleClone = Instantiate(SelectionTriangle,
                    new Vector3(initialTargetLocation.x, initialTargetLocation.y + initialTarget.GetComponent<BoxCollider2D>().bounds.size.y, initialTargetLocation.z), new Quaternion(0f, 0f, 180f, 0f));

                state = State.SelectingTarget;
            }
        }
    }
}
