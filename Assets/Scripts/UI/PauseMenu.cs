using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //Lo script � assegnato ad una empty che gestisce tutta la HUD del giocatore. Il men� di pausa � un cavas che viene acceso/spento 
    //premendo escape; i comandi sono disabilitati quando il men� � attivo.
    [SerializeField] Canvas pauseMenu;
    [SerializeField] Canvas deathScreen;
    [SerializeField] Canvas winScreen;
    [SerializeField] Canvas settingsScreen;
    [SerializeField] Canvas hints;
    [SerializeField] Canvas[] UI_elements;
    [SerializeField] GameObject player;
    [NonSerialized] public bool isUIOpen = false;
    [SerializeField] CinemachineFreeLook playerCamera;
    [SerializeField] CinemachineFreeLook lockOnCamera;

    private bool yInverted;

    private void Awake()
    {
        pauseMenu.enabled = false;
        deathScreen.enabled = false;
        winScreen.enabled = false;
        settingsScreen.enabled = false;
        hints.enabled = false;
        yInverted = false;
        playerCamera.m_YAxis.m_InvertInput = false;
        lockOnCamera.m_YAxis.m_InvertInput = false;

        PlayerCharacter.OnPlayerDeath += DeathScreen;
        FinalArena.OnEndGame += WinScreen;
    }

    private void OnDestroy()
    {
        PlayerCharacter.OnPlayerDeath -= DeathScreen;
        FinalArena.OnEndGame -= WinScreen;
    }


    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)) 
        { 
            PauseGame();
        }
    }

    public void DeathScreen(object sender, EventArgs e)
    {
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.GetComponent<PlayerMovement>().enabled = false;
        deathScreen.enabled = true;
        foreach (var item in UI_elements)
        {
            item.enabled = false;
            Time.timeScale = 0f; //� una versione rudimentale per la pausa, va modificata per gli eventi che non avvengono in update
        }

        PlayMorte();
    }

    private FMOD.Studio.EventInstance deathSound;
    public void WinScreen(object sender, EventArgs e)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.GetComponent<PlayerMovement>().enabled = false;
        winScreen.enabled = true;
        foreach (var item in UI_elements)
        {
            item.enabled = false;
            Time.timeScale = 0f; //� una versione rudimentale per la pausa, va modificata per gli eventi che non avvengono in update
        }
        PlayWin();
    }

    public void SettingsScreen()
    {
        pauseMenu.enabled = false;
        settingsScreen.enabled = true;
    }
    public void BAckToPauseMenu()
    {
        settingsScreen.enabled = false;
        pauseMenu.enabled = true;

    }


    public void PlayWin()
    {
        //Da introdurre quello che deve succedere in caso di vittoria.
    }
    public void PlayMorte()
    {
        deathSound = FMODUnity.RuntimeManager.CreateInstance("event:/Morte");
        deathSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        deathSound.start();
        deathSound.release();
    }
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isUIOpen = true;
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
        winScreen.enabled = false;
        isUIOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach (var item in UI_elements)
        {
            item.enabled = true;
            Time.timeScale = 1f;
        }
    }

    public void InvertiComandiCamera()
    {
        Debug.Log("Non salva i cambiamenti");
       if(!yInverted)
        {
            playerCamera.m_YAxis.m_InvertInput = true;
            lockOnCamera.m_YAxis.m_InvertInput = true;
            yInverted = true;
        }

        else
        {
            playerCamera.m_YAxis.m_InvertInput = false;
            lockOnCamera.m_YAxis.m_InvertInput = false;
            yInverted = false;
        }

    }

    public void ChangeCamSensitivityX(System.Single xValue)
    {
        playerCamera.m_XAxis.m_MaxSpeed = xValue;
    }
    public void ChangeCamSensitivityY(System.Single yValue)
    {
        playerCamera.m_YAxis.m_MaxSpeed = yValue;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
