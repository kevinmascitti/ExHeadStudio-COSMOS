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
    [SerializeField] public FireInteractions typeOfObjectInteraction;

    private void Awake()
    {

        fireLight.enabled = false;
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
    }
}
