using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private string CharacterType;
    [SerializeField]
    private string HeroAction;

    private BattleStateMachine BSM;

    private int TargetIndex = 0;

    private void Start()
    {
        MaxCooldown = characterData.BaseCooldown;
        state = State.Processing;

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
            }
        }
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
            newTurn.TurnOwnerName = gameObject.name;
            newTurn.TurnOwnerGameObject = gameObject;
            newTurn.targetGameObject = targetsList[TargetIndex];
            BSM.AddToTurnQueue(newTurn);
            BSM.HeroQueue.RemoveAt(0);

            Destroy(SelectionTriangleClone, 0.5f);
            SelectionTriangleClone = null;

            TargetIndex = 0;
            state = State.Processing;
            CurrentCooldown = 0;
            Panel.gameObject.SetActive(false);
            HeroAction = null;
        }
    }

    public void SetNewState(string action)
    {
        if(HeroAction == null || HeroAction == "") {
            HeroAction = action;

            GameObject initialTarget = BSM.Enemies[0];
            Vector3 initialTargetLocation = initialTarget.transform.position;
            SelectionTriangleClone = Instantiate(SelectionTriangle,
                new Vector3(initialTargetLocation.x, initialTargetLocation.y + initialTarget.GetComponent<BoxCollider2D>().bounds.size.y, initialTargetLocation.z), new Quaternion(0f, 0f, 180f, 0f));

            state = State.SelectingTarget;
        }
    }
}
