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
    public int Auto;
    public int currentAmmo;



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
    
}