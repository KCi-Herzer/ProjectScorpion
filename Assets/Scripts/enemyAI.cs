using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamageable
{
    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer rend;
    [SerializeField] Animator anim;

    [Header("----- Enemy Stats -----")]
    [Range(0, 100)] [SerializeField] int HP;
    [Range(1, 10)] [SerializeField] float speedRoam;
    [Range(1, 10)] [SerializeField] float speedChase;
    [Range(1, 10)] [SerializeField] int playerFaceSpeed;
    [Range(1, 50)] [SerializeField] int roamRadius;
    [Range(1, 180)] [SerializeField] int viewAngle;
    [SerializeField] GameObject headPosition;


    [Header("----- Weapon Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    Vector3 playerDir;
    bool takingDamage;
    bool isShooting;
    bool playerInRange;
    Vector3 lastPlayerPos;
    float stoppingDistOrig;
    bool hasSeen; //Made this to see where the player was when breaking LOS
    Vector3 startingPos;
    int speedOrig;
    bool roamPathValid;
    float angle;
    bool isDmg;

    private void Start()
    {
        //gameManager.instance.enemyIncrement();
        lastPlayerPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedRoam = agent.speed;
        //speedOrig = agent.speed;
        startingPos = transform.position;
    }

    void Update()
    {
        if (agent.enabled)
        {
            playerDir = gameManager.instance.player.transform.position - transform.position;
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * 4));

            if (!takingDamage)
            {
                if (playerInRange)
                {
                    //if(angle > viewAngle && agent.stoppingDistance != 0) - Student's code from class
                    //facePlayer();

                    rayToPlayer();

                }

                if (agent.remainingDistance < 0.001f && agent.destination != gameManager.instance.player.transform.position)
                {
                    roam();
                    //agent.SetDestination(lastPlayerPos);
                    //agent.stoppingDistance = 0;
                }
            }
        }
    }

    void roam()
    {
        agent.stoppingDistance = 0;
        agent.speed = speedRoam;

        Vector3 randomDir = Random.insideUnitSphere * roamRadius;
        randomDir += startingPos;
        

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, 1, 1);
        NavMeshPath path = new NavMeshPath();
        
        agent.CalculatePath(hit.position, path);
        agent.SetPath(path);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

     void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            lastPlayerPos = gameManager.instance.player.transform.position;
            agent.stoppingDistance = 0;
            hasSeen = false;
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
        anim.SetTrigger("Damage");
        agent.speed = 0;

        StartCoroutine(flashColor());
        lastPlayerPos = gameManager.instance.player.transform.position;

        if(HP <= 0 && agent.enabled)
        {
            enemyDead();
        }
        

        if (!playerInRange)
        {
            agent.SetDestination(lastPlayerPos);
            //agent.SetPath();
        }
        
    }

    IEnumerator flashColor()
    {
        takingDamage = true;
        rend.material.color = Color.red;
        agent.speed = 0;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = Color.white;
        agent.speed = speedRoam;
        takingDamage = false;
    }

    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void rayToPlayer()
    {
        float angle = Vector3.Angle(playerDir, transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up, playerDir, out hit))
        {
            Debug.DrawRay(transform.position + transform.up, playerDir);

            if (hit.collider.CompareTag("Player") && angle <= viewAngle)
            {
                hasSeen = true;
                lastPlayerPos = gameManager.instance.player.transform.position;

                agent.SetDestination(gameManager.instance.player.transform.position);
                agent.stoppingDistance = stoppingDistOrig;

                facePlayer();
                //if (agent.stoppingDistance >= agent.remainingDistance) //Changed <= to >=

                if (!isShooting)
                    StartCoroutine(shoot());
            }
            else if (hasSeen == true)
            {
                //If the enemy has seen the player, they will follow after
                //Just like exiting the range, but instead exiting sight
                agent.SetDestination(lastPlayerPos);
                agent.stoppingDistance = 0;
            }
            else
            {
                hasSeen = false;
                //roam();
            }
        }
    }

    public void playerDied()
    {
        //Added to fix bug: OnCollisionExit not being called when player dies
        
        playerInRange = false;
        agent.stoppingDistance = 0;
        roam();
    }

    void enemyDead()
    {
         gameManager.instance.enemyDecrement();

        anim.SetBool("Dead", true);
        agent.enabled = false;

        //foreach (Collider col in GetComponent<Collider>())
            //col.enabled = false;

        
    }
}
