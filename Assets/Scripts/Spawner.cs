using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : AIArea
{
    [SerializeField] private int maxEnemiesNumber;
    [SerializeField] GameObject meleeEnemy;
    [SerializeField] GameObject rangedEnemy;
    [SerializeField] List<Transform> spawnList= new List<Transform>();
    private int spawnIndex=0;
    [Tooltip("0 = nemico melee\n 1 = nemico ranged")]
    [SerializeField] private int tipoNemico;
    [Tooltip("false = calcolo random\n true = crea solo nemici dati da tipoNemico")]
    [SerializeField] bool decideLuca;
    private int idControl = 0;
    private void Awake()
    {
        base.Awake();
    }
    
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && count < maxEnemiesNumber)
        {
            
            for(spawnIndex = 0; spawnIndex < maxEnemiesNumber; spawnIndex++)
            {
                float enemyType;
                if (!decideLuca) enemyType = Random.value;
                else enemyType = tipoNemico;
                GameObject nextSpawningEnemy;
                if (enemyType < .5f) nextSpawningEnemy = meleeEnemy;
                else nextSpawningEnemy = rangedEnemy;
                var enemy = Instantiate(nextSpawningEnemy, spawnList[spawnIndex].position, spawnList[spawnIndex].rotation );
                //enemy.GetComponent<Enemy>().SetID(++idControl);
                enemyList.Add(enemy.GetComponent<Enemy>().GetInstanceID(), enemy);
                enemy.GetComponent<StateController>().SetAreaOfAction(this);
            }
            spawnIndex = 0;
            count = enemyList.Count;
            return;
        }
        if(other.gameObject.tag.Equals("Enemy") || other.gameObject.tag.Equals("ShootingEnemy") && !enemyList.ContainsKey(other.gameObject.GetComponent<Enemy>().GetID()))
        {
            other.GetComponent<StateController>().SetAreaOfAction(this);
            enemyList.Add(other.gameObject.GetInstanceID(), other.gameObject);
            count=enemyList.Count;
        }
        //All'inizio del gioco, salvo in ogni area i nemici all'interno e in caso il player
        else 
        {
            isPlayerInside = true;
            OnPlayerEnter?.Invoke(this, new OnPlayerArg(areaID));
        }
    }
}
