using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PartType
{
    Head,
    Body,
    RightArm,
    LeftArm,
    Legs
}

public class LULPlayerCharacter : LULCharacter
{
    public float MAX_HP = 100;
    public float def_HP = 100;
    public Slider sliderHP;

    public Dictionary<PartType, LULPiece> composition;

    [NonSerialized] public LULScenario currentScenario;
    [NonSerialized] public LULScenario defaultScenario;

    public static EventHandler OnPlayerDeath;
    public static EventHandler<ScenarioArgs> OnScenarioBegin;

    // Start is called before the first frame update
    public void Awake()
    {
        isPlayer = true;

        if (sliderHP)
        {
            sliderHP.maxValue = MAX_HP;
        }
    
        UpdateHP(def_HP);
    }

    public override void UpdateHP(float newHP)
    {
        base.UpdateHP(newHP);
        UpdateHPUI(currentHP);
    }
    
    public void UpdateHPUI(float HP)
    {
        if(sliderHP)
            sliderHP.value = HP;
    }

    public override void Die()
    {
        base.Die();
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        Debug.Log("DIED");
        Respawn();
    }

    public void Respawn()
    {
        UpdateHP(MAX_HP);
        currentScenario = defaultScenario;
        gameObject.transform.position = currentScenario.respawnPoint;
        Debug.Log("RESPAWNED");
    }

}

public class ScenarioArgs : EventArgs
{
    public ScenarioArgs(LULScenario a)
    {
        scenario = a;
    }
    public LULScenario scenario;
}