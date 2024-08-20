using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldObj : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Vector3 direction = ((other.transform.position + other.transform.forward) - other.transform.position).normalized; //acquisisce la direzione di movimento
        Vector3 inverse = direction * -1;
        Vector3 position = other.transform.position; //crea una reference
        Debug.DrawRay(position, inverse, Color.magenta, 100f);
        Vector3 reflected = Vector3.Reflect(direction, transform.forward);
        other.transform.rotation = Quaternion.LookRotation(reflected);
        float mag = other.transform.GetComponent<Rigidbody>().velocity.magnitude;
        other.GetComponent<Rigidbody>().velocity = reflected.normalized * mag;
    }
}
