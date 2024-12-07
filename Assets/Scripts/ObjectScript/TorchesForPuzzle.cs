using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchesForPuzzle : MonoBehaviour
{

    //A CHI RINOMINA LE COSE TAGLIO LE MANI
    [SerializeField] GameObject torchOrb;
    [SerializeField] int lightIndex;

    private PuzzleManagerFirstDoor puzzleManager;
    [SerializeField] GameObject brazierOrb;

    private bool alreadyLit;
    private void Awake()
    {
        torchOrb.SetActive(false);
        alreadyLit = false;
        puzzleManager = GameObject.Find("PuzzleManagerFirstDoor").GetComponent<PuzzleManagerFirstDoor>();
    }

    private void Update(){
        if(brazierOrb.GetComponent<ParticleSystem>().isPlaying){
              torchOrb.SetActive(true);
        }
    }

    /*
    private void switchLight()
    {
        if(!alreadyLit && puzzleManager.brazierCounter+1 == lightIndex) //questo pi� uno da vedere in base a come legge gli eventi 
        {
            torchOrb.SetActive(true);
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
    */
}
