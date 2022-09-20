using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    //[SerializeField] int currentEnemies;
    [SerializeField] int maxEnemies;
    [SerializeField] int timer;

    int enemiesSpawned;
    bool isSpawned;
    bool startSpawning;
    bool hasReachedMax;


    private void Start()
    {
        //gameManager.instance.enemyIncrement(gameManager.instance.enemyCount);
        //gameManager.instance.countCurrentEnemies(maxEnemies);
    }

    private void Update()
    {
        if(startSpawning && gameManager.instance.enemyCount <= maxEnemies && hasReachedMax != true)
        {
            StartCoroutine(spawn());
        }
        else if(gameManager.instance.enemyCount >= maxEnemies)
        {
            hasReachedMax = true;
        }
    }

    IEnumerator spawn()
    {
        if (!isSpawned && gameManager.instance.enemyCount < maxEnemies)
        {
            isSpawned = true;
            gameManager.instance.enemyCount++;

            Instantiate(enemy, transform.position, enemy.transform.rotation);
            gameManager.instance.enemyCounter.text = gameManager.instance.enemyCount.ToString("F0");

            yield return new WaitForSeconds(timer);

            isSpawned = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }
}
