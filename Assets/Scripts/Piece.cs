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

    // Start is called before the first frame update
    void Start()
    {
        name = gameObject.name;
        // prefab = (GameObject) Resources.Load("Pieces/" + name);
        stats = new Stats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
