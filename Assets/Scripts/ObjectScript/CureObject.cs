using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CureObject : MonoBehaviour
{

    [TextArea(2, 10)]
    public string description = "";


    [SerializeField] float healthAmount;
    [SerializeField] bool isContinous;
    [Tooltip("Dopo qunato tempo inizia l'effetto")]
    [SerializeField] float continuousTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isContinous)
        {
            float actual = other.gameObject.GetComponent<PlayerCharacter>().currentHP;
            actual += healthAmount;
            other.gameObject.GetComponent<PlayerCharacter>().UpdateHP(actual);
            Destroy(this.gameObject);
        }
        //else if(other.CompareTag("Player") && isContinous)
        //{
        //    StartCoroutine("DamageOverTime");
        //}

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && isContinous)
        {
            Debug.Log("Sono nel collider");
            StartCoroutine(DamageOverTime(other));

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }


    IEnumerator DamageOverTime(Collider playerCollider)
    {
        Debug.Log("Sono nella coraut");
        yield return new WaitForSeconds(continuousTimer);
        float actual = playerCollider.gameObject.GetComponent<PlayerCharacter>().currentHP;
        actual += healthAmount;
        playerCollider.gameObject.GetComponent<PlayerCharacter>().UpdateHP(actual);
    }
}