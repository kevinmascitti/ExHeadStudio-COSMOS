using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldObj : MonoBehaviour
{

    private BulletScript bulletScript;
    private BulletScriptLauncherVariant bulletScriptLauncherVariant;
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
        }
    }
}
