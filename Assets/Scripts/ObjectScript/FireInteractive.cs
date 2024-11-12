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
    private bool alreadyLight = false;

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
        //far partire un'animazione?
        OnEnemyDestroyed?.Invoke(this, new EnemyTr(this.gameObject.transform));
        Destroy(gameObject, disappearTime);
    }

    public void DisappearingForPuzzle()
    { 
        
        if(canDestroy)
        {
            OnEnemyDestroyed?.Invoke(this, new EnemyTr(this.gameObject.transform));
            Destroy(gameObject, disappearTime);
        }

    }

    public void Lighter()
    {
        Debug.Log("Ho acceso un braciere");
        OnEnemyDestroyed?.Invoke(this, new EnemyTr(this.gameObject.transform));
        fireLight.enabled = true;
        smokeEffect.Play();
        fireEffect.Play();

    }

    public void LighterForPuzzle()
    {
        if(!alreadyLight)
        {

            onBrazierLight.Invoke();
            OnEnemyDestroyed?.Invoke(this, new EnemyTr(this.gameObject.transform));
            fireLight.enabled = true;
            smokeEffect.Play();
            fireEffect.Play();
            gameObject.GetComponent<Collider>().enabled = false;
            alreadyLight = true;
        }

    }



    public void OnDestroy()
    {
        OnEnemyDestroyed?.Invoke(this, new EnemyTr(this.gameObject.transform));
    }

}
