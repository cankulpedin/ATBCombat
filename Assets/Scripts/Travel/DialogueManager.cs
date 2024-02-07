using NUnit.Framework;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    GameManager gameManager;

    private Dialogue.DialogueNode currentNode;
    private Dialogue currentDialogue;

    public GameObject dialogueCanvas;
    public GameObject buttonWrapper;
    public Button buttonPrefab;

    public TMP_Text npcText;

    [SerializeField]
    private List<IDialogueObserver> observers = new List<IDialogueObserver>();

    private int selectedResponseIndex = 0;

    private void Awake()
    {
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
        dialogueCanvas.SetActive(true);
        NotifyDialogueStarted();
        DisplayCurrentNode();
    }

    public void RegisterObserver(IDialogueObserver observer)
    {
        observers.Add(observer);
    }

    public void UnregisterObserver(IDialogueObserver observer)
    {
        observers.Remove(observer);
    }

    private void NotifyDialogueStarted()
    {
        foreach (var observer in observers)
        {
            observer.NotifyDialogueStarted();
        }
    }

    private void NotifyDialogueEnded()
    {
        foreach (var observer in observers)
        {
            observer.NotifyDialogueEnded();
        }
    }

    public void SelectResponse(int responseIndex)
    {
        gameManager.SetWorldEvent(currentNode.responses[responseIndex].outcome, true);

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

            NotifyDialogueEnded();
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
                responseButton.GetComponent<Image>().color = new Color(27f, 58f, 209f);
                responseButton.Select();
            }
            else
            {
                responseButton.GetComponent<Image>().color = new Color(72f, 84f, 142f);
            }
        }
    }
}
