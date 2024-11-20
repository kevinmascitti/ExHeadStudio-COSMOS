using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSwapper : MonoBehaviour
{
    // versione base, pu�, funzioanare se � da usare poche volte.


    [Tooltip("Ho inserito un pezzo preso direttamente dalla empty del player")]
    [SerializeField] private GameObject pieceToSet;

    private Piece[] pieceArray;

    private Piece pieceToSetScript;
    
    [SerializeField] private GameObject VFX;

    private void Awake()
    {
        pieceToSetScript = pieceToSet.GetComponent<Piece>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pieceArray = other.GetComponentsInChildren<Piece>();

            for(int i  = 0; i < pieceArray.Length; i++)
            {
                if(pieceArray[i].type == pieceToSetScript.type) 
                {
                    pieceArray[i].gameObject.SetActive(false);
                    break;
                }
            }

            pieceToSet.SetActive(true);

            gameObject.SetActive(false);
            VFX.SetActive(true);
            VFX.GetComponent<ParticleSystem>().Play();
            VFX.transform.position = other.transform.position;
            VFX.transform.parent = other.transform;
            Destroy(this, 0.5f);
        }
    }
}
