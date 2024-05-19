using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //Lo script è assegnato ad una empty che gestisce tutta la HUD del giocatore. Il menù di pausa è un cavas che viene acceso/spento 
    //premendo escape; i comandi sono disabilitati quando il menù è attivo.
    [SerializeField] Canvas pauseMenu;
    [SerializeField] Canvas[] UI_elemets;
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
        foreach(var item in UI_elemets) 
        { 
            item.enabled = false;
        }
    }

    public void BackToGame()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        pauseMenu.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        foreach (var item in UI_elemets)
        {
            item.enabled = true;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
