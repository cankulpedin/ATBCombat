using System;
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

    public Image ProgressBar;
    public GameObject Panel;
    public GameObject SelectionTriangle;
    private GameObject SelectionTriangleClone;

    public ActionTypes ActionType; // this is only for hero, to arrange menus
    public ActionTypes[] Targetables = { ActionTypes.Attack, ActionTypes.Magic };

    private string CharacterType;

    private BattleStateMachine BSM;

    private int TargetIndex = 0;

    private void Start()
    {
        MaxCooldown = characterData.BaseCooldown;
        state = State.Processing;
        ActionType = ActionTypes.Wait;

        CharacterType = gameObject.tag;

        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
    }

    private void Update()
    {
        Debug.Log(state);
        switch (state) { 
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
                // action()
                CurrentCooldown = 0;
                break;
        }
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

        if(CurrentCooldown > MaxCooldown)
        {
            if(CharacterType == "Hero")
            {
                BSM.HeroQueue.Add(gameObject);
                state = State.WaitingForTurn;
            } else if(CharacterType == "Enemy")
            {
                SelectEnemyAction();
            }
        }
    }

    private void SelectEnemyAction()
    {
        int randomIndex = UnityEngine.Random.Range(1, Enum.GetNames(typeof(ActionTypes)).Length);
        Debug.Log("action "+randomIndex);
        ActionTypes selectedAction = (ActionTypes)randomIndex;

        if(selectedAction == ActionTypes.Pass)
        {
            state = State.Processing;
            return;
        } else if(selectedAction == ActionTypes.Flee)
        {
            // TODO Flee action, should be immediate
            // will be put in first place in queue by some random chance
        } else if(Array.Exists(Targetables, element => element == selectedAction)) {
            Debug.Log("girdi " + BSM.Heroes.Count);
            int randomTargetIndex = UnityEngine.Random.Range(0, BSM.Heroes.Count);
            Debug.Log(randomTargetIndex);
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

    // TODO add selection with mouse
    private void SelectTarget()
    {
        List<GameObject> targetsList = new List<GameObject>();
        targetsList.AddRange(BSM.Enemies);
        targetsList.AddRange(BSM.Heroes);

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            TargetIndex = TargetIndex == targetsList.Count - 1 ? 0 : TargetIndex + 1;
            Vector3 targetLocation = targetsList[TargetIndex].transform.position;

            SelectionTriangleClone.transform.position = new Vector3(targetLocation.x, targetLocation.y + targetsList[TargetIndex].GetComponent<BoxCollider2D>().bounds.size.y, targetLocation.z);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TargetIndex = TargetIndex == 0 ? targetsList.Count - 1 : TargetIndex - 1;
            Vector3 targetLocation = targetsList[TargetIndex].transform.position;

            SelectionTriangleClone.transform.position = new Vector3(targetLocation.x, targetLocation.y + targetsList[TargetIndex].GetComponent<BoxCollider2D>().bounds.size.y, targetLocation.z);

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

    public void SetNewState(int action)
    {
        if(ActionType == ActionTypes.Wait) {
            ActionType = (ActionTypes)action;

            if(Array.Exists(Targetables, element=> element == (ActionTypes)action))
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
