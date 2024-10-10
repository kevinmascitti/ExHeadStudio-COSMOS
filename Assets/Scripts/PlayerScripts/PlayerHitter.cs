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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            OnPlayerCollision?.Invoke(this, new PlayerCollisionArgs(other.gameObject.GetComponent<PlayerCharacter>(), gameObject.GetComponentInParent<Enemy>().GetInstanceID()));
        }
    }

}

public class PlayerCollisionArgs : EventArgs
{
    public PlayerCollisionArgs(PlayerCharacter p, int i)
    {
        player = p;
        //hitter = h;
        id = i;
    }

    public PlayerCharacter player;
    //public PlayerHitter hitter;
    public int id;
}