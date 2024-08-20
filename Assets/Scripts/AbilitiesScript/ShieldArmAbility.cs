using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldArmAbility : ActiveAbilities
{
    //[SerializeField] private Transform shieldPosition;
    [SerializeField] private GameObject shield;
    //[SerializeField] private float shieldDurationTime;

    private void Awake()
    {
        shield.SetActive(false);
    }

    public override void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("Preso");
            Ability();
        }
        else
        {
            shield.SetActive(false);
        }
    }
    public override void Ability()
    {
        shield.SetActive(true);
    }
}
