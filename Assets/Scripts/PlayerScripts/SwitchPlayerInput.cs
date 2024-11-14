using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlayerInput : MonoBehaviour
{
    [SerializeField] PlayerMovement movement;

    public void disableInput()
    {
        movement.enabled = false;
    }

    public void enableInput() 
    {
        movement.enabled = true;
    }

}
