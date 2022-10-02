using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [Header("Stats")]
    public string weaponName = "flamethrower";
    public int weaponInt = 3;
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
        fireRate = Config.flamethrowerFireRate;
        reloadTime = Config.flamethrowerReloadTime;
        maxAmmo = Config.flamethrowerMaxAmmo;
        damage = Config.flamethrowerDamage;
        fireVelocity = Config.flamethrowerFireVelocity;

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
