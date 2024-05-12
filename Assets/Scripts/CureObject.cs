using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureObject : MonoBehaviour
{
    [SerializeField] float healthAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))     
        {
            float actual = other.gameObject.GetComponent<PlayerCharacter>().currentHP;
            Debug.Log("Oggetto Cura: " + healthAmount);
            other.gameObject.GetComponent<PlayerCharacter>().UpdateHP(actual + healthAmount);
        }

        gameObject.SetActive(false);//Vorrei eliminarlo, ma non mi lascia
    }
}
