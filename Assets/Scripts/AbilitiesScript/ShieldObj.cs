using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ShieldObj : MonoBehaviour
{

    private CinemachineImpulseSource cameraShake;

    private BulletScript bulletScript;
    private BulletScriptLauncherVariant bulletScriptLauncherVariant;
    private EnemyWeapon enemyWeapon;
    private Enemy enemyScript;
    private Rigidbody enemyRb;

    [SerializeField] private float forceMag = 30f;
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<BulletScript>(out bulletScript))
        {
            Debug.Log("Test collisione");
            Vector3 direction = (other.transform.forward).normalized; //acquisisce la direzione di movimento
            Vector3 inverse = direction * -1;
            Vector3 position = other.transform.position; //crea una reference
            Vector3 reflected = Vector3.Reflect(direction, transform.forward);
            other.transform.rotation = Quaternion.LookRotation(reflected);
            float mag = other.transform.GetComponent<Rigidbody>().velocity.magnitude;
            other.GetComponent<Rigidbody>().velocity = reflected.normalized * mag;

            if ((cameraShake = GetComponent<CinemachineImpulseSource>()) != null)
            {
                cameraShake.GenerateImpulse();
            }
        }
        else if(other.TryGetComponent<BulletScriptLauncherVariant>(out bulletScriptLauncherVariant))
        {
            Debug.Log("Test collisione launcher");
            Vector3 direction = (other.transform.forward).normalized; //acquisisce la direzione di movimento
            Vector3 inverse = direction * -1;
            Vector3 position = other.transform.position; //crea una reference
            Vector3 reflected = Vector3.Reflect(direction, transform.forward);
            other.transform.rotation = Quaternion.LookRotation(reflected);
            float mag = other.transform.GetComponent<Rigidbody>().velocity.magnitude;
            bulletScriptLauncherVariant.SetVelocity(reflected.normalized * mag);

            if ((cameraShake = GetComponent<CinemachineImpulseSource>()) != null)
            {
                cameraShake.GenerateImpulse();
            }
        }
        else if(other.TryGetComponent<EnemyWeapon>(out enemyWeapon))
        {
            enemyScript = other.GetComponentInParent<Enemy>();
            enemyRb = enemyScript.gameObject.GetComponent<Rigidbody>();

            enemyRb.AddForce((enemyScript.gameObject.transform.position - transform.position).normalized*forceMag, ForceMode.Impulse);

            if ((cameraShake = GetComponent<CinemachineImpulseSource>()) != null)
            {
                cameraShake.GenerateImpulse();
            }

        }
    }
}
