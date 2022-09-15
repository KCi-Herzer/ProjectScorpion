using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] int maxEnemies;
    [SerializeField] int timer;

    int enemiesSpawned;
    bool isSpawned;
    bool startSpawning;


    private void Start()
    {
        gameManager.instance.enemyIncrement(maxEnemies);
    }

    private void Update()
    {
        if(startSpawning)
        {
            StartCoroutine(spawn());
        }
    }

    IEnumerator spawn()
    {
        if (!isSpawned && enemiesSpawned < maxEnemies)
        {
            isSpawned = true;
            enemiesSpawned++;

            Instantiate(enemy, transform.position, enemy.transform.rotation);

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
