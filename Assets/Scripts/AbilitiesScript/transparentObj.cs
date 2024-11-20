using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparentObj : MonoBehaviour
{
    [SerializeField] private Material objMaterial;
    private Renderer objRenderer;

    void Awake()
    {
        objRenderer = GetComponent<Renderer>();
        objRenderer.material = objMaterial;
        objMaterial.color = new Color(objMaterial.color.r, objMaterial.color.g, objMaterial.color.b, 0f);
    }
}
