using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    [Header("Stats")]
    public string weaponName = "sniper";
    public int weaponInt = 2;
    public bool automatic = false;
    
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
        fireRate = Config.sniperFireRate;
        reloadTime = Config.sniperReloadTime;
        maxAmmo = Config.sniperMaxAmmo;
        damage = Config.sniperDamage;
        fireVelocity = Config.sniperFireVelocity;

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
