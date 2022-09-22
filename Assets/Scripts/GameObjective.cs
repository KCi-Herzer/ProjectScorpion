using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjective : MonoBehaviour, IDamageable
{
    [Range(0, 100)] [SerializeField] int HP;
    [SerializeField] Renderer rend;

    [Header("----- Audio -----")]

    [SerializeField] AudioSource aud;

    [SerializeField] AudioClip[] objDamageSound;
    [Range(0, 1)] [SerializeField] float objDamageSoundVol;

    public void takeDamage(int dmg)
    {
        aud.PlayOneShot(objDamageSound[Random.Range(0, objDamageSound.Length)], objDamageSoundVol);
        HP -= dmg;
        StartCoroutine(flashColor());
        if (HP <= 0)
        {
            
            Destroy(gameObject);
            gameManager.instance.totalObjectives--;
            gameManager.instance.updateObjectiveUI();
        }
    }

    IEnumerator flashColor()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = Color.white;
    }

}
