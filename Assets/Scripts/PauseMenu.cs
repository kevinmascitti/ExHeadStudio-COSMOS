using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //Lo script è assegnato ad una empty che gestisce tutta la HUD del giocatore. Il menù di pausa è un cavas che viene acceso/spento 
    //premendo escape; i comandi sono disabilitati quando il menù è attivo.
    [SerializeField] Canvas pauseMenu;
    [SerializeField] Canvas[] UI_elements;
    [SerializeField] GameObject player;

    private void Awake()
    {
        pauseMenu.enabled = false;

    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)) 
        { 
            PauseGame();
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void PauseGame()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        pauseMenu.enabled = true;
        foreach(var item in UI_elements) 
        { 
            item.enabled = false;
            Time.timeScale = 0f; //è una versione rudimentale per la pausa, va modificata per gli eventi che non avvengono in update
        }
    }

    public void BackToGame()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        pauseMenu.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        foreach (var item in UI_elements)
        {
            item.enabled = true;
            Time.timeScale = 1;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
