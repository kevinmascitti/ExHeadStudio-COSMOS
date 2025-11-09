using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Rendering;

public class LockOnCamSwitcher : MonoBehaviour
{
    // Attention: renaming objects might break the script functionality
    // All objects that can be locked on must have a rigidbody with discrete collisions, I created an additional tag "sceneObj" to filter
    // objects that get added to the list

    [SerializeField] CinemachineFreeLook playerFreeLookCam;
    [SerializeField] CinemachineFreeLook lockOnCam;
    [SerializeField] CinemachineTargetGroup targetGroup;
    [Tooltip("Insert the object used as cross-hair for locked objects")]
    [SerializeField] GameObject lockOnEmpty;
    [SerializeField] GameObject lockOnIcon;
    [SerializeField] private LayerMask lockOnMask;
    [Tooltip("The vector defines the dimensions of the raycast that detects enemies")]
    [SerializeField] private Vector3 lockOnDimensions = new Vector3(20, 10, 2);
    [SerializeField] private float yOffset = 5f;
    [SerializeField] private float lockOnRange = 40f;
    [SerializeField] private float maxTargetDistance = 20f;

    private RaycastHit[] enemyArray;

    private string lockOnObjName; // This is used to identify objects in the hierarchy
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
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            if(!lockOn) // Modified the direction, previously it was only transform.forward and then I added an arbitrary y offset
            {
                enemyNumber = Physics.BoxCastNonAlloc(transform.position + new Vector3(0f, yOffset, 0f), lockOnDimensions, Camera.main.transform.forward, enemyArray, Quaternion.identity, lockOnRange, lockOnMask, QueryTriggerInteraction.Collide); // The last parameter allows using trigger colliders
            }
                
            if(enemyNumber >=1)
            {
                foreach(RaycastHit enemy in enemyArray)
                {
                    /*if(targetGroup.FindMember(enemy.transform) == -1 && enemy.transform.gameObject.CompareTag("sceneObj"))// Method condition, add only if not already present
                    {
                        targetGroup.AddMember(enemy.transform, 2f, 1f);
                    }
                    else */
                    if(targetGroup.FindMember(enemy.transform) == -1)
                    {
                        targetGroup.AddMember(enemy.transform, 0.5f, 0.1f);
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

            if(targetGroup.m_Targets.Length >1 && targetGroup.m_Targets[enemyIndex].target == null)
            {
                enemyIndex++;
                targetGroup.RemoveMember(targetGroup.m_Targets[enemyIndex-1].target);
                if (enemyIndex >= targetGroup.m_Targets.Length)
                    enemyIndex = 1;
            }

            if (targetGroup.m_Targets.Length == 1 || Vector3.Distance(targetGroup.m_Targets[enemyIndex].target.position, transform.position) >= maxTargetDistance)
            {
                if(targetGroup.m_Targets.Length >1 && targetGroup.m_Targets[enemyIndex].target == null)
                    targetGroup.RemoveMember(targetGroup.m_Targets[enemyIndex].target);
                else if(targetGroup.m_Targets.Length > 1)
                    targetGroup.m_Targets[enemyIndex].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(false);
                ChangeToFree();
                //return;
            }

            // In the new version, the mouse wheel activates the lock, rotation moves the pointer forward

            Debug.Log("Mouse wheel: " + Input.mouseScrollDelta);
            if (Input.mouseScrollDelta.y > 0.01f)///(Input.GetKeyDown(KeyCode.Mouse2)) // MIDDLE MOUSE BUTTON
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

                targetGroup.m_Targets[enemyIndex].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(true);
                transform.rotation = Quaternion.LookRotation(targetGroup.m_Targets[enemyIndex].target.position - transform.position, Vector3.up);
            }
        }
    }

    private void ChangeToFree()
    {
        /*for (int i = 1; i < targetGroup.m_Targets.Length; i++)
        {
            if (targetGroup.m_Targets[i].target != null)
                targetGroup.m_Targets[i].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(false); // Turn off the indicator for everyone

            targetGroup.RemoveMember(targetGroup.m_Targets[i].target);
        }*/

        for (int i = targetGroup.m_Targets.Length-1; i > 0; i--)
        {
            if (targetGroup.m_Targets[i].target != null)
                targetGroup.m_Targets[i].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(false); // Turn off the indicator for everyone

            targetGroup.RemoveMember(targetGroup.m_Targets[i].target);
        }

        playerFreeLookCam.Priority = 11;
        lockOnCam.Priority = 10;

        lockOnSwitcher = true;
        lockOn = false;

        if (targetGroup.m_Targets.Length > 1 && targetGroup.m_Targets[enemyIndex].target != null) // Turn off the current enemy's indicator
        {
            targetGroup.m_Targets[enemyIndex].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(false);
        }

        enemyIndex = 1;
    }

    private void ChangeToLock()
    {
        for (int i = 1; i < targetGroup.m_Targets.Length; i++) // Clean up null transforms
        {
            if (targetGroup.m_Targets[i].target == null)
                targetGroup.RemoveMember(targetGroup.m_Targets[i].target);
        }

        var playerPos = transform.position;
        for (int i = 1; i < targetGroup.m_Targets.Length - 1; i++)
        {
            int imin = i;
            for (int j = i + 1; j < targetGroup.m_Targets.Length; j++)
            {
                if (Vector3.Distance(targetGroup.m_Targets[j].target.position, playerPos) < Vector3.Distance(targetGroup.m_Targets[imin].target.position, playerPos))
                {
                    imin = j;
                }
            }

            var temp = targetGroup.m_Targets[i];
            targetGroup.m_Targets[i] = targetGroup.m_Targets[imin];
            targetGroup.m_Targets[imin] = temp;
        }

        playerFreeLookCam.Priority = 10;
        lockOnCam.Priority = 11;

        lockOnSwitcher = false;
        lockOn = true;
        if(enemyIndex < targetGroup.m_Targets.Length)
         targetGroup.m_Targets[enemyIndex].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(true);
    }

    private void RemoveEnemy(object sender, EnemyTr args)
    {
        if(targetGroup.FindMember(args.tr) != -1)
        {
            if (targetGroup.FindMember(args.tr) == enemyIndex)
            {
                targetGroup.m_Targets[enemyIndex].target.Find(lockOnEmptyName + "/" + lockOnObjName).gameObject.SetActive(false);
                enemyIndex++;
            }
            targetGroup.RemoveMember(args.tr); // When an enemy is destroyed, remove it
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

    private void OnDrawGizmos() // Test to visualize Ciro's lock area
    {
        Gizmos.DrawCube(transform.position + new Vector3(0f, yOffset, 0f), 2*lockOnDimensions);
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDestroyed -= RemoveEnemy;
    }
}
