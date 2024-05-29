using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public float distanceFromObject = 1.0f;

    private const int nArrows = 2;
    private GameObject[] arrows;

    void Start()
    {
        arrows = new GameObject[nArrows];
        for (int i = 0; i < nArrows; i++)
        {
            arrows[i] = Instantiate((GameObject) Resources.Load("UIArrow"));
            arrows[i].SetActive(false);
        }
    }

    public void ShowArrows()
    {
        Bounds bounds = GetComponent<BoxCollider>().bounds;

        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        Vector3[] positions = new Vector3[nArrows];
        positions[2] = new Vector3(center.x + extents.x + distanceFromObject, center.y, center.z); // Destra
        positions[3] = new Vector3(center.x - extents.x - distanceFromObject, center.y, center.z); // Sinistra

        for (int i = 0; i < nArrows; i++)
        {
            arrows[i].SetActive(true);
            arrows[i].transform.position = positions[i];
            arrows[i].transform.LookAt(center);
        }
    }

    public void HideArrows()
    {
        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(false);
        }
    }
}
