using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArm : Piece
{
    [NonSerialized] public Weapon weapon;
    
    void Start()
    {
        type = PartType.RightArm;
    }

    void Update()
    {
        
    }
}
