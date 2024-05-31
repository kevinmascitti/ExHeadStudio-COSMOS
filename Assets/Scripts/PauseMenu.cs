using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //Lo script � assegnato ad una empty che gestisce tutta la HUD del giocatore. Il men� di pausa � un cavas che viene acceso/spento 
    //premendo escape; i comandi sono disabilitati quando il men� � attivo.
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
        }
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.GetComponent<PlayerMovement>().enabled = false;
        pauseMenu.enabled = true;
        foreach(var item in UI_elements) 
        { 
            item.enabled = false;
            Time.timeScale = 0f; //� una versione rudimentale per la pausa, va modificata per gli eventi che non avvengono in update
        }
    }

    public void BackToGame()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        pauseMenu.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach (var item in UI_elements)
        {
            item.enabled = true;
            Time.timeScale = 1;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
