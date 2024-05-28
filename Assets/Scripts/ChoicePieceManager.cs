using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoicePieceManager : MonoBehaviour
{
    // dizionari con le posizioni utili per la UI di scelta dei pezzi
    [SerializeField] private Dictionary<PartType, Vector3> firstRightPositions;
    [SerializeField] private Dictionary<PartType, Vector3> uiPositions;
    [SerializeField] private Dictionary<PartType, Vector3> lastLeftPositions;

    // dizionario della lista completa dei pezzi presenti nel gioco, ogni pezzo può essere sbloccato o meno
    [SerializeField] private Dictionary<Piece, GameObject> piecesPrefabs;
    
    // dizionario dei pezzi presenti nella UI di scelta pezzo
    [SerializeField] private Dictionary<PartType, GameObject> compositionUI;

    [SerializeField] private PlayerCharacter player;
    [SerializeField] private PartType selectedPartType; // la partType selezionata nella UI
    [SerializeField] private int selectedPieceNumber;   // il numero del pezzo selezionato, serve ad accedere alla lista dei modelli di pezzi del player

    public static EventHandler<ChangePieceArgs> OnChangePiece;

    // Start is called before the first frame update
    void Start()
    {
        foreach (PartType partType in player.completePiecesList.Keys)
        {
            foreach (Piece piece in player.completePiecesList[partType])
            {
                // Riempimento della lista COMPLETA di pezzi
                // ogni pezzo deve avere il suo nome esatto definito come il nome del prefab 
                // e ogni pezzo deve essere nella cartella corretta della sua PartType
                piecesPrefabs.Add(piece, (GameObject) Resources.Load("Pieces/"+partType+"/"+piece.name));
            }
        }

    }

    public void PreviousPartType()
    {
        int newSelectedPartTypeNumber = (int) (selectedPartType - 1) % Enum.GetValues(typeof(PartType)).Length;
        if (newSelectedPartTypeNumber == -1)
            newSelectedPartTypeNumber = Enum.GetValues(typeof(PartType)).Length - 1;
        selectedPartType = (PartType) newSelectedPartTypeNumber;
    }
    
    public void NextPartType()
    {
        int newSelectedPartTypeNumber = (int) (selectedPartType + 1) % Enum.GetValues(typeof(PartType)).Length;
        if (newSelectedPartTypeNumber == -1)
            newSelectedPartTypeNumber = Enum.GetValues(typeof(PartType)).Length - 1;
        selectedPartType = (PartType) newSelectedPartTypeNumber;
    }
    
    public void PreviousPiece()
    {
        int oldPieceNumber = selectedPieceNumber;
        FindPreviousUnlockedPieceNumber();
        int newPieceNumber = selectedPieceNumber;
        OnChangePiece?.Invoke(this, new ChangePieceArgs(selectedPartType, oldPieceNumber, newPieceNumber));
        
        compositionUI[selectedPartType].GetComponent<ChoicePieceAnimation>().Deselect(uiPositions[selectedPartType], firstRightPositions[selectedPartType]);
        compositionUI[selectedPartType] = Instantiate(piecesPrefabs[player.completePiecesList[selectedPartType][newPieceNumber]], lastLeftPositions[selectedPartType], new Quaternion(0,0,0,0));
        compositionUI[selectedPartType].GetComponent<ChoicePieceAnimation>().Select(lastLeftPositions[selectedPartType], uiPositions[selectedPartType]);

        UpdateUIInformation(player.completePiecesList[selectedPartType][selectedPieceNumber]);
    }
    
    public void NextPiece()
    {
        int oldPieceNumber = selectedPieceNumber;
        FindNextUnlockedPieceNumber();
        int newPieceNumber = selectedPieceNumber;
        OnChangePiece?.Invoke(this, new ChangePieceArgs(selectedPartType, oldPieceNumber, newPieceNumber));
        
        compositionUI[selectedPartType].GetComponent<ChoicePieceAnimation>().Deselect(uiPositions[selectedPartType], lastLeftPositions[selectedPartType]);
        compositionUI[selectedPartType] = Instantiate(piecesPrefabs[player.completePiecesList[selectedPartType][newPieceNumber]], firstRightPositions[selectedPartType], new Quaternion(0,0,0,0));
        compositionUI[selectedPartType].GetComponent<ChoicePieceAnimation>().Select(firstRightPositions[selectedPartType], uiPositions[selectedPartType]);

        UpdateUIInformation(player.completePiecesList[selectedPartType][selectedPieceNumber]);
    }

    public void FindPreviousUnlockedPieceNumber()
    {
        selectedPieceNumber--;
        while (!player.completePiecesList[selectedPartType][selectedPieceNumber].isUnlocked)
            selectedPieceNumber--;
    }

    public void FindNextUnlockedPieceNumber()
    {
        selectedPieceNumber++;
        while (!player.completePiecesList[selectedPartType][selectedPieceNumber].isUnlocked)
            selectedPieceNumber++;
    }

    public void UpdateUIInformation(Piece piece)
    {
        
    }

}

public class ChangePieceArgs : EventArgs 
{
    public ChangePieceArgs(PartType t, int old, int n)
    {
        partType = t;
        oldPieceNumber = old;
        newPieceNumber = n;
    }

    public PartType partType;
    public int oldPieceNumber;
    public int newPieceNumber;
}