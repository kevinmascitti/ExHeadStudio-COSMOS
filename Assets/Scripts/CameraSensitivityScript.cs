using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSensitivityScript : MonoBehaviour
{
    [SerializeField]
    InputAction action;

    [SerializeField]
    float x = 3f;

    private void Awake()
    {
        ChenageValue(x);
    }
    void ChenageValue(float x)
    {
        action.ApplyParameterOverride("scaleVector2:x", x);
    }

}
