using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class ChoicePieceManager : MonoBehaviour
{
    [NonSerialized] public bool isUIOpen = false;
    [SerializeField] private GameObject canvasChoicePieces;
    
    // dizionario dei pezzi presenti nella UI di scelta pezzo
    [SerializeField] private Dictionary<PartType, GameObject> compositionUI = new Dictionary<PartType, GameObject>();

    [SerializeField] private GameObject headEmpty;
    [SerializeField] private GameObject leftArmEmpty;
    [SerializeField] private GameObject rightArmEmpty;
    [SerializeField] private GameObject bodyEmpty;
    [SerializeField] private GameObject legsEmpty;
    // dizionario dei box delle partType utili all'arrowIndicator
    [SerializeField] private Dictionary<PartType, GameObject> partTypeEmpties = new Dictionary<PartType, GameObject>();

    // lista degli sprite da utilizzare per ogni elemento nella UI: in ordine come l'enum Element
    [SerializeField] private List<Sprite> elementSymbols = new List<Sprite>();

    [SerializeField] private PlayerCharacter player;
    [SerializeField] private Camera choicePiecesCamera;

    [SerializeField] private TMP_Text TMP_title;
    [SerializeField] private TMP_Text TMP_element;
    [SerializeField] private Image TMP_elementImage;
    [SerializeField] private TMP_Text TMP_stats;
    [SerializeField] private TMP_Text TMP_description;
        
    // la partType selezionata nella UI
    [SerializeField] private PartType selectedPartType;
    // il numero del pezzo selezionato per partType, serve ad accedere alla lista dei modelli di pezzi del player
    [SerializeField] private Dictionary<PartType, int> selectedPieceNumbers = new Dictionary<PartType, int>();
    
    public static EventHandler<ChangePieceArgs> OnChangePiece;

    // Start is called before the first frame update
    void Start()
    {
        partTypeEmpties[PartType.Head] = headEmpty;
        partTypeEmpties[PartType.LeftArm] = leftArmEmpty;
        partTypeEmpties[PartType.RightArm] = rightArmEmpty;
        partTypeEmpties[PartType.Body] = bodyEmpty;
        partTypeEmpties[PartType.Legs] = legsEmpty;
        
        selectedPieceNumbers.Add(PartType.Head, 0);
        selectedPieceNumbers.Add(PartType.LeftArm, 0);
        selectedPieceNumbers.Add(PartType.RightArm, 0);
        selectedPieceNumbers.Add(PartType.Body, 0);
        selectedPieceNumbers.Add(PartType.Legs, 0);
        
        ChoicePieceButton.OnClickedArrow += UpdateUI;
        PlayerCharacter.OnChoicePieces += OpenChoicePiecesUI;
        PlayerCharacter.OnEndChoicePieces += CloseChoicePiecesUI;
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

    public void OpenChoicePiecesUI(object sender, EventArgs args)
    {
        if (!isUIOpen)
        {
            Debug.Log("UI APERTA");
            isUIOpen = true;
            selectedPieceNumbers[PartType.Head] = player.composition[PartType.Head].numberInList;
            selectedPieceNumbers[PartType.Body] = player.composition[PartType.Body].numberInList;
            selectedPieceNumbers[PartType.RightArm] = player.composition[PartType.RightArm].numberInList;
            selectedPieceNumbers[PartType.LeftArm] = player.composition[PartType.LeftArm].numberInList;
            selectedPieceNumbers[PartType.Legs] = player.composition[PartType.Legs].numberInList;

            compositionUI[PartType.Head] = Instantiate(
                player.completePiecesList[PartType.Head][selectedPieceNumbers[PartType.Head]].prefab,
                partTypeEmpties[PartType.Head].transform.position, Quaternion.Euler(270, 180, 0),
                partTypeEmpties[PartType.Head].transform);
            compositionUI[PartType.Body] = Instantiate(
                player.completePiecesList[PartType.Body][selectedPieceNumbers[PartType.Body]].prefab,
                partTypeEmpties[PartType.Body].transform.position, Quaternion.Euler(270, 180, 0),
                partTypeEmpties[PartType.Body].transform);
            compositionUI[PartType.RightArm] = Instantiate(
                player.completePiecesList[PartType.RightArm][selectedPieceNumbers[PartType.RightArm]].prefab,
                partTypeEmpties[PartType.RightArm].transform.position, Quaternion.Euler(270, 180, 0),
                partTypeEmpties[PartType.RightArm].transform);
            compositionUI[PartType.LeftArm] = Instantiate(
                player.completePiecesList[PartType.LeftArm][selectedPieceNumbers[PartType.LeftArm]].prefab,
                partTypeEmpties[PartType.LeftArm].transform.position, Quaternion.Euler(270, 180, 0),
                partTypeEmpties[PartType.LeftArm].transform);
            compositionUI[PartType.Legs] = Instantiate(
                player.completePiecesList[PartType.Legs][selectedPieceNumbers[PartType.Legs]].prefab,
                partTypeEmpties[PartType.Legs].transform.position, Quaternion.Euler(270, 180, 0),
                partTypeEmpties[PartType.Legs].transform);

            selectedPartType = PartType.Head;
            partTypeEmpties[selectedPartType].GetComponent<ArrowIndicator>().ShowArrows();

            string scriptName = "Outline";
            var script = compositionUI[selectedPartType].GetComponent(scriptName) as MonoBehaviour;
            script.enabled = true;

            canvasChoicePieces.SetActive(true);
        }
    }

    public void CloseChoicePiecesUI(object sender, EventArgs args)
    {
        if (isUIOpen)
        {
            Debug.Log("UI CHIUSA");
            isUIOpen = false;
            partTypeEmpties[selectedPartType].GetComponent<ArrowIndicator>().HideArrows();

            string scriptName = "Outline";
            var script = compositionUI[selectedPartType].GetComponent(scriptName) as MonoBehaviour;
            script.enabled = false;

            Destroy(compositionUI[PartType.Head]);
            Destroy(compositionUI[PartType.Body]);
            Destroy(compositionUI[PartType.RightArm]);
            Destroy(compositionUI[PartType.LeftArm]);
            Destroy(compositionUI[PartType.Legs]);

            canvasChoicePieces.SetActive(false);
        }
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
        
        string scriptName = "Outline";
        var scriptOld = compositionUI[selectedPartType].GetComponent(scriptName) as MonoBehaviour;
        scriptOld.enabled = false;
        
        int newSelectedPartTypeNumber = (int) (selectedPartType - 1) % Enum.GetValues(typeof(PartType)).Length;
        if (newSelectedPartTypeNumber == -1)
            newSelectedPartTypeNumber = Enum.GetValues(typeof(PartType)).Length - 1;
        selectedPartType = (PartType) newSelectedPartTypeNumber;
        partTypeEmpties[selectedPartType].GetComponent<ArrowIndicator>().ShowArrows();
        
        var scriptNew = compositionUI[selectedPartType].GetComponent(scriptName) as MonoBehaviour;
        scriptNew.enabled = true;
        
        UpdateUIInformation(player.completePiecesList[selectedPartType][selectedPieceNumbers[selectedPartType]]);
    }
    
    private void NextPartType()
    {
        partTypeEmpties[selectedPartType].GetComponent<ArrowIndicator>().HideArrows();
        
        string scriptName = "Outline";
        var scriptOld = compositionUI[selectedPartType].GetComponent(scriptName) as MonoBehaviour;
        scriptOld.enabled = false;
        
        int newSelectedPartTypeNumber = (int) (selectedPartType + 1) % Enum.GetValues(typeof(PartType)).Length;
        selectedPartType = (PartType) newSelectedPartTypeNumber;
        partTypeEmpties[selectedPartType].GetComponent<ArrowIndicator>().ShowArrows();
        
        var scriptNew = compositionUI[selectedPartType].GetComponent(scriptName) as MonoBehaviour;
        scriptNew.enabled = true;
        
        UpdateUIInformation(player.completePiecesList[selectedPartType][selectedPieceNumbers[selectedPartType]]);
    }
    
    private void PreviousPiece()
    {
        int oldPieceNumber = selectedPieceNumbers[selectedPartType];
        
        string scriptName = "Outline";
        var scriptOld = compositionUI[selectedPartType].GetComponent(scriptName) as MonoBehaviour;
        scriptOld.enabled = false;
        
        FindPreviousUnlockedPieceNumber();
        int newPieceNumber = selectedPieceNumbers[selectedPartType];
        OnChangePiece?.Invoke(this, new ChangePieceArgs(selectedPartType, oldPieceNumber, newPieceNumber));
        
        compositionUI[selectedPartType].GetComponent<UIPiece>().Deselect(partTypeEmpties[selectedPartType].transform.position, partTypeEmpties[selectedPartType].transform.position + new Vector3 (2,0,0));
        compositionUI[selectedPartType] = Instantiate(player.completePiecesList[selectedPartType][newPieceNumber].prefab, partTypeEmpties[selectedPartType].transform.position - new Vector3 (2,0,0), Quaternion.Euler(270, 180,0), partTypeEmpties[selectedPartType].transform);
        compositionUI[selectedPartType].GetComponent<UIPiece>().Select(partTypeEmpties[selectedPartType].transform.position - new Vector3 (2,0,0), partTypeEmpties[selectedPartType].transform.position);
        
        var scriptNew = compositionUI[selectedPartType].GetComponent(scriptName) as MonoBehaviour;
        scriptNew.enabled = true;
        
        UpdateUIInformation(player.completePiecesList[selectedPartType][selectedPieceNumbers[selectedPartType]]);
    }
    
    private void NextPiece()
    {
        int oldPieceNumber = selectedPieceNumbers[selectedPartType];
        
        string scriptName = "Outline";
        var scriptOld = compositionUI[selectedPartType].GetComponent(scriptName) as MonoBehaviour;
        scriptOld.enabled = false;
        
        FindNextUnlockedPieceNumber();
        int newPieceNumber = selectedPieceNumbers[selectedPartType];
        OnChangePiece?.Invoke(this, new ChangePieceArgs(selectedPartType, oldPieceNumber, newPieceNumber));
        
        compositionUI[selectedPartType].GetComponent<UIPiece>().Deselect(partTypeEmpties[selectedPartType].transform.position, partTypeEmpties[selectedPartType].transform.position - new Vector3 (2,0,0));
        compositionUI[selectedPartType] = Instantiate(player.completePiecesList[selectedPartType][newPieceNumber].prefab, partTypeEmpties[selectedPartType].transform.position + new Vector3 (2,0,0), Quaternion.Euler(270, 180,0), partTypeEmpties[selectedPartType].transform);
        compositionUI[selectedPartType].GetComponent<UIPiece>().Select(partTypeEmpties[selectedPartType].transform.position + new Vector3 (2,0,0), partTypeEmpties[selectedPartType].transform.position);

        var scriptNew = compositionUI[selectedPartType].GetComponent(scriptName) as MonoBehaviour;
        scriptNew.enabled = true;
        
        UpdateUIInformation(player.completePiecesList[selectedPartType][selectedPieceNumbers[selectedPartType]]);
    }

    private void FindPreviousUnlockedPieceNumber()
    {
        selectedPieceNumbers[selectedPartType]--;
        if (selectedPieceNumbers[selectedPartType] == -1)
            selectedPieceNumbers[selectedPartType] = player.completePiecesList[selectedPartType].Count - 1;
        while (!player.completePiecesList[selectedPartType][selectedPieceNumbers[selectedPartType]].isUnlocked)
        {
            selectedPieceNumbers[selectedPartType]--;
            if (selectedPieceNumbers[selectedPartType] == -1)
                selectedPieceNumbers[selectedPartType] = player.completePiecesList[selectedPartType].Count - 1;
        }
    }

    private void FindNextUnlockedPieceNumber()
    {
        selectedPieceNumbers[selectedPartType] = (selectedPieceNumbers[selectedPartType] + 1) % player.completePiecesList[selectedPartType].Count;

        while (!player.completePiecesList[selectedPartType][selectedPieceNumbers[selectedPartType]].isUnlocked)
            selectedPieceNumbers[selectedPartType] = (selectedPieceNumbers[selectedPartType] + 1) % player.completePiecesList[selectedPartType].Count;
    }

    private void UpdateUIInformation(Piece piece)
    {
        TMP_title.text = piece.name;
        TMP_element.text = piece.element.ToString();
        TMP_elementImage.sprite = elementSymbols[(int) piece.element];
        TMP_stats.text = "Atk: " + piece.stats.atk + "\nDef: " + piece.stats.def;
        TMP_description.text = piece.description;
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