using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitter : MonoBehaviour
{
    public static EventHandler<PlayerCollisionArgs> OnPlayerCollision;
    public Enemy _enemy;

   

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !other.TryGetComponent<ShieldObj>( out ShieldObj shield))
        {
            if(other.gameObject.GetComponentInParent<PlayerCharacter>()!=null)
                _enemy.DoDamage(other.gameObject.GetComponentInParent<PlayerCharacter>());
            //OnPlayerCollision?.Invoke(this, new PlayerCollisionArgs(other.gameObject.GetComponent<PlayerCharacter>(), gameObject.GetComponentInParent<Enemy>().GetInstanceID()));

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