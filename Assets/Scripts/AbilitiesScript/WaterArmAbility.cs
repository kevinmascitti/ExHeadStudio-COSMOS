using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterArmAbility : ActiveAbilities
{
    [Tooltip("Bisogna controllare che la empty assegnata ruoti correttamente, come children del modello")]
    [SerializeField] private Transform startPosition;
    [SerializeField] private float maxRange = 1f, forceMagnitude = 1f;
    [SerializeField] private LayerMask mask;

    private LockOnCamSwitcher lockOnScript;
    private Ray ray;
    private CinemachineImpulseSource cameraShake;
    public override void Start()
    {
        base.Start();
        lockOnScript = GameObject.Find("Player").GetComponent<LockOnCamSwitcher>();

    }

    public override void Ability()
    {
        Debug.DrawRay(startPosition.position, Camera.main.transform.forward * maxRange, Color.white, 0.5f);

        if(lockOnScript.lockOn)
        {
             ray = new Ray(this.startPosition.position, (lockOnScript.GetCurrentEnemyTr().position - this.startPosition.position).normalized * maxRange);
        }
        else
        {
             ray = new Ray(this.startPosition.position, GameObject.Find("Player").transform.forward * maxRange);
        }

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRange, mask))
        {
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.point * forceMagnitude, ForceMode.Impulse);
            }
    
        }
        if ((cameraShake = GetComponent<CinemachineImpulseSource>()) != null)
        {
            cameraShake.GenerateImpulse();
        }
    }

}
