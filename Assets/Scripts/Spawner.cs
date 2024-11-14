using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : AIArea
{
    [SerializeField]private int maxEnemiesNumber;
    [SerializeField] GameObject meleeEnemy;
    [SerializeField] GameObject rangedEnemy;
    [SerializeField] List<Transform> spawnList= new List<Transform>();
    private int spawnPos=0;
    [Tooltip("0 = nemico melee\n 1 = nemico ranged")]
    [SerializeField] private int tipoNemico;
    [Tooltip("false = calcolo random\n true = crea solo nemici dati da tipoNemico")]
    [SerializeField] bool decideLuca;
    private void Awake()
    {
        base.Awake();
        maxEnemiesNumber = spawnList.Count;
    }
    
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && enemyList.Count < maxEnemiesNumber)
        {
            int currentEnemiesN=enemyList.Count;
            for(int i = 0; i < maxEnemiesNumber-currentEnemiesN; i++)
            {
                Debug.Log("Spawn");
                float enemyType;
                if (!decideLuca) enemyType = Random.value;
                else enemyType = tipoNemico;
                GameObject nextSpawningEnemy;
                if (enemyType < .5f) nextSpawningEnemy = meleeEnemy;
                else nextSpawningEnemy = rangedEnemy;
                int position = (int)  Random.Range(0f, spawnList.Count-0.1f);
                var enemy = Instantiate(nextSpawningEnemy, spawnList[spawnPos].position, spawnList[spawnPos].rotation );
                //enemy.GetComponent<Enemy>().SetID(++idControl);
                enemyList.Add(enemy.GetComponent<Enemy>().GetInstanceID(), enemy);
                enemy.GetComponent<StateController>().SetAreaOfAction(this);
                spawnPos++;
            }
            spawnPos = 0;
            if (!isPlayerInside)
            {
                isPlayerInside = true;

                OnPlayerEnter?.Invoke(this, new OnPlayerArg(areaID));
            }
            
            //count = enemyList.Count;
            return;
        }
        if(other.gameObject.tag.Equals("Enemy") || other.gameObject.tag.Equals("ShootingEnemy") && !enemyList.ContainsKey(other.gameObject.GetComponent<Enemy>().GetInstanceID()))
        {
            other.GetComponent<StateController>().SetAreaOfAction(this);
            enemyList.Add(other.gameObject.GetInstanceID(), other.gameObject);
        }
        //All'inizio del gioco, salvo in ogni area i nemici all'interno e in caso il player
        else 
        {
            if (!isPlayerInside)
            {
                isPlayerInside = true;

                OnPlayerEnter?.Invoke(this, new OnPlayerArg(areaID));
            }
        }
    }
}
