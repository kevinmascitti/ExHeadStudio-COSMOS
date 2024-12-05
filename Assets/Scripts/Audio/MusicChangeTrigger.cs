using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    [Header("Area")]
    [SerializeField] private MusicState state;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player")) 
        {
            AudioManager.instance.SetMusicState(state);
            Debug.Log("cambio stato musica");
            Debug.Log(state);
        }
    }
}
