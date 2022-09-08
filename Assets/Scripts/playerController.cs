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

    //SEE GUN.CS for shooting

    [Header("-----UI Settings-----")]
    [SerializeField] float playerDamageFlashTime;


    int HPOrig;
    int timesJumped;
    private Vector3 playerVelocity;
    Vector3 move;
    //bool isShooting;


    private void Start()
    {
        HPOrig = HP;
        respawn();
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            movement();
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

    //IEnumerator shoot()
    //{
    //    //SEE GUN.CS for shooting
    //}

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