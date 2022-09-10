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
    [Range(0, 100)] [SerializeField] int HP;
    [Range(1, 10)] [SerializeField] int playerFaceSpeed;

    [Header("----- Weapon Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    Vector3 playerDir;
    bool isShooting;
    bool playerInRange;
    Vector3 lastPlayerPos;
    float stoppingDistOrig;

    private void Start()
    {
        gameManager.instance.enemyIncrement();
        lastPlayerPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    void Update()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position;

        if (playerInRange)
        {   
            canSeePlayer();
        }
        else
        {
            agent.SetDestination(lastPlayerPos);
            agent.stoppingDistance = 0;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            lastPlayerPos = gameManager.instance.player.transform.position;
            agent.stoppingDistance = 0;
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
        lastPlayerPos = gameManager.instance.player.transform.position;


        if (HP <= 0)
        {
            gameManager.instance.enemyDecrement();
            Destroy(gameObject);
        }
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

    void canSeePlayer()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, playerDir, out hit))
        {
            Debug.DrawRay(transform.position, playerDir);
            if(hit.collider.CompareTag("Player"))
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
                agent.stoppingDistance = stoppingDistOrig;

                if(agent.stoppingDistance <= agent.remainingDistance)
                    facePlayer();

                if (!isShooting)
                    StartCoroutine(shoot());
            }
        }
    }
}
