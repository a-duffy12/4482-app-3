using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [Header("GameObjects")]
    public Transform firePoint;
    [SerializeField] private GameObject assaultRiflePrefab;
    [SerializeField] private GameObject shotgunPrefab;
    [SerializeField] private GameObject sniperPrefab;
    [SerializeField] private GameObject flamethrowerPrefab;
    
    AudioSource inventorySource;
    AssaultRifle assaultRifle;
    Shotgun shotgun;
    Sniper sniper;
    Flamethrower flamethrower;
    
    [HideInInspector] public int currentWeaponInt;

    void Awake()
    {
        inventorySource = GameObject.FindGameObjectWithTag("Inventory").GetComponent<AudioSource>();
        assaultRifle = assaultRiflePrefab.GetComponent<AssaultRifle>();
        shotgun = shotgunPrefab.GetComponent<Shotgun>();
        sniper = sniperPrefab.GetComponent<Sniper>();
        flamethrower = flamethrowerPrefab.GetComponent<Flamethrower>();
    }

    // Start is called before the first frame update
    void Start()
    {
        inventorySource.playOnAwake = false;
        inventorySource.spatialBlend = 1f;
        inventorySource.volume = 1f;

        currentWeaponInt = assaultRifle.weaponInt;
        assaultRiflePrefab.SetActive(true);
        shotgunPrefab.SetActive(false);
        sniperPrefab.SetActive(false);
        flamethrowerPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchWeapons(int weaponInt)
    {
        currentWeaponInt = weaponInt;

        if (currentWeaponInt == 0)
        {
            assaultRiflePrefab.SetActive(true);
            shotgunPrefab.SetActive(false);
            sniperPrefab.SetActive(false);
            flamethrowerPrefab.SetActive(false);
        }
        else if (currentWeaponInt == 1)
        {
            assaultRiflePrefab.SetActive(false);
            shotgunPrefab.SetActive(true);
            sniperPrefab.SetActive(false);
            flamethrowerPrefab.SetActive(false);
        }
        else if (currentWeaponInt == 2)
        {
            assaultRiflePrefab.SetActive(false);
            shotgunPrefab.SetActive(false);
            sniperPrefab.SetActive(true);
            flamethrowerPrefab.SetActive(false);
        }
        else if (currentWeaponInt == 3)
        {
            assaultRiflePrefab.SetActive(false);
            shotgunPrefab.SetActive(false);
            sniperPrefab.SetActive(false);
            flamethrowerPrefab.SetActive(true);
        }
    }

    #region input functions

    public void Shoot(InputAction.CallbackContext con)
    {
       
    }

    public void Aim(InputAction.CallbackContext con)
    {
       
    }

    public void Reload(InputAction.CallbackContext con)
    {
       
    }

    public void Item1(InputAction.CallbackContext con)
    {
        if (Config.assaultRifleUnlocked && con.performed)
        {
            SwitchWeapons(assaultRifle.weaponInt);
        }
    }

    public void Item2(InputAction.CallbackContext con)
    {
        if (Config.shotgunUnlocked && con.performed)
        {
            SwitchWeapons(shotgun.weaponInt);
            Debug.Log("2");
        }
    }

    public void Item3(InputAction.CallbackContext con)
    {
        if (Config.sniperUnlocked && con.performed)
        {
            SwitchWeapons(sniper.weaponInt);
            Debug.Log("3");
        }
    }

    public void Item4(InputAction.CallbackContext con)
    {
        if (Config.flamethrowerUnlocked && con.performed)
        {
            SwitchWeapons(flamethrower.weaponInt);
            Debug.Log("4");
        }
    }

    #endregion input functions
}
