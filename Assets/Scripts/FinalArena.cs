using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalArena : AIArea
{
    public bool justStarted;
    public bool showJustOnce;
    public static EventHandler<EventArgs> OnEndGame;
    private void Awake()
    {
        base.Awake();
        justStarted = true;
        showJustOnce = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if(isPlayerInside && justStarted && enemyList.Count>0) 
        {
            justStarted = false;
        }

        if(isPlayerInside && enemyList.Count <= 0 && !justStarted && showJustOnce)
        {
            showJustOnce=false;
            ShowEndGameUIPanel();
        }
    }

    private void ShowEndGameUIPanel()
    {
        OnEndGame?.Invoke(this, EventArgs.Empty);
    }
    public override void OnTriggerExit(Collider other)
    {
        if (!justStarted && enemyList.Count >= 0)
        {
            justStarted = true;
        }
        base.OnTriggerExit(other);
        
    }
}

