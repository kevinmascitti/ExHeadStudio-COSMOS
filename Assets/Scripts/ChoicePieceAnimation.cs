using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoicePieceAnimation : MonoBehaviour
{

    public void Select(Vector3 begin, Vector3 end)
    {
        StartCoroutine(SelectCoroutine(begin, end));
    }

    public void Deselect(Vector3 begin, Vector3 end)
    {
        StartCoroutine(DeselectCoroutine(begin, end));
    }

    private IEnumerator SelectCoroutine(Vector3 begin, Vector3 end)
    {
        float elapsedTime = 0f;
        float transitionDuration = 0.4f;

        while (elapsedTime < transitionDuration)
        {
            transform.position = Vector3.Lerp(begin, end, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
    }
    
    private IEnumerator DeselectCoroutine(Vector3 begin, Vector3 end)
    {
        float elapsedTime = 0f;
        float transitionDuration = 0.4f;

        while (elapsedTime < transitionDuration)
        {
            transform.position = Vector3.Lerp(begin, end, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        Destroy(gameObject);
    }
    
}
