using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveManager : MonoBehaviour
{
    public static waveManager instance;

    [Header("-----Enemies------")]
    [SerializeField] GameObject meleeEnemy;
    [SerializeField] GameObject rangeEnemy;
    //[SerializeField] List<GameObject> enemyTypes; Can possibly change to list, not going to now though
    [SerializeField] GameObject bossEnemy;
    [Header("-----Initial Spawn Count-----")]
    [SerializeField] int meleeEnemyInitial;
    [SerializeField] int rangeEnemyInitial;
    [Header("-----Spawn Frequency-----")]
    [Range(0, 1)] [SerializeField] double meleeEnemyFreq; //The precentage multiplyer of enemies as the waves progress 0.1 is my default
    [Range(0, 1)] [SerializeField] double rangeEnemyFreq;
    [Range(1, 20)] [SerializeField] int bossSpawnInterval; //What rounds do the bosses spawn on
    [SerializeField] int spawnTimer;

    [Header("-----Not used in editor-----")]
    public int currentWave;

    List<enemySpawner> spawners = new List<enemySpawner>();

    ////-----Current Enemy Count-----
    //int meleeEnemyCount;
    //int rangeEnemyCount;
    //int bossEnemyCount;
    //-----Max Enemy PerWave-----
    int meleeEnemyMax;
    int rangeEnemyMax;
    int bossEnemyMax;
    //-----Enemies Spawned-----
    int meleeEnemiesSpawned;
    int rangeEnemiesSpawned;
    int bossEnemiesSpawned;

    bool isSpawning;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        enemySpawner[] spns = FindObjectsOfType<enemySpawner>();
        foreach (enemySpawner item in spns)
        {
            spawners.Add(item);
        }
        NextWave(); //Starts wave 1
    }

    private void Update()
    {
        if (meleeEnemiesSpawned < meleeEnemyMax)
        {
            StartCoroutine(spawnMelee());
        }
        if (rangeEnemiesSpawned < rangeEnemyMax)
        {
            StartCoroutine(spawnRanged());
        }
        if (bossEnemiesSpawned < bossEnemyMax)
        {
            StartCoroutine(spawnBoss());
        }
    }

    public void NextWave()
    {
        meleeEnemiesSpawned = 0;
        rangeEnemiesSpawned = 0;
        bossEnemiesSpawned = 0;
        currentWave++;
        gameManager.instance.waveCounter.text = currentWave.ToString("F0");
        //Calculate how many enemies will be in the wave
        meleeEnemyMax = (int)((1 + (currentWave - 1) * meleeEnemyFreq) * meleeEnemyInitial);
        rangeEnemyMax = (int)((1 + (currentWave - 1) * rangeEnemyFreq) * rangeEnemyInitial);
        if (currentWave % bossSpawnInterval == 0)
        {
            bossEnemyMax++; //Only one boss will spawn per wave, can change later, put on extended backlog for now
        }
        gameManager.instance.enemyCount = (meleeEnemyMax + rangeEnemyMax + bossEnemyMax); //Sends the amount of enemies for the round to the gameManager
        gameManager.instance.enemyCounter.text = gameManager.instance.enemyCount.ToString("F0");

    }

    //I'm sure theres a way to make the IEnumerators below combine to one but I haven't gotten around to doing this yet either

    IEnumerator spawnMelee()
    {
        //Pick a random spawn
        enemySpawner selectedSpawn = spawners[Random.Range(0, spawners.Count)];
        if (!selectedSpawn.isSpawning && meleeEnemiesSpawned < meleeEnemyMax && !selectedSpawn.rangedOnly) //Checks to see if spawner is currently in use
        {
            selectedSpawn.isSpawning = true;
            //gameManager.instance.enemyCount++;

            Instantiate(meleeEnemy, selectedSpawn.transform.position, meleeEnemy.transform.rotation);
            //gameManager.instance.enemyCounter.text = gameManager.instance.enemyCount.ToString("F0");
            meleeEnemiesSpawned++; //Increments the count
            yield return new WaitForSeconds(spawnTimer);
            //Debug.Log("Melee");
            selectedSpawn.isSpawning = false;
        }
    }

    IEnumerator spawnRanged()
    {
        
        //Pick a random spawn
        enemySpawner selectedSpawn = spawners[Random.Range(0, spawners.Count)];
        if (!selectedSpawn.isSpawning && rangeEnemiesSpawned < rangeEnemyMax && selectedSpawn.rangedOnly) //Checks to see if spawner is currently in use
        {
            selectedSpawn.isSpawning = true;
            //gameManager.instance.enemyCount++;

            Instantiate(rangeEnemy, selectedSpawn.transform.position, rangeEnemy.transform.rotation);
            //gameManager.instance.enemyCounter.text = gameManager.instance.enemyCount.ToString("F0");
            rangeEnemiesSpawned++; //Increments the count
            yield return new WaitForSeconds(spawnTimer);
            //Debug.Log("Ranged");
            selectedSpawn.isSpawning = false;
        }
    }

    IEnumerator spawnBoss()
    {
        
        //Pick a random spawn
        enemySpawner selectedSpawn = spawners[Random.Range(0, spawners.Count)];
        if (!selectedSpawn.isSpawning && bossEnemiesSpawned < bossEnemyMax && selectedSpawn.bossOK) //Checks to see if spawner is currently in use
        {
            selectedSpawn.isSpawning = true;
            //gameManager.instance.enemyCount++;

            Instantiate(bossEnemy, selectedSpawn.transform.position, bossEnemy.transform.rotation);
            //gameManager.instance.enemyCounter.text = gameManager.instance.enemyCount.ToString("F0");
            bossEnemiesSpawned++; //Increments the count
            yield return new WaitForSeconds(spawnTimer);
            //Debug.Log("Boss");
            selectedSpawn.isSpawning = false;
        }
    }
}
