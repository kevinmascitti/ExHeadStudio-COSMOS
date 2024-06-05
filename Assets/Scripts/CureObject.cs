using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureObject : MonoBehaviour
{
    [SerializeField] float healthAmount;
   // [SerializeField] bool isContinous;
    [SerializeField] float continuousTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))     
        {
            //if(isContinous)
            //{
            //    other.gameObject.GetComponent<PlayerCharacter>().UpdateHP(healthAmount);

            //}

                //Debug.Log("Oggetto Cura: " + healthAmount.ToString());
                float actual = other.gameObject.GetComponent<PlayerCharacter>().currentHP;
                actual += healthAmount;
                //Debug.Log("Actual: " + actual.ToString());
                //Debug.Log("Actual + amount: " + (actual + healthAmount).ToString());
                other.gameObject.GetComponent<PlayerCharacter>().UpdateHP(actual);

           

            Destroy(this.gameObject);
        }

    }
}
