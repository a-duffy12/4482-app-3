using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : MonoBehaviour
{
    [Header("Stats")]
    public string weaponName = "assault_rifle";
    public int weaponInt = 0;
    public bool automatic = true;
    
    float fireRate;
    float reloadTime;
    int maxAmmo;
    float damage;
    float fireVelocity;

    [HideInInspector] public int ammo { get { return currentAmmo; } }
    private int currentAmmo;

    // Start is called before the first frame update
    void Start()
    {
        fireRate = Config.assaultRifleFireRate;
        reloadTime = Config.assaultRifleReloadTime;
        maxAmmo = Config.assaultRifleMaxAmmo;
        damage = Config.assaultRifleDamage;
        fireVelocity = Config.assaultRifleFireVelocity;

        currentAmmo = maxAmmo;
    }

    public void Fire()
    {
        Debug.Log("Fire " + weaponName);
    }

    public void Reload()
    {
        Debug.Log("Reload " + weaponName);
    }
}
