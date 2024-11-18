using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchesForPuzzle : MonoBehaviour
{

    //A CHI RINOMINA LE COSE TAGLIO LE MANI
    [SerializeField] Light lightToSwitch;
    [SerializeField] int lightIndex;

    private PuzzleManagerFirstDoor puzzleManager;

    private bool alreadyLit;
    private void Awake()
    {
        lightToSwitch.enabled = false;
        alreadyLit = false;
        puzzleManager = GameObject.Find("PuzzleManagerFirstDoor").GetComponent<PuzzleManagerFirstDoor>();
    }

    private void switchLight()
    {
        if(!alreadyLit && puzzleManager.brazierCounter+1 == lightIndex) //questo più uno da vedere in base a come legge gli eventi 
        {
            lightToSwitch.enabled = true;
            alreadyLit = true;
        }
    }

    private void OnEnable()
    {
        FireInteractive.onBrazierLight += switchLight;
    }
    private void OnDisable()
    {
        FireInteractive.onBrazierLight -= switchLight;
    }
}
