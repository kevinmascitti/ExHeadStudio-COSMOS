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
            //Debug.Log("Oggetto Cura: " + healthAmount.ToString());
            float actual = other.gameObject.GetComponent<PlayerCharacter>().currentHP;
            actual += healthAmount;
            //Debug.Log("Actual: " + actual.ToString());
            //Debug.Log("Actual + amount: " + (actual + healthAmount).ToString());
            other.gameObject.GetComponent<PlayerCharacter>().UpdateHP(actual);
            Destroy(this.gameObject);
        }

        gameObject.SetActive(false);//Vorrei eliminarlo, ma non mi lascia
    }
}
