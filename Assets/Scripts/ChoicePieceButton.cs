using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoicePieceButton : MonoBehaviour, IPointerClickHandler
{
    private bool isRight;

    public static EventHandler<bool> OnClickedArrow;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        EventSystem.current.SetSelectedGameObject(null);
        OnClickedArrow?.Invoke(this, isRight);
    }
    
}
