using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManagerFirstDoor : MonoBehaviour
{
    [Tooltip("Selezionare il numero di bracieri da attivare per il puzzle")]
    [SerializeField] private int brazierNumber = 0;
    [Tooltip("Inserire l'oggetto da attivare con il puzzle")]
    [SerializeField] private GameObject rampicantiPuzzle;

    private FireInteractive rampicantiScript;
    private int brazierCounter = 0;

    private void Start()
    {
        rampicantiScript = rampicantiPuzzle.GetComponent<FireInteractive>();
        rampicantiScript.canDestroy = false ;
    }

    // Update is called once per frame
    void Update()
    {
        if(brazierCounter == brazierNumber)
        {
            rampicantiScript.canDestroy = true;
        }
    }

    private void Count()
    {
        brazierCounter++;
        Debug.Log("Puzzle manager: " + brazierCounter);
    }

    private void OnEnable()
    {
        FireInteractive.onBrazierLight += Count;
    }

    private void OnDisable()
    {
        FireInteractive.onBrazierLight -= Count;
    }
}
