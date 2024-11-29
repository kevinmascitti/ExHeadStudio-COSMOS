using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHintsController : MonoBehaviour
{
    [SerializeField] Canvas hintsCanvas;
    //[SerializeField] List<string> hints = new List<string>();
    //Dictionary<int, string> hintsDic = new Dictionary<int, string>();
    public static EventHandler<HintArgs> showHint;
    private void Awake()
    {
        // hintsCanvas = GetComponent<Canvas>();
       /* int id = 0;
        foreach (string hint in hints)
        {
            
            hintsDic.Add(id, hint);
            ++id;
        }
       */
        showHint += ShowHintOnScreen;
    }
    private void OnDestroy()
    {
        showHint -= ShowHintOnScreen;
    }


    private void ShowHintOnScreen(object sender, HintArgs e)
    {
        
            hintsCanvas.GetComponentInChildren<TextMeshProUGUI>().text = e.hintText;
            hintsCanvas.GetComponent<CanvasGroup>().alpha = 0f;
            hintsCanvas.enabled = true;
            StartCoroutine(FadeInCoroutine());
        
    }

    IEnumerator FadeInCoroutine()
    {
        for(float a=0.0f; a<=1f; a+=0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            hintsCanvas.GetComponent<CanvasGroup>().alpha = a;
        }
        hintsCanvas.GetComponent<CanvasGroup>().alpha = 1f;
        StartCoroutine(FadeOutCoroutine());
    }
    IEnumerator FadeOutCoroutine()
    {
        yield return new WaitForSeconds(5f);
        for(float a=1.0f; a >= 0f; a -= 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            hintsCanvas.GetComponent<CanvasGroup>().alpha = a;
        }
        hintsCanvas.GetComponent<CanvasGroup>().alpha = 0f;
        hintsCanvas.enabled = false;
    }
}
    public class HintArgs: EventArgs
{
    public string hintText;
    public HintArgs(string text)
    {
        hintText = text;
    }

    
}