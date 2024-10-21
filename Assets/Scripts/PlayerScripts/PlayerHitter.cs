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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !other.TryGetComponent<ShieldObj>( out ShieldObj shield))
        {

<<<<<<< HEAD
            OnPlayerCollision?.Invoke(this, new PlayerCollisionArgs(other.gameObject.GetComponent<PlayerCharacter>(), gameObject.GetComponentInParent<Enemy>().GetInstanceID()));
=======
           OnPlayerCollision?.Invoke(this, new PlayerCollisionArgs(other.gameObject.GetComponent<PlayerCharacter>(), this, gameObject.GetComponentInParent<Enemy>().GetInstanceID()));
>>>>>>> testing
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