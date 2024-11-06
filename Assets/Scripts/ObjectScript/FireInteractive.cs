using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

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

    [SerializeField] private bool isFirstDoorPuzzle = false;

    public static EventHandler<EnemyTr> OnEnemyDestroyed; //questo evento è dichiarato in enemy, ma ho bisogno di chiamare la stessa logica

    //Dichiaro un evento per il puzzle dei bracieri
    public delegate void OnBrazierLight();
    public static event OnBrazierLight onBrazierLight;
    public bool canDestroy = false;

    private void Awake()
    {
        if (fireLight && smokeEffect && fireEffect)
        {
            fireLight.enabled = false;
            smokeEffect.Stop();
            fireEffect.Stop();
        }
        canDestroy = false;

    }

    public void InteractionsType(FireInteractions interactionType)
    {
        switch (interactionType)
        {
            case FireInteractions.Destructible:

                if (isFirstDoorPuzzle)
                {
                    DisappearingForPuzzle();
                }
                else
                {
                    Disappearing();
                }
                break;

            case FireInteractions.Lighter:
                if(isFirstDoorPuzzle)
                {
                    LighterForPuzzle();
                }
                else
                {
                    Lighter();
                }
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

    public void DisappearingForPuzzle()
    {
        Debug.Log("Ho trovato un rovo puzzle");
        //far partire un'animazione?
        if(canDestroy)
        {
            Destroy(gameObject, disappearTime);
        }

    }

    public void Lighter()
    {
        Debug.Log("Ho acceso un braciere");
        fireLight.enabled = true;
        smokeEffect.Play();
        fireEffect.Play();

    }

    public void LighterForPuzzle()
    {
        onBrazierLight.Invoke();
        fireLight.enabled = true;
        smokeEffect.Play();
        fireEffect.Play();


    }



    public void OnDestroy()
    {
        OnEnemyDestroyed?.Invoke(this, new EnemyTr(this.gameObject.transform));
    }

}
