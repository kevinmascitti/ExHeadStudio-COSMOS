using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSwapper : MonoBehaviour
{
    // versione base, può, funzioanare se è da usare poche volte.


    [Tooltip("Ho inserito un pezzo preso direttamente dalla empty del player")]
    [SerializeField] private GameObject pieceToSet;

    private Piece[] pieceArray;

    private Piece pieceToSetScript;

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

            Destroy(this, 0.5f);
        }
    }
}
