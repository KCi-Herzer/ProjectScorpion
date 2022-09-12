using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ammoPickup : MonoBehaviour
{
    [SerializeField] int ammoAmount;
    [SerializeField] TMP_Text[] countLables;

    private void Start()
    {
        foreach (TMP_Text lable in countLables)
        {
            lable.text = "+" + ammoAmount.ToString("F0");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager.instance.playerScript.hasGun)
            {
                gameManager.instance.playerScript.ammoPickup(ammoAmount);
                Destroy(gameObject);
            }
        }
    }
}
