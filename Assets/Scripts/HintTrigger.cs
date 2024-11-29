using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{

    //public int hintNumber;
    public string hintText;
    public bool neverShownYet=true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && neverShownYet)
        {
            neverShownYet = false;
            UIHintsController.showHint?.Invoke(this, new HintArgs(hintText));
        }
    }
}
