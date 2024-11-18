using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    Collider spawnTrigger;
    [SerializeField] private int maxEnemiesNumber;
    [SerializeField] GameObject meleeEnemy;
    [SerializeField] GameObject rangedEnemy;
    [SerializeField] List<Transform> spawnList = new List<Transform>();
    private int spawnPos = 0;
    [Tooltip("0 = nemico melee\n 1 = nemico ranged")]
    [SerializeField] private int tipoNemico;
    [Tooltip("false = calcolo random\n true = crea solo nemici dati da tipoNemico")]
    [SerializeField] bool decideLuca;
    [SerializeField] private AIArea aiArea;
    
    private void Awake()
    {
        spawnTrigger = GetComponent<Collider>();
        aiArea = GetComponentInParent<AIArea>();
        maxEnemiesNumber = spawnList.Count;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && aiArea.enemyList.Count < maxEnemiesNumber && aiArea.canSpawnAgain)
        {
            aiArea.canSpawnAgain = false;
            aiArea.spawning = true;
            int currentEnemiesN = aiArea.enemyList.Count;
            for (int i = 0; i < maxEnemiesNumber - currentEnemiesN; i++)
            {
                //Debug.Log("Spawn");
                float enemyType;
                if (!decideLuca) enemyType = Random.value;
                else enemyType = tipoNemico;
                GameObject nextSpawningEnemy;
                if (enemyType < .5f) nextSpawningEnemy = meleeEnemy;
                else nextSpawningEnemy = rangedEnemy;
                //int position = (int)Random.Range(0f, spawnList.Count - 0.1f);
                var enemy = Instantiate(nextSpawningEnemy, spawnList[i].position, spawnList[i].rotation);
                //enemy.GetComponent<Enemy>().SetID(++idControl);
                aiArea.enemyList.Add(enemy.GetInstanceID(), enemy);
                enemy.GetComponent<StateController>().SetAreaOfAction(aiArea);
                Debug.Log(aiArea.enemyList[enemy.GetInstanceID()].GetInstanceID());
               
            }
            
            StartCoroutine(WaitForIsSpawning());
            
        }
    }
    IEnumerator WaitForIsSpawning()
    {
        yield return new WaitForSeconds(2f);
    }
    private void OnTriggerExit(Collider other)
    {
       
    }
}
