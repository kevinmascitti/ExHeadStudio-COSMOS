using Cinemachine;
using PilotoStudio;
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

    [SerializeField] private BeamEmitter waterEffectScript;
    [SerializeField] private GameObject waterEffectObj;
    [SerializeField] private Transform beamBaseTarget;
    public override void Start()
    {
        base.Start();
        lockOnScript = GameObject.Find("Player").GetComponent<LockOnCamSwitcher>();
    }

    public override void Ability()
    {
        if(!playerAnimator.GetBool("isJumpAscension")){
            waterEffectObj.SetActive(true);
            playerAnimator.Play("Cyrus_Cosmos_Rig_Cyrus_WaterJet"); 
            if(lockOnScript.lockOn)
            {
                waterEffectScript.SetBeamTarget(lockOnScript.GetCurrentEnemyTr());
                ray = new Ray(this.startPosition.position, (lockOnScript.GetCurrentEnemyTr().position - this.startPosition.position).normalized * maxRange);
            }
            else
            {
                waterEffectScript.SetBeamTarget(beamBaseTarget);
                ray = new Ray(this.startPosition.position, GameObject.Find("Player").transform.forward * maxRange);
            }

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxRange, mask))
            {
                if(hit.rigidbody != null && !lockOnScript.lockOn)
                {
                    hit.rigidbody.AddForce((beamBaseTarget.position-transform.position) * forceMagnitude, ForceMode.Impulse);
                }
                if (hit.rigidbody != null && lockOnScript.lockOn)
                {
                    hit.rigidbody.AddForce((lockOnScript.GetCurrentEnemyTr().position - transform.position) * forceMagnitude, ForceMode.Impulse);
                }

            }
        }
       
    }


    public override void SetFalseObj()
    {
        waterEffectObj.SetActive(false);
        playerAnimator.SetBool("isWaterJetOn", false);
    }

}
