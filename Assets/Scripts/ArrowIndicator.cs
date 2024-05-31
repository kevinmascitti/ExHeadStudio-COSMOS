using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public float distanceFromObject = 1.0f;

    private GameObject[] arrows;
    [SerializeField] private Vector2 rightPos;
    [SerializeField] private Vector2 leftPos;
    [SerializeField] private GameObject canvasChoicePieces;

    void Start()
    {
        Bounds bounds = GetComponent<BoxCollider>().bounds;
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;
        
        arrows = new GameObject[2];
        arrows[0] = InstantiateButton(true, rightPos);
        arrows[1] = InstantiateButton(false, leftPos);
        arrows[0].SetActive(false);
        arrows[1].SetActive(false);
    }
    
    public GameObject InstantiateButton(bool isRight, Vector2 position)
    {
        GameObject newButton;
        if (isRight)
            newButton = Instantiate((GameObject) Resources.Load("NextButton"), canvasChoicePieces.transform);
        else
            newButton = Instantiate((GameObject) Resources.Load("PreviousButton"), canvasChoicePieces.transform);

        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        return newButton;
    }

    public void ShowArrows()
    {
        arrows[0].SetActive(true);
        arrows[1].SetActive(true);
    }

    public void HideArrows()
    {
        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(false);
        }
    }
}
