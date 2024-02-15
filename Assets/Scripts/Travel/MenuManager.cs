using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private int currentHoverIndex = 0;
    private int selectedIndex = -1;

    private bool isMenuActive = false;

    private GameManager gameManager;
    private DialogueManager dialogueManager;

    public GameObject pauseMenu;
    public GameObject pauseMenuButtonWrapper;
    public GameObject[] buttons = new GameObject[3];
    public GameObject[] menuContents = new GameObject[3];

    private Color selectionColor = new Color(67f/255f, 53f / 255f, 159f / 255f, 160f / 255f);
    private Color transparent = new Color(255f, 255f, 255f, 0f);

    private InventoryMenuManager inventoryMenuManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        inventoryMenuManager= gameObject.GetComponent<InventoryMenuManager>();
    }

    private void Update()
    {
        if(isMenuActive)
        {
            SetActiveMenu();
        }

        if (!isMenuActive && !dialogueManager.isDialogueActive && Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActive = true;
            gameManager.PauseGame();
            pauseMenu.SetActive(true);
            pauseMenuButtonWrapper.SetActive(true);
            currentHoverIndex = 0;
            buttons[currentHoverIndex].GetComponent<Image>().color = selectionColor;

        } else if(isMenuActive && selectedIndex == -1 && Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActive = false;
            gameManager.UnpauseGame();
            pauseMenu.SetActive(false);
            pauseMenuButtonWrapper.SetActive(false);
            currentHoverIndex = 0;
            selectedIndex = -1;
        } else if (isMenuActive && selectedIndex == -1)
        {
            NavigateOnMainMenu();
        } else if (isMenuActive && selectedIndex != -1 && Input.GetKeyDown(KeyCode.Escape))
        {
            selectedIndex = -1;
            inventoryMenuManager.isActive = false;
        }
    }

    private void SetActiveMenu()
    {
        for(int i =0; i < menuContents.Length; i++)
        {
            if(i == currentHoverIndex)
            {
                menuContents[i].SetActive(true);
            }
            else
            {
                menuContents[i].SetActive(false);
            }
        }
    }

    private void SelectMenu()
    {
        switch (currentHoverIndex)
        {
            case 0:
                inventoryMenuManager.isActive = true;
                break;
            case 1:
                break;
            case 2:
                break;
        }

        selectedIndex = currentHoverIndex;
    }

    private void NavigateOnMainMenu()
    {
        Debug.Log("currentHoverIndex " + currentHoverIndex);
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            buttons[currentHoverIndex].GetComponent<Image>().color = transparent;
            if (currentHoverIndex +1 == buttons.Length)
            {
                buttons[0].GetComponent<Image>().color = selectionColor;
                currentHoverIndex = 0;
            } else
            {
                buttons[++currentHoverIndex].GetComponent<Image>().color = selectionColor;
            }
        } 
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            buttons[currentHoverIndex].GetComponent<Image>().color = transparent;

            if (currentHoverIndex -1  == -1)
            {
                buttons[buttons.Length - 1].GetComponent<Image>().color = selectionColor;
                currentHoverIndex = buttons.Length - 1;
            }
            else
            {
                buttons[--currentHoverIndex].GetComponent<Image>().color = selectionColor;
            }
        } 
        else if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            SelectMenu();
        }
    }
}
