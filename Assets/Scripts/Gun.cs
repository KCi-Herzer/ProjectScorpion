using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [SerializeField] int ammoCap;
    [SerializeField] int shootdist;

    [SerializeField] float shootrate;
    [SerializeField] int shootDamage;
    [SerializeField] int ammoReserve;

    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(shoot());
    }

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

            RaycastHit hit;
            Debug.Log("Shooting");
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootdist))
            {
                //if(hit.transform.CompareTag("Cube"))
                Debug.Log("Casted");
                if (hit.collider.GetComponent<IDamageable>() != null)
                {
                    Debug.Log("Connected");
                    hit.collider.GetComponent<IDamageable>().takeDamage(shootDamage);
                }
            }
            //Instantiate(Cube, transform.position, Cube.transform.rotation);
            yield return new WaitForSeconds(shootrate);
            isShooting = false;
        }
    }
}