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

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentNode = dialogue.dialogueNodes[0];
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
    }

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
        }
    }
}
