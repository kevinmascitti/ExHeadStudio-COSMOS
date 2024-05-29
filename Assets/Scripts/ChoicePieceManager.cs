using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class ChoicePieceManager : MonoBehaviour
{
    [NonSerialized] public bool isUIOpen = true;
    
    // dizionario dei pezzi presenti nella UI di scelta pezzo
    [SerializeField] private Dictionary<PartType, GameObject> compositionUI = new Dictionary<PartType, GameObject>();
    
    // dizionario dei box delle partType utili all'arrowIndicator
    [SerializeField] private Dictionary<PartType, GameObject> partTypeEmpties = new Dictionary<PartType, GameObject>();

    [SerializeField] private PlayerCharacter player;
    [SerializeField] private Camera choicePiecesCamera;
        
    // la partType selezionata nella UI
    [SerializeField] private PartType selectedPartType;
    // il numero del pezzo selezionato per partType, serve ad accedere alla lista dei modelli di pezzi del player
    [SerializeField] private Dictionary<PartType, int> selectedPieceNumbers = new Dictionary<PartType, int>();
    
    public static EventHandler<ChangePieceArgs> OnChangePiece;

    // Start is called before the first frame update
    void Start()
    {
        OpenChoicePiecesUI();

        ChoicePieceButton.OnClickedArrow += UpdateUI;
    }

    void Update()
    {
        if (isUIOpen)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextPiece();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PreviousPiece();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                NextPartType();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                PreviousPartType();
            }
        }
    }

    public void OpenChoicePiecesUI()
    {
        isUIOpen = true;
        selectedPieceNumbers[PartType.Head] = player.composition[PartType.Head].numberInList;
        selectedPieceNumbers[PartType.Body] = player.composition[PartType.Body].numberInList;
        selectedPieceNumbers[PartType.RightArm] = player.composition[PartType.RightArm].numberInList;
        selectedPieceNumbers[PartType.LeftArm] = player.composition[PartType.LeftArm].numberInList;
        selectedPieceNumbers[PartType.Legs] = player.composition[PartType.Legs].numberInList;

        Instantiate(player.completePiecesList[PartType.Head][selectedPieceNumbers[PartType.Head]].prefab, partTypeEmpties[PartType.Head].transform.position, new Quaternion(0,0,0,0), partTypeEmpties[PartType.Head].transform);
        Instantiate(player.completePiecesList[PartType.Body][selectedPieceNumbers[PartType.Body]].prefab, partTypeEmpties[PartType.Body].transform.position, new Quaternion(0,0,0,0), partTypeEmpties[PartType.Body].transform);
        Instantiate(player.completePiecesList[PartType.RightArm][selectedPieceNumbers[PartType.RightArm]].prefab, partTypeEmpties[PartType.RightArm].transform.position, new Quaternion(0,0,0,0), partTypeEmpties[PartType.RightArm].transform);
        Instantiate(player.completePiecesList[PartType.LeftArm][selectedPieceNumbers[PartType.LeftArm]].prefab, partTypeEmpties[PartType.LeftArm].transform.position, new Quaternion(0,0,0,0), partTypeEmpties[PartType.LeftArm].transform);
        Instantiate(player.completePiecesList[PartType.Legs][selectedPieceNumbers[PartType.Legs]].prefab, partTypeEmpties[PartType.Legs].transform.position, new Quaternion(0,0,0,0), partTypeEmpties[PartType.Legs].transform);
        
        selectedPartType = PartType.Head;
        partTypeEmpties[selectedPartType].GetComponent<ArrowIndicator>().ShowArrows();
    }

    private void UpdateUI(object sender, bool isRight)
    {
        if (isUIOpen)
        {
            if (isRight)
            {
                Debug.Log("Right");
                NextPiece();
            }
            else
            {
                PreviousPiece();
                Debug.Log("Left");
            }
        }
    }

    private void PreviousPartType()
    {
        partTypeEmpties[selectedPartType].GetComponent<ArrowIndicator>().HideArrows();
        int newSelectedPartTypeNumber = (int) (selectedPartType - 1) % Enum.GetValues(typeof(PartType)).Length;
        if (newSelectedPartTypeNumber == -1)
            newSelectedPartTypeNumber = Enum.GetValues(typeof(PartType)).Length - 1;
        selectedPartType = (PartType) newSelectedPartTypeNumber;
        partTypeEmpties[selectedPartType].GetComponent<ArrowIndicator>().ShowArrows();
    }
    
    private void NextPartType()
    {
        partTypeEmpties[selectedPartType].GetComponent<ArrowIndicator>().HideArrows();
        int newSelectedPartTypeNumber = (int) (selectedPartType + 1) % Enum.GetValues(typeof(PartType)).Length;
        selectedPartType = (PartType) newSelectedPartTypeNumber;
        partTypeEmpties[selectedPartType].GetComponent<ArrowIndicator>().ShowArrows();
    }
    
    private void PreviousPiece()
    {
        int oldPieceNumber = selectedPieceNumbers[selectedPartType];
        FindPreviousUnlockedPieceNumber();
        int newPieceNumber = selectedPieceNumbers[selectedPartType];
        OnChangePiece?.Invoke(this, new ChangePieceArgs(selectedPartType, oldPieceNumber, newPieceNumber));
        
        compositionUI[selectedPartType].GetComponent<UIPiece>().Deselect(partTypeEmpties[selectedPartType].transform.position, partTypeEmpties[selectedPartType].transform.position + new Vector3 (2,0,0));
        compositionUI[selectedPartType] = Instantiate(player.completePiecesList[selectedPartType][newPieceNumber].prefab, partTypeEmpties[selectedPartType].transform.position - new Vector3 (2,0,0), new Quaternion(0,0,0,0), partTypeEmpties[selectedPartType].transform);
        compositionUI[selectedPartType].GetComponent<UIPiece>().Select(partTypeEmpties[selectedPartType].transform.position - new Vector3 (2,0,0), partTypeEmpties[selectedPartType].transform.position);

        UpdateUIInformation(player.completePiecesList[selectedPartType][selectedPieceNumbers[selectedPartType]]);
    }
    
    private void NextPiece()
    {
        int oldPieceNumber = selectedPieceNumbers[selectedPartType];
        FindNextUnlockedPieceNumber();
        int newPieceNumber = selectedPieceNumbers[selectedPartType];
        OnChangePiece?.Invoke(this, new ChangePieceArgs(selectedPartType, oldPieceNumber, newPieceNumber));
        
        compositionUI[selectedPartType].GetComponent<UIPiece>().Deselect(partTypeEmpties[selectedPartType].transform.position, partTypeEmpties[selectedPartType].transform.position - new Vector3 (2,0,0));
        compositionUI[selectedPartType] = Instantiate(player.completePiecesList[selectedPartType][newPieceNumber].prefab, partTypeEmpties[selectedPartType].transform.position + new Vector3 (2,0,0), new Quaternion(0,0,0,0), partTypeEmpties[selectedPartType].transform);
        compositionUI[selectedPartType].GetComponent<UIPiece>().Select(partTypeEmpties[selectedPartType].transform.position + new Vector3 (2,0,0), partTypeEmpties[selectedPartType].transform.position);

        UpdateUIInformation(player.completePiecesList[selectedPartType][selectedPieceNumbers[selectedPartType]]);
    }

    private void FindPreviousUnlockedPieceNumber()
    {
        selectedPieceNumbers[selectedPartType]--;
        while (!player.completePiecesList[selectedPartType][selectedPieceNumbers[selectedPartType]].isUnlocked)
            selectedPieceNumbers[selectedPartType]--;
    }

    private void FindNextUnlockedPieceNumber()
    {
        selectedPieceNumbers[selectedPartType]++;
        while (!player.completePiecesList[selectedPartType][selectedPieceNumbers[selectedPartType]].isUnlocked)
            selectedPieceNumbers[selectedPartType]++;
    }

    private void UpdateUIInformation(Piece piece)
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