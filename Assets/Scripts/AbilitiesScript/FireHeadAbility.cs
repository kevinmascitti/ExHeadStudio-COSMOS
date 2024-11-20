using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FireHeadAbility : MonoBehaviour
{
    [Tooltip("Il raggio entro il quale la luce trova gli oggetti")]
    [SerializeField] private float lightRange;
    [SerializeField] private LayerMask objectMask;
    [SerializeField] private Light lightSource;
    private Collider[] revealingObjects;
    private int maxObjects = 10;
    private int objectNumber;
    private Renderer objRenderer;

    private void Awake()
    {
        revealingObjects = new Collider[maxObjects];
    }

    private void Update()
    {
        lightSource.range = lightRange;

        objectNumber = Physics.OverlapSphereNonAlloc(transform.position, lightRange, revealingObjects, objectMask);

        if(objectNumber != 0) 
        { 
            for(int i = 0; i < revealingObjects.Length && revealingObjects[i] != null; i++)
            {
                objRenderer = revealingObjects[i].GetComponent<Renderer>();
                Debug.Log("Ho trovato: " + revealingObjects[i].name);
                Color objColor = objRenderer.material.color;
                Debug.Log("Colore: " + revealingObjects[i].GetComponent<Renderer>().material.color);
                objRenderer.material.color = new Color(objRenderer.material.color.r, objRenderer.material.color.g, objRenderer.material.color.b,
                    Mathf.Lerp(objRenderer.material.color.g, 1f, 1)); //PROVARE CON UNA COROUTINE per rendere graduale la transizione
                Debug.Log("Colore 2: " + revealingObjects[i].GetComponent<Renderer>().material.color);
            }
        }
    }

    //Serve a visualizzare il volume della luce
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, lightRange);  
    //}



}
