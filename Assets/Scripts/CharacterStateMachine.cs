using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStateMachine : MonoBehaviour
{
    public enum State
    {
        Processing,
        SelectingAction,
        SelectingTarget,
        AddToList,
        WaitingForTurn,
        Action,
        WaitingForOtherActionFinish,
        Death
    }

    public State state;

    public CharacterData characterData;

    private float CurrentCooldown = 0f;
    private float MaxCooldown;

    public Image ProgressBar;
    public GameObject Panel;
    public GameObject SelectionTriangle;

    private string CharacterType;
    private string HeroAction;

    private BattleStateMachine BSM;

    private void Start()
    {
        MaxCooldown = characterData.BaseCooldown;
        state = State.Processing;

        CharacterType = gameObject.tag;

        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
    }

    private void Update()
    {
        switch (state) { 
            case State.Processing:
                Processing();
                break;
            case State.SelectingAction:
                SelectingAction();
                break;
            case State.SelectingTarget:
                StartCoroutine(SelectTarget());

                break;
        }
    }

    private void Processing()
    {
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
            }

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

    public void SetNewState(string action)
    {
        HeroAction = action;

        GameObject initialTarget = BSM.Enemies[0];
        Vector3 initialTargetLocation = initialTarget.transform.position;
        Instantiate(SelectionTriangle,
            new Vector3(initialTargetLocation.x, initialTargetLocation.y + initialTarget.GetComponent<BoxCollider2D>().bounds.size.y, initialTargetLocation.z), new Quaternion(0f, 0f, 180f, 0f));

        state = State.SelectingTarget;
    }

    private IEnumerator SelectTarget()
    {
        List<GameObject> targetList = new List<GameObject>();
        targetList.AddRange(BSM.Enemies);
        targetList.AddRange(BSM.Heroes);

        int index = 0;

        while(TargetSelection(ref index, targetList.Count)) {
            GameObject currentTarget = targetList[index];
            Vector3 currentTargetLocation = currentTarget.transform.position;

            SelectionTriangle.transform.position = new Vector3(currentTargetLocation.x, 
                currentTargetLocation.y + currentTarget.GetComponent<BoxCollider2D>().bounds.size.y, currentTargetLocation.z);
            yield return null;
        }

        Debug.Log(index);
        
        
        yield return index;

    }

    private bool TargetSelection(ref int index, int targetListCount)
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            return false;
        }

        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0)
        {
            index++;
        }
        else if (Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") < 0)
        {
            index--;
        }

        Debug.Log("index" + index);


        if (index < 0)
        {
            index = targetListCount - 1;
        }
        else if (index > targetListCount - 1)
        {
            index = 0;
        }

        return true;

    }
}
