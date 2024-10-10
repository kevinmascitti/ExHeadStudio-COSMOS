using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Rendering;

public class LockOnCamSwitcher : MonoBehaviour
{

    [SerializeField] CinemachineFreeLook playerFreeLookCam;
    [SerializeField] CinemachineFreeLook lockOnCam;
    [SerializeField] CinemachineTargetGroup targetGroup;
    [SerializeField] GameObject lockOnIcon;

    private RaycastHit[] enemyArray;
    [SerializeField] private LayerMask lockOnMask;
    private string lockOnName;
    private bool lockOnSwitcher = true;

    public bool lockOn = false;
    private int enemyIndex = 1;
    private int enemyNumber = 0;
    void Start()
    {

        lockOnName = lockOnIcon.name;
        playerFreeLookCam.Priority = 11;
        lockOnCam.Priority = 10;
        Enemy.OnEnemyDestroyed += RemoveEnemy;
        enemyArray = new RaycastHit[20];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(!lockOn)
                 enemyNumber = Physics.BoxCastNonAlloc(transform.position, new Vector3(20, 10, 2), transform.forward, enemyArray, Quaternion.identity, 40f, lockOnMask, QueryTriggerInteraction.Collide);

            if(enemyNumber == 0) //cambiato con enemyArray.lenght
            {
                Debug.Log("Nessun nemico trovato");
            }
            else
            {
                foreach(RaycastHit enemy in enemyArray)
                {
                   // Debug.Log(enemy.collider.name); //aggiunge tutti i collider ATTIVI con lo stesso layer, non va bene
                    if(targetGroup.FindMember(enemy.transform) == -1)//conidzione del metodo
                    {
                        targetGroup.AddMember(enemy.transform, 1.5f, 2f);
                    }

                }
            }
            
            if (lockOnSwitcher && targetGroup.m_Targets.Length >1)
            {
                Debug.Log("cambiocameraLock");
                playerFreeLookCam.Priority = 10;
                lockOnCam.Priority = 11;
                lockOnSwitcher = false;
                lockOn = true;
                //targetGroup.m_Targets[GetEnemyIndex()].target.Find(lockOnName).gameObject.SetActive(true);
                //Debug.Log(targetGroup.m_Targets[GetEnemyIndex()].target.GetChild(0).GetChild(0).name);
                }
            else
            {
                Debug.Log("cambiocameraFree");
                for (int i = 1; i < targetGroup.m_Targets.Length; i++)
                {
                    targetGroup.RemoveMember(targetGroup.m_Targets[i].target);
                }

                playerFreeLookCam.Priority = 11;
                lockOnCam.Priority = 10;
                lockOnSwitcher = true;
                lockOn = false;

                if (targetGroup.m_Targets.Length >1)
                {
                    //targetGroup.m_Targets[GetEnemyIndex()].target.Find(lockOnName).gameObject.SetActive(false);
                }

                enemyIndex = 1;
            }

        }

        if (lockOn)
        {
            if (targetGroup.m_Targets.Length == 1) lockOn = false;
            if (Input.GetKeyDown(KeyCode.Mouse2)) //TASTO CENTRALE DEL MOUSE
            {
                Debug.Log("Ho cambiato nemico");

                if (targetGroup.m_Targets[GetEnemyIndex()].target == null)
                {
                    targetGroup.RemoveMember(targetGroup.m_Targets[GetEnemyIndex()].target);
                    enemyIndex++;
                }
                else
                {
                    //targetGroup.m_Targets[GetEnemyIndex()].target.Find(lockOnName).gameObject.SetActive(false);
                }

                if (enemyIndex < targetGroup.m_Targets.Length && targetGroup.m_Targets.Length > 1)
                {
                    enemyIndex++;
                }

            }

            if (enemyIndex >= targetGroup.m_Targets.Length)
            {
                enemyIndex = 1;
            }
            if (targetGroup.m_Targets[GetEnemyIndex()].target != null)
            {
                //targetGroup.m_Targets[GetEnemyIndex()].target.Find(lockOnName).gameObject.SetActive(true);
            }

        }
    }

    private void RemoveEnemy(object sender, EnemyTr args)
    {
        if(targetGroup.FindMember(args.tr) != -1)
        {
            targetGroup.RemoveMember(args.tr); //quando un nemico viene distrutto, lo elimino
            enemyIndex++;
        }

    }
    public int GetEnemyIndex()
    {
        return enemyIndex;
    }

    private void OnDrawGizmos() //Test per vedere l'area del lock di Ciro
    {
        Gizmos.DrawCube(transform.position, new Vector3(10, 20, 2));
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDestroyed -= RemoveEnemy;
    }

}
