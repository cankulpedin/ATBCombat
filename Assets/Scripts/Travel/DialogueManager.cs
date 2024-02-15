using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    OutcomeManager outcomeManager;
    GameManager gameManager;

    private Dialogue.DialogueNode currentNode;
    private Dialogue currentDialogue;

    public GameObject dialogueCanvas;
    public GameObject buttonWrapper;
    public Button buttonPrefab;

    public TMP_Text npcText;

    private int selectedResponseIndex = 0;

    public bool isDialogueActive = false;

    private void Start()
    {
        outcomeManager = GameObject.Find("OutcomeManager").GetComponent<OutcomeManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (currentNode != null && buttonWrapper.transform.childCount > 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SelectNextResponse();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SelectPreviousResponse();
            }
            // FIXME dialogue selection input should be same as dialogue iniation input
            // but it triggers getkeydown same time with dialogue trigger.
            // should find a way to solve this.
            else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
            {
                SelectResponse(selectedResponseIndex);
            }
        }
    }

    private void SelectPreviousResponse()
    {
        selectedResponseIndex = Mathf.Min(selectedResponseIndex + 1, currentNode.responses.Length - 1);
        DisplayCurrentNode();
    }

    private void SelectNextResponse()
    {
        selectedResponseIndex = Mathf.Max(selectedResponseIndex - 1, 0);
        DisplayCurrentNode();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentNode = dialogue.dialogueNodes[0];
        selectedResponseIndex = 0;
        isDialogueActive = true;
        dialogueCanvas.SetActive(true);
        gameManager.PauseGame();
        DisplayCurrentNode();
    }

    public void SelectResponse(int responseIndex)
    {
        Outcome outcome = currentNode.responses[responseIndex].outcome;
        Debug.Log(outcome);
        if (!outcomeManager.worldEvents[outcome].Equals(true))
        {
            outcomeManager.SetWorldEvent(currentNode.responses[responseIndex].outcome, true);
        }

        int nextIndex = currentNode.responses[responseIndex].nextDialogueNodeIndex;
        if(nextIndex != -1)
        {
            currentNode = currentDialogue.dialogueNodes[nextIndex];
            DisplayCurrentNode();
        }
        else
        {
            Transform buttonWrapperTransform = buttonWrapper.transform;
            foreach (Transform child in buttonWrapperTransform)
            {
                Destroy(child.gameObject);
            }
            npcText.text = "";
            dialogueCanvas.SetActive(false);
            isDialogueActive = false;
            gameManager.UnpauseGame();
        }
    }

    // 1B3AD2

    public void DisplayCurrentNode()
    {
        npcText.text = currentNode.npcLine;

        Transform buttonWrapperTransform = buttonWrapper.transform;

        foreach(Transform child in buttonWrapperTransform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < currentNode.responses.Length; i++)
        {
            Button responseButton = Instantiate(buttonPrefab, buttonWrapperTransform);
            int index = i;
            responseButton.onClick.AddListener(() => SelectResponse(index));
            responseButton.GetComponentInChildren<TMP_Text>().text = currentNode.responses[index].playerLine;

            if (i == selectedResponseIndex)
            {
                responseButton.GetComponent<Image>().color = new Color(27f / 255f, 58f / 255f, 209f / 255f);
                responseButton.Select();
            }
            else
            {
                responseButton.GetComponent<Image>().color = new Color(72f / 255f, 84f / 255f, 142f / 255f);
            }
        }
    }
}
