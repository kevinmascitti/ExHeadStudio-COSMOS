using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitter : MonoBehaviour
{
    public static EventHandler<PlayerCollisionArgs> OnPlayerCollision;
    

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("l'enemy ha colpito: " + other.gameObject.layer);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            OnPlayerCollision?.Invoke(this, new PlayerCollisionArgs(other.gameObject.GetComponent<PlayerCharacter>(), this));
        }
    }

}

public class PlayerCollisionArgs : EventArgs
{
    public PlayerCollisionArgs(PlayerCharacter p, PlayerHitter h)
    {
        player = p;
        hitter = h;
    }

    public PlayerCharacter player;
    public PlayerHitter hitter;
}