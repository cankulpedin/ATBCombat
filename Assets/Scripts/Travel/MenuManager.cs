using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private int currentSelectedIndex = 0;

    private bool isMenuActive = false;

    private GameManager gameManager;
    private DialogueManager dialogueManager;

    public GameObject pauseMenu;
    public GameObject[] buttons = new GameObject[3];

    private Color selectionColor = new Color(67f/255f, 53f / 255f, 159f / 255f, 160f / 255f);
    private Color transparent = new Color(255f, 255f, 255f, 0f);


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
    }

    private void Update()
    {
        Debug.Log(currentSelectedIndex);
        if(!isMenuActive && !dialogueManager.isDialogueActive && Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActive = true;
            gameManager.PauseGame();
            pauseMenu.SetActive(true);
            currentSelectedIndex = 0;
            buttons[currentSelectedIndex].GetComponent<Image>().color = selectionColor;

        } else if(isMenuActive && Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActive = false;
            gameManager.UnpauseGame();
            pauseMenu.SetActive(false);
            currentSelectedIndex = 0;
        } 

        if(isMenuActive)
        {
            NavigateOnMainMenu();
        }
    }

    private void NavigateOnMainMenu()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            buttons[currentSelectedIndex].GetComponent<Image>().color = transparent;
            if (currentSelectedIndex +1 == buttons.Length)
            {
                buttons[0].GetComponent<Image>().color = selectionColor;
                currentSelectedIndex = 0;
            } else
            {
                buttons[++currentSelectedIndex].GetComponent<Image>().color = selectionColor;
            }
        } else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            buttons[currentSelectedIndex].GetComponent<Image>().color = transparent;

            if (currentSelectedIndex -1  == -1)
            {
                buttons[buttons.Length - 1].GetComponent<Image>().color = selectionColor;
                currentSelectedIndex = buttons.Length - 1;
            }
            else
            {
                buttons[--currentSelectedIndex].GetComponent<Image>().color = selectionColor;
            }
        }
    }
}
