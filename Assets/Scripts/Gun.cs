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
    public int Auto;
    public GameObject model;
    public AudioClip sound;
    public GameObject hitEffect;

    int currentAmmo;

    public void setStartingAmmo()
    {
        currentAmmo = ammoCap;
    }

    public int getAmmoCount
    {
        get { return currentAmmo; }
        set { currentAmmo = value; }
    }
    
}