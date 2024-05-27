using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitter : MonoBehaviour
{
    public static EventHandler<PlayerCollisionArgs> OnPlayerCollision;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player damage!");
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