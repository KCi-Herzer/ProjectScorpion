using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class healthPickup : MonoBehaviour
{
    [SerializeField] int healthGain;
    [SerializeField] TMP_Text[] countLables;
    

    private void Start()
    {
        foreach (TMP_Text lable in countLables)
        {
            lable.text = "+" + healthGain.ToString("F0");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager.instance.playerScript)
            {
                gameManager.instance.playerScript.gainHealth(healthGain);
                Destroy(gameObject);
            }
        }
    }
}
