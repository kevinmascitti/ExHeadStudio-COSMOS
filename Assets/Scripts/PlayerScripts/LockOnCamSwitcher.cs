using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Rendering;

public class LockOnCamSwitcher : MonoBehaviour
{
    // Occhio a rinominare gli oggetti che altrimenti lo script non funziona
    // tutti gli oggetti che possono avere il lock devono avere un rigidbody con collisioni discrete
    [SerializeField] CinemachineFreeLook playerFreeLookCam;
    [SerializeField] CinemachineFreeLook lockOnCam;
    [SerializeField] CinemachineTargetGroup targetGroup;
    [Tooltip("Inserire l'oggetto che viene usato come cross-hair per gli oggeti lockati")]
    [SerializeField] GameObject lockOnEmpty;
    [SerializeField] GameObject lockOnIcon;
    [SerializeField] private LayerMask lockOnMask;
    [Tooltip("Il vettore serve a dare le dimensioni del raycast che rileva i nemici")]
    [SerializeField] private Vector3 lockOnDimensions = new Vector3(20, 10, 2);
    [SerializeField] private float lockOnRange = 40f;
    [SerializeField] private float maxTargetDistance = 20f;



    private RaycastHit[] enemyArray;

    private string lockOnObjName; //questa serve per identificare gli oggetti nella hierarchy
    private string lockOnEmptyName;

    private bool lockOnSwitcher = true;
    public bool lockOn = false;
    private int enemyIndex = 1;
    private int enemyNumber = 0;

    void Start()
    {

        lockOnObjName = lockOnIcon.name;
        lockOnEmptyName = lockOnEmpty.name;
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
                 enemyNumber = Physics.BoxCastNonAlloc(transform.position, lockOnDimensions, transform.forward, enemyArray, Quaternion.identity, lockOnRange, lockOnMask, QueryTriggerInteraction.Collide); //l'ultimo parametro permette di usare dei collider trigger

            if(enemyNumber >=1)
            {
                foreach(RaycastHit enemy in enemyArray)
                {
                    if(targetGroup.FindMember(enemy.transform) == -1)//condizione del metodo, aggiungo solo se non sono già presenti
                    {
                        targetGroup.AddMember(enemy.transform, 2f, 2f);
                    }

                }
            }

            if (lockOnSwitcher && targetGroup.m_Targets.Length > 1)
            {
                ChangeToLock();
            }
            else
            {
                ChangeToFree();
            }

        }

        if (lockOn)
        {

            if (enemyIndex >= targetGroup.m_Targets.Length)
                enemyIndex = 1;

            if (targetGroup.m_Targets.Length == 1 || Vector3.Distance(targetGroup.m_Targets[enemyIndex].target.position, transform.position) >= maxTargetDistance)
            {
                if(targetGroup.m_Targets.Length >1 && targetGroup.m_Targets[enemyIndex].target == null)
                    targetGroup.RemoveMember(targetGroup.m_Targets[enemyIndex].target);
                else if(targetGroup.m_Targets.Length > 1)
                    targetGroup.m_Targets[enemyIndex].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(false);
                ChangeToFree();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse2)) //TASTO CENTRALE DEL MOUSE
            {
                if (targetGroup.m_Targets[enemyIndex].target != null)
                    targetGroup.m_Targets[enemyIndex].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(false);
                else
                    targetGroup.RemoveMember(targetGroup.m_Targets[enemyIndex].target);


                if (enemyIndex < targetGroup.m_Targets.Length)
                {
                    enemyIndex++;
                }
                else
                {
                    enemyIndex = 1;
                }

                if (enemyIndex < targetGroup.m_Targets.Length && targetGroup.m_Targets[enemyIndex].target == null)
                {
                    targetGroup.RemoveMember(targetGroup.m_Targets[enemyIndex].target);
                    enemyIndex++;
                }

                if (enemyIndex >= targetGroup.m_Targets.Length)
                    enemyIndex = 1;

                targetGroup.m_Targets[GetEnemyIndex()].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(true);


            }
        }


    }


    private void ChangeToFree()
    {
        for (int i = 1; i < targetGroup.m_Targets.Length; i++)
        {
            if (targetGroup.m_Targets[i].target != null)
                targetGroup.m_Targets[i].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(false); //spengo a tutti l'indicatore

            targetGroup.RemoveMember(targetGroup.m_Targets[i].target);
        }

        playerFreeLookCam.Priority = 11;
        lockOnCam.Priority = 10;

        lockOnSwitcher = true;
        lockOn = false;

        if (targetGroup.m_Targets.Length > 1 && targetGroup.m_Targets[enemyIndex].target != null) //spengo l'indicatore del nemico corrente
        {
            targetGroup.m_Targets[enemyIndex].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(false);
        }

        enemyIndex = 1;
    }

    private void ChangeToLock()
    {
        for (int i = 1; i < targetGroup.m_Targets.Length; i++) //pulizia dai transform nulli
        {
            if (targetGroup.m_Targets[i].target == null)
                targetGroup.RemoveMember(targetGroup.m_Targets[i].target);
        }

        playerFreeLookCam.Priority = 10;
        lockOnCam.Priority = 11;

        lockOnSwitcher = false;
        lockOn = true;
        targetGroup.m_Targets[enemyIndex].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(true);
        //targetGroup.m_Targets[enemyIndex].target.Find(lockOnObjName).gameObject.SetActive(true);
    }
    private void RemoveEnemy(object sender, EnemyTr args)
    {
        if(targetGroup.FindMember(args.tr) != -1)
        {
            if(targetGroup.FindMember(args.tr) == enemyIndex)
            {
                enemyIndex++;
            }
            targetGroup.RemoveMember(args.tr); //quando un nemico viene distrutto, lo elimino

        }
        if (enemyIndex >= targetGroup.m_Targets.Length)
            enemyIndex = 1;
        if(targetGroup.m_Targets.Length >1)
            targetGroup.m_Targets[enemyIndex].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(true);


    }
    public int GetEnemyIndex()
    {
        return enemyIndex;
    }

    public Transform GetCurrentEnemyTr()
    {
        return targetGroup.m_Targets[GetEnemyIndex()].target;
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
