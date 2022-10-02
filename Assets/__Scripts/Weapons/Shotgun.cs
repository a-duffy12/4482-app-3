using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("Stats")]
    public string weaponName = "shotgun";
    public int weaponInt = 1;
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
        fireRate = Config.shotgunFireRate;
        reloadTime = Config.shotgunReloadTime;
        maxAmmo = Config.shotgunMaxAmmo;
        damage = Config.shotgunDamage;
        fireVelocity = Config.shotgunFireVelocity;

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
