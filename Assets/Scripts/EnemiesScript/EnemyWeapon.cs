using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyWeaponType
{
    Punch,
    Sword,
    Claws,
    Cannon
}
public class EnemyWeapon : PlayerHitter
{

        public EnemyWeaponType type;
        public Element element;
        public float movementSpeed;
        public AnimationClip baseAttack;
        public AnimationClip strongAttack;
    private void Awake()
    {
        if (type == EnemyWeaponType.Punch)
        {
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
    private void Start()
    {

        element = gameObject.GetComponentInParent<Enemy>().enemyElement;
    }

    public void SetCollider(bool state)
    {
        GetComponent<BoxCollider>().enabled = state;
    }

}
