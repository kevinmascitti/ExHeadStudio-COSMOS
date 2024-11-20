using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements.Experimental;
using UnityStandardAssets.Cameras;

public class FireArmAbility : ActiveAbilities
{
    [SerializeField] Transform bulletStartPosition;
    [SerializeField] GameObject bulletPrefab;

    private CinemachineImpulseSource cameraShake; //impulso per muovere la camera allo sparo

    private int triggerID = Animator.StringToHash("isFireballShoot");

    public override void Ability()
    {
        playerAnimator.SetBool(triggerID, true);       
    }

    public void ShootBullet() //la funzione viene chiamata da un animation event, che accede a PlayerStateController
    {
        Instantiate(bulletPrefab, bulletStartPosition.position, bulletStartPosition.rotation);
        playerAnimator.SetBool(triggerID, false);
        if ((cameraShake = GetComponent<CinemachineImpulseSource>()) != null )
        {
            cameraShake.GenerateImpulse();
        }
    }
}


































//VERSIONE PRECEDENTE
//using Cinemachine;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class FireArmAbility : LeftArm
//{

//    [SerializeField] float cooldownTime;
//    [SerializeField] Transform bulletStartPosition;
//    [SerializeField] GameObject bulletPrefab;
//    [SerializeField] Image frontAbilityImage;
//    [SerializeField] TMPro.TextMeshProUGUI abilityText;

//    private float abilityTimer;
//    private float abilityFraction;
//    [SerializeField]
//    CinemachineImpulseSource impulseSource;

//    private bool cooldown = false;

//    private void Awake()
//    {
//        frontAbilityImage.fillAmount = 1;
//    }
//    public void Start()
//    {
//        base.Start();
//        //abilityTimer = 0; //cooldownTime;
//        Mathf.Clamp(abilityTimer, 0, cooldownTime);
//        impulseSource = GetComponent<CinemachineImpulseSource>(); //� importante che la camera abbia il tag MainCamera

//    }

//    public void Update()
//    {
//        base.Update();
//        abilityTimer += Time.deltaTime;
//        UpdateAbiltyColumn();
//    }

//    public override void LeftArmAbility()
//    {
//        if(!cooldown)
//        {
//            base.LeftArmAbility();
//            cooldown = true;
//            abilityTimer = 0;
//            //impulseSource = GetComponent<CinemachineImpulseSource>();

//            ShootBullet();
//            impulseSource.GenerateImpulse(Camera.main.transform.forward);
//            StartCoroutine("AbilityCooldown");
//        }

//    }

//    public void ShootBullet()
//    {
//        //Debug.Log("Ho sparato");
//        Instantiate(bulletPrefab, bulletStartPosition.position, bulletStartPosition.rotation);
//        //cameraShake.GenerateImpulse();
//    }

//    private IEnumerator AbilityCooldown()
//    {
//        yield return new WaitForSeconds(cooldownTime);
//        cooldown = false;
//        //Debug.Log("Ho aspettato");
//    }

//    public void UpdateAbiltyColumn()
//    {
//        if(abilityTimer < cooldownTime)
//        {
//            abilityFraction = abilityTimer / cooldownTime;
//            abilityText.color = new Color(abilityText.color.r, abilityText.color.g, abilityText.color.b, 0.5f);
//        }
//        else
//        {
//            abilityText.color = new Color(abilityText.color.r, abilityText.color.g, abilityText.color.b, 1);
//            abilityFraction = 1;
//        }
//        //if(abilityFraction >= 1)
//        //{
//        //    abilityFraction = 1;
//        //}
//        //else
//        //{
//        //     abilityFraction = abilityTimer / cooldownTime;
//        //}
//        //Debug.Log("timer: " + abilityTimer) ;
//        //Debug.Log("frazione:" + abilityFraction);
//        frontAbilityImage.fillAmount = abilityFraction;

//    }
//}
