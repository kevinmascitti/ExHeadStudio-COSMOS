using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireInteractions
{
    Destructible,
    Lighter
}
public class FireInteractive : MonoBehaviour
{
    [SerializeField] float disappearTime;
    [SerializeField] Light fireLight;
    [SerializeField] ParticleSystem smokeEffect;
    [SerializeField] ParticleSystem fireEffect;
    [SerializeField] public FireInteractions typeOfObjectInteraction;

    private void Awake()
    {
        if (fireLight && smokeEffect && fireEffect)
        {
            fireLight.enabled = false;
            smokeEffect.Stop();
            fireEffect.Stop();
        }
            
    }

    public void InteractionsType(FireInteractions interactionType)
    {
        switch (interactionType)
        {
            case FireInteractions.Destructible:
                Disappearing();
                break;

            case FireInteractions.Lighter:
                Lighter(); 
                break;

            default: break;
        }
    }

    public void Disappearing()
    {
        Debug.Log("Ho trovato un rovo");
        //far partire un'animazione?
        Destroy(gameObject, disappearTime);
    }

    public void Lighter()
    {
        fireLight.enabled = true;
        smokeEffect.Play();
        fireEffect.Play();

    }
}
