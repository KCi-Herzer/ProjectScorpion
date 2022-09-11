using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamageable
{
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [SerializeField] int HP;
    [SerializeField] float playerSpeed;
    [SerializeField] float gravityValue;
    [SerializeField] float jumpHeight;
    [SerializeField] int jumpsMax;


    [SerializeField] int ammoCap;
    [SerializeField] int shootdist;

    [SerializeField] float shootrate;
    [SerializeField] int shootDamage;
    [SerializeField] int Auto;


    [Header("-----UI Settings-----")]
    [SerializeField] float playerDamageFlashTime;


    [SerializeField] List<Gun> gunStats = new List<Gun>();
    [SerializeField] GameObject gunModel;

    int HPOrig;
    int timesJumped;
    private Vector3 playerVelocity;
    Vector3 move;
    bool isShooting;
    int selectedGun;



    private void Start()
    {
        HPOrig = HP;
        respawn();
        updateAmmoUI();
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            movement();
            gunSelect();

            
            StartCoroutine(shoot());
        }
    }

    void movement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            timesJumped = 0;
        }

        move = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && timesJumped < jumpsMax)
        {
            playerVelocity.y = jumpHeight;
            timesJumped++;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }


    public void gunPickup(Gun stats)
    {
        shootrate = stats.shootrate;
        shootDamage = stats.shootDamage;
        shootdist = stats.shootdist;
        ammoCap = stats.ammoCap;
        

        updateAmmoUI();

        gunModel.GetComponent<MeshFilter>().sharedMesh = stats.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = stats.model.GetComponent<MeshRenderer>().sharedMaterial;

        gunStats.Add(stats);
    }

    void gunSelect()
    {
        if(gunStats.Count > 1)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunStats.Count - 1)
            {
                selectedGun++;
                shootrate = gunStats[selectedGun].shootrate;
                shootdist = gunStats[selectedGun].shootdist;
                shootDamage = gunStats[selectedGun].shootDamage;
                ammoCap = gunStats[selectedGun].ammoCap;
                Auto = gunStats[selectedGun].Auto;


                updateAmmoUI();

                gunModel.GetComponent<MeshFilter>().sharedMesh = gunStats[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
                gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStats[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
            {
                selectedGun--;
                shootrate = gunStats[selectedGun].shootrate;
                shootdist = gunStats[selectedGun].shootdist;
                shootDamage = gunStats[selectedGun].shootDamage;
                ammoCap = gunStats[selectedGun].ammoCap;
                Auto = gunStats[selectedGun].Auto;
                
                
                updateAmmoUI();

                gunModel.GetComponent<MeshFilter>().sharedMesh = gunStats[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
                gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStats[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
            }
        }
    }

    IEnumerator shoot()
    {
            
        if (gunStats.Count >= 1 && !isShooting && gunStats[selectedGun].Auto == 0 && Input.GetButtonDown("Shoot") && ammoCap > 0)
        {
            shooting();
        }
        else if(gunStats.Count >= 1 && !isShooting && gunStats[selectedGun].Auto == 1 && Input.GetButtonDown("Shoot") && ammoCap > 0)
        {
            shooting();
        }

        yield return new WaitForSeconds(shootrate);
        isShooting = false;

    }

    void shooting()
    {
        isShooting = true;
        ammoCap--;
        updateAmmoUI();

        RaycastHit hit;
        //Debug.Log("Shooting");
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootdist))
        {
            //if(hit.transform.CompareTag("Cube"))
            //Debug.Log("Casted");

            if (hit.collider.GetComponent<IDamageable>() != null)
            {
                //Debug.Log("Connected");
                hit.collider.GetComponent<IDamageable>().takeDamage(shootDamage);
            }
        }
        //Instantiate(Cube, transform.position, Cube.transform.rotation);
        
    }

    public void updateAmmoUI()
    {
        gameManager.instance.ammoCounter.text = ammoCap.ToString("F0");
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        updatePlayerHP();

        StartCoroutine(damageFlash());

        if (HP <= 0)
        {
            gameManager.instance.playerIsDead();
        }
    }

    IEnumerator damageFlash()
    {
        gameManager.instance.playerDamage.SetActive(true);
        yield return new WaitForSeconds(playerDamageFlashTime);
        gameManager.instance.playerDamage.SetActive(false);
    }

    public void respawn()
    {
        controller.enabled = false;
        HP = HPOrig;
        updatePlayerHP();
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        gameManager.instance.cursorUnlockUnpause();
        gameManager.instance.isPaused = false;
        controller.enabled = true;
    }

    public void updatePlayerHP()
    {
        gameManager.instance.HPBar.fillAmount = (float)HP / (float)HPOrig;
    }
}