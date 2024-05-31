using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArm : Piece
{

    void Start()
    {
        type = PartType.LeftArm;
    }
    public virtual void Update()
    {
        if (Input.GetKey(KeyCode.V))//da sostituire con il tasto del mouse
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

