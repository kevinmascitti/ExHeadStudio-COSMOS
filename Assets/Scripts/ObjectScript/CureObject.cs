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
    [SerializeField] GameObject HealingVFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isContinous)
        {
            GameObject healParticles = Instantiate(HealingVFX) as GameObject;
            healParticles.transform.position = other.transform.position;
            healParticles.transform.parent = other.transform;
            healParticles.GetComponent<ParticleSystem>().Play();
            float actual = other.gameObject.GetComponent<PlayerCharacter>().currentHP;
            actual += healthAmount;
            other.gameObject.GetComponent<PlayerCharacter>().UpdateHP(actual);
            Destroy(this.gameObject);
            StartCoroutine(DestroyParticle(healParticles));
        }
        //else if(other.CompareTag("Player") && isContinous)
        //{
        //    StartCoroutine("DamageOverTime");
        //}

    }

    IEnumerator DestroyParticle(GameObject particles)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(particles);
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