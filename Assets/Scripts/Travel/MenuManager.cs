using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private int currentSelectedIndex = 0;

    private bool isMenuActive = false;

    private GameManager gameManager;

    public GameObject pauseMenu;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if(!isMenuActive && Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActive = true;
            gameManager.PauseGame();
            pauseMenu.SetActive(true);

        } else if(isMenuActive && Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActive = false;
            gameManager.UnpauseGame();
            pauseMenu.SetActive(false);
        }
    }
}
