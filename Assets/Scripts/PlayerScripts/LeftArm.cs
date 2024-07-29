using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArm : MonoBehaviour
{
    private PartType type;
    
    public void Start()
    {
        type = PartType.LeftArm;
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            LeftArmAbility();
        }
    }

    //funzione che regola le abilit� specifiche di un braccio
    public virtual void LeftArmAbility()
    {

        //qui bisogna inserire animazioni, suoni, effetti...
    }




}

