using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public string name;
    public int numberInList;
    public GameObject prefab;
    public PartType type;
    public bool isUnlocked = true;
    public Stats stats;
    public Element element;
    public string description;

    //METTERE OUTLINE A 20
    //Layer UI per i pezzi da assegnare, tali pezzi sono nella cartella Resources/Pieces, come prefab dei pezzi di Cyrus che hanno gli script UIpiece e Outline

    // Start is called before the first frame update
    public void Awake()
    {
        name = gameObject.name;
        prefab = Resources.Load<GameObject>("Pieces/" + name);
        stats = new Stats();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
