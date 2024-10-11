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
       /* if(targetGroup.m_Targets[GetEnemyIndex()].target != null && Vector3.Distance(transform.position, targetGroup.m_Targets[GetEnemyIndex()].target.position) > 10f)//valore arbitrario
        {

        }*/ //CONDIZIONE DA IMPLEMENTARE per far si che quando sei troppo distante per il lock
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(!lockOn)
                 enemyNumber = Physics.BoxCastNonAlloc(transform.position, new Vector3(20, 10, 2), transform.forward, enemyArray, Quaternion.identity, 40f, lockOnMask, QueryTriggerInteraction.Collide);
            Debug.Log(enemyNumber);

            if(enemyNumber == 0) //cambiato con enemyArray.lenght
            {
                Debug.Log("Nessun nemico trovato");
            }
            else
            {
                foreach(RaycastHit enemy in enemyArray)
                {
                    if(targetGroup.FindMember(enemy.transform) == -1)//condizione del metodo, aggiungo solo se non sono già presenti
                    {
                        targetGroup.AddMember(enemy.transform, 1.5f, 2f);
                    }

                }
            }
            
            if (lockOnSwitcher && targetGroup.m_Targets.Length >1)
            {
                for (int i = 1; i < targetGroup.m_Targets.Length; i++) //pulizia dai transform nulli
                {
                    if (targetGroup.m_Targets[i].target == null)
                        targetGroup.RemoveMember(targetGroup.m_Targets[i].target);
                }
                Debug.Log("cambiocameraLock");
                playerFreeLookCam.Priority = 10;
                lockOnCam.Priority = 11;
                lockOnSwitcher = false;
                lockOn = true;
                targetGroup.m_Targets[GetEnemyIndex()].target.Find("LockOnEmpty/" + lockOnName).gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("cambiocameraFree");
                for (int i = 1; i < targetGroup.m_Targets.Length; i++)
                {
                    if (targetGroup.m_Targets[i].target != null)
                        targetGroup.m_Targets[i].target.Find("LockOnEmpty/" + lockOnName).gameObject.SetActive(false); //spengo a tutti l'indicatore
                    targetGroup.RemoveMember(targetGroup.m_Targets[i].target);
                }

                playerFreeLookCam.Priority = 11;
                lockOnCam.Priority = 10;
                lockOnSwitcher = true;
                lockOn = false;

                if (targetGroup.m_Targets.Length >1) //spengo l'indicatore del nemico corrente
                {
                    targetGroup.m_Targets[GetEnemyIndex()].target.Find("LockOnEmpty/" + lockOnName).gameObject.SetActive(false);
                }

                enemyIndex = 1;
            }

        }

        if (lockOn) //QUESTA LOGICA da controllare
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
                    targetGroup.m_Targets[GetEnemyIndex()].target.Find("LockOnEmpty/" + lockOnName).gameObject.SetActive(false);
                    enemyIndex++;
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
                targetGroup.m_Targets[GetEnemyIndex()].target.Find("LockOnEmpty/" + lockOnName).gameObject.SetActive(true);
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
