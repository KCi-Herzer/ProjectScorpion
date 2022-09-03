using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamageable
{
    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer rend;

    [Header("----- Enemy Stats -----")]
    [Range(0, 10)] [SerializeField] int HP;
    [Range(1, 10)] [SerializeField] int playerFaceSpeed;

    [Header("----- Weapon Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    Vector3 playerDir;
    bool isShooting;

    void Update()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position;

        agent.SetDestination(gameManager.instance.player.transform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isShooting)
                StartCoroutine(shoot());
            facePlayer();

        }
    }

    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        StartCoroutine(flashColor());

        if (HP <= 0)
            Destroy(gameObject);
    }

    IEnumerator flashColor()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = Color.white;
    }

    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
