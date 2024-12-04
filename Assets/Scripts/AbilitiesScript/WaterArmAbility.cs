    using Cinemachine;
using PilotoStudio;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class WaterArmAbility : ActiveAbilities
{
    [Tooltip("Bisogna controllare che la empty assegnata ruoti correttamente, come children del modello")]
    [SerializeField] private Transform startPosition;
    [SerializeField] private float maxRange = 1f, forceMagnitude = 1f;
    [SerializeField] private LayerMask mask;

    private LockOnCamSwitcher lockOnScript;
    private Ray ray;

    private Transform enemyTransform;

    private GameObject player;

    [SerializeField] private BeamEmitter waterEffectScript;
    [SerializeField] private GameObject waterEffectObj;
    [SerializeField] private Transform beamBaseTarget;
    public override void Start()
    {
        base.Start();

        player = GameObject.Find("Player");
        lockOnScript = player.GetComponent<LockOnCamSwitcher>();
    }

    public override void Ability()
    {
        if (waterEffectObj.activeSelf)
        {
            return;
        }
        if (!playerAnimator.GetBool("isJumpAscension")){
            waterEffectObj.SetActive(true);
            playerAnimator.Play("Cyrus_Cosmos_Rig_Cyrus_WaterJet");
            PlayWaterJet();

            if(lockOnScript.lockOn)
            {
                enemyTransform = lockOnScript.GetCurrentEnemyTr();
                enemyTransform.position += new Vector3(0f, 3f, 0f);
                waterEffectScript.SetBeamTarget(enemyTransform); //ho aggiunto un offset arbitratio per non sparare sui piedi
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
                    player.transform.rotation = Quaternion.LookRotation((lockOnScript.GetCurrentEnemyTr().position - transform.position), Vector3.up);
                    hit.rigidbody.AddForce((lockOnScript.GetCurrentEnemyTr().position - transform.position) * forceMagnitude, ForceMode.Acceleration);
                }

            }
        }
       
    }


    public override void SetFalseObj()
    {
        waterEffectObj.SetActive(false);
        playerAnimator.SetBool("isWaterJetOn", false);
        waterJet.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private FMOD.Studio.EventInstance waterJet;

    private void PlayWaterJet()
    {
        waterJet = FMODUnity.RuntimeManager.CreateInstance("event:/Water Jet");
        waterJet.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        waterJet.start();
        waterJet.release();
    }
}
