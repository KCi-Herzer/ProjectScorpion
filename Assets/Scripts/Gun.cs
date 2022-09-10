using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu]

public class Gun : ScriptableObject
{
    [Header("----- Weapon Stats -----")]
    public float shootrate;
    public int shootdist;
    public int shootDamage;
    public int ammoCap;
    public GameObject model;


    /*[SerializeField] int ammoCap;
    [SerializeField] int shootdist;

    [SerializeField] float shootrate;
    [SerializeField] int shootDamage;
    [SerializeField] int ammoReserve;*/


    //[Header("----- UI -----")]
    

    //bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        //updateAmmoUI();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!gameManager.instance.isPaused)
        
            //StartCoroutine(shoot());
        
    }
    /*
    bool HaveAmmo()
    {
        if (ammoCap > 0)
            return true;
        else
            return false;
    }

    IEnumerator shoot()
    {
        if (!isShooting && Input.GetButtonDown("Shoot") && HaveAmmo())
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
            yield return new WaitForSeconds(shootrate);
            isShooting = false;
        }
    }

    public void updateAmmoUI()
    {
        gameManager.instance.ammoCounter.text = ammoCap.ToString("F0");
    }*/
}