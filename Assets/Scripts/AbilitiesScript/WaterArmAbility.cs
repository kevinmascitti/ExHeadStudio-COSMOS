using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterArmAbility : ActiveAbilities
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private float maxRange, forceMagnitude;
    [SerializeField] private LayerMask mask;
    private GameObject objectHit;


    public override void Update()
    {
        //test
        if(Input.GetKey(KeyCode.Q))
        {
            Ability();
        }

    }

    public override void Ability()
    {
        Debug.DrawRay(startPosition.position, Vector3.forward, Color.white);
        var ray = new Ray(this.startPosition.position,Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRange, mask))
        {
            Debug.Log("Preso oggetto");
            Debug.DrawRay(startPosition.position, Vector3.forward, Color.red);
            if(hit.rigidbody)
            {
                hit.rigidbody.AddForce(hit.transform.position, ForceMode.Force);
            }
    
        }
    }

}
