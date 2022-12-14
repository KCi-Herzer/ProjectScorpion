using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamageable
{
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [SerializeField] int HP;
    [SerializeField] float playerSpeed;
    [SerializeField] float sprintMulti;
    [SerializeField] float gravityValue;
    [SerializeField] float jumpHeight;
    [SerializeField] int jumpsMax;

    [Header("----- Gun Stats -----")]
    [SerializeField] int shootdist;
    [SerializeField] float shootrate;
    [SerializeField] int shootDamage;
    [SerializeField] int Auto; 
    [SerializeField] List<Gun> gunStats = new List<Gun>();
    [SerializeField] GameObject gunModel;
    [SerializeField] List<GameObject> gunInHand;
    
    int currentAmmo;
    
    //int ammoCap; not used

    [Header("----- Audio -----")]

    public AudioSource aud;

    [Range(0, 1)] [SerializeField] float gunShootSoundVol;
    
    [SerializeField] AudioClip[] playerDamage;
    [Range(0, 1)] [SerializeField] float playerDamageVol;

    [SerializeField] AudioClip[] playerJumpSound;
    [Range(0, 1)] [SerializeField] float playerJumpSoundVol;

    [SerializeField] AudioClip[] playerFootstepsSound;
    [Range(0, 1)] [SerializeField] float playerFootstepsSoundVol;

    [SerializeField] AudioClip[] healthPickupSound;
    [Range(0, 1)] [SerializeField] float healthPickupSoundVol;

    [SerializeField] AudioClip[] ammoPickupSound;
    [Range(0, 1)] [SerializeField] float ammoPickupSoundVol;

    [SerializeField] AudioClip[] weaponSwapSound;
    [Range(0, 1)] [SerializeField] float weaponSwapSoundVol;

    public AudioClip[] objDeadSound;
    [Range(0, 1)] public float objDeadSoundVol;

    public AudioClip[] winSound;
    [Range(0, 1)] public float winSoundVol;

    public AudioClip[] uiClickSound;
    [Range(0, 1)] public float uiClickSoundVol;

    [Header("-----UI Settings-----")]
    [SerializeField] float playerDamageFlashTime;

    int HPOrig;
    int timesJumped;
    private Vector3 playerVelocity;
    Vector3 move;
    public bool isShooting;
    int selectedGun;
    public bool hasGun; //Added for ammoPickup
    float playerSpeedOrig;
    bool isSprinting;
    bool playingFootSteps;

    private void Start()
    {
        HPOrig = HP;
        respawn();
        playerSpeedOrig = playerSpeed;
        //updateAmmoUI();
        if (!hasGun)
        {
            gameManager.instance.ammoCounter.text = "";
        }
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            movement();
            sprint();
            StartCoroutine(footSteps());
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
            aud.PlayOneShot(playerJumpSound[Random.Range(0, playerJumpSound.Length)], playerJumpSoundVol);
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        
    }

    void sprint()
    {
        if(Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed = playerSpeed * sprintMulti;
        }
        else if(Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed = playerSpeedOrig;
        }
    }

    IEnumerator footSteps()
    {
        if(!playingFootSteps && controller.isGrounded && move.normalized.magnitude > .3)
        {
            playingFootSteps = true;
            aud.PlayOneShot(playerFootstepsSound[Random.Range(0, playerFootstepsSound.Length)], playerFootstepsSoundVol);

            if (isSprinting)
                yield return new WaitForSeconds(0.3f);
            else
                yield return new WaitForSeconds(0.4f);

            playingFootSteps = false;
        }
    }

    public void gunPickup(Gun stats)
    {
        aud.PlayOneShot(weaponSwapSound[Random.Range(0, weaponSwapSound.Length)], weaponSwapSoundVol);
        if (hasGun == true)
        {
            gunInHand[gunStats[selectedGun].weaponInt].SetActive(false); //Changes the model
        }
        hasGun = true;
        shootrate = stats.shootrate;
        shootDamage = stats.shootDamage;
        shootdist = stats.shootdist;

        stats.setStartingAmmo();
        currentAmmo = stats.getAmmoCount;

        gunInHand[stats.weaponInt].SetActive(true); //Changes the model

        //gunModel.GetComponent<MeshFilter>().sharedMesh = stats.model.GetComponent<MeshFilter>().sharedMesh;
        //gunModel.GetComponent<MeshRenderer>().sharedMaterial = stats.model.GetComponent<MeshRenderer>().sharedMaterial;

        gunStats.Add(stats);
        selectedGun = gunStats.Count - 1;
        
        updateAmmoUI();
    }

    public void ammoPickup(int ammoAmount)
    {
        aud.PlayOneShot(ammoPickupSound[Random.Range(0, ammoPickupSound.Length)], ammoPickupSoundVol);
        currentAmmo += ammoAmount;
        gunStats[selectedGun].getAmmoCount = currentAmmo;
        updateAmmoUI();
    }

    void gunSelect()
    {
        if(gunStats.Count > 1)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunStats.Count - 1)
            {
                aud.PlayOneShot(weaponSwapSound[Random.Range(0, weaponSwapSound.Length)], weaponSwapSoundVol);
                gunInHand[gunStats[selectedGun].weaponInt].SetActive(false); //Changes the model
                selectedGun++;
                gunInHand[gunStats[selectedGun].weaponInt].SetActive(true); //Changes the model
                shootrate = gunStats[selectedGun].shootrate;
                shootdist = gunStats[selectedGun].shootdist;
                shootDamage = gunStats[selectedGun].shootDamage;

                currentAmmo = gunStats[selectedGun].getAmmoCount;
                

                Auto = gunStats[selectedGun].Auto;

                updateAmmoUI();

                
                //gunModel.GetComponent<MeshFilter>().sharedMesh = gunStats[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
                //gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStats[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
            {
                aud.PlayOneShot(weaponSwapSound[Random.Range(0, weaponSwapSound.Length)], weaponSwapSoundVol);
                gunInHand[gunStats[selectedGun].weaponInt].SetActive(false); //Changes the model
                selectedGun--;
                gunInHand[gunStats[selectedGun].weaponInt].SetActive(true); //Changes the model
                shootrate = gunStats[selectedGun].shootrate;
                shootdist = gunStats[selectedGun].shootdist;
                shootDamage = gunStats[selectedGun].shootDamage;
                currentAmmo = gunStats[selectedGun].getAmmoCount;
                Auto = gunStats[selectedGun].Auto;

                updateAmmoUI();

                //gunModel.GetComponent<MeshFilter>().sharedMesh = gunStats[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
                //gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStats[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
            }
        }
    }

    IEnumerator shoot()
    {
        if (gunStats.Count >= 1 && currentAmmo > 0 && Input.GetButton("Shoot") && !isShooting)
        {
            isShooting = true;
            currentAmmo--;
            gunStats[selectedGun].getAmmoCount = currentAmmo;
            updateAmmoUI();

            aud.PlayOneShot(gunStats[selectedGun].sound, gunShootSoundVol);

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootdist))
            {
                //if(hit.transform.CompareTag("Cube"))
                //Debug.Log("Casted");

                if (hit.collider.GetComponent<IDamageable>() != null)
                    hit.collider.GetComponent<IDamageable>().takeDamage(shootDamage);
                //Debug.Log("Connected");

                Instantiate(gunStats[selectedGun].hitEffect, hit.point, transform.rotation);
                Instantiate(gunStats[selectedGun].hitEffect, gunModel.transform.position, transform.rotation);

            }
            if (gunStats[selectedGun].Auto == 1)
            {
                yield return new WaitForSeconds(shootrate);
                isShooting = false;
            }
            else if (gunStats[selectedGun].Auto == 0)
            {
                yield return new WaitForSeconds(shootrate);
                isShooting = false;
            }
        }
    }

    public void updateAmmoUI()
    {
        gameManager.instance.ammoCounter.text = gunStats[selectedGun].getAmmoCount.ToString("F0");
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        updatePlayerHP();
        aud.PlayOneShot(playerDamage[Random.Range(0, playerDamage.Length)], playerDamageVol);

        StartCoroutine(damageFlash());

        if (HP <= 0)
        {
            gameManager.instance.playerIsDead();
        }
    }

    public void gainHealth(int amount)
    {
        HP += amount;
        aud.PlayOneShot(healthPickupSound[Random.Range(0, healthPickupSound.Length)], healthPickupSoundVol);
        updatePlayerHP();
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