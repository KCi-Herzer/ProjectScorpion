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


    int timesJumped;
    private Vector3 playerVelocity;
    Vector3 move;


    private void Start()
    {

    }

    void Update()
    {
        movement();
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

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        StartCoroutine(damageFlash());
    }

    IEnumerator damageFlash()
    {
        gameManager.instance.playerDamage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamage.SetActive(false);
    }
}