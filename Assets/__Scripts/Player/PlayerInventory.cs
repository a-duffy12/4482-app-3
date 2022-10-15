using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [Header("GameObjects")]
    public Transform firePoint;
    public Transform throwPoint;
    [SerializeField] private GameObject assaultRiflePrefab;
    [SerializeField] private GameObject shotgunPrefab;
    [SerializeField] private GameObject sniperPrefab;
    [SerializeField] private GameObject flamethrowerPrefab;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private GameObject knifePrefab;

    [Header("Audio")]
    public AudioClip switchWeaponAudio;
    public AudioClip throwGrenadeAudio;
    
    AudioSource inventorySource;
    AssaultRifle assaultRifle;
    Shotgun shotgun;
    Sniper sniper;
    Flamethrower flamethrower;
    Knife knife;
    
    [HideInInspector] public int currentWeaponInt;
    [HideInInspector] public bool refill;

    private float shootAutoWeapon;
    private float nextGrenadeTime;
    private float nextKnifeTime;

    void Awake()
    {
        inventorySource = GameObject.FindGameObjectWithTag("Inventory").GetComponent<AudioSource>();
        assaultRifle = assaultRiflePrefab.GetComponent<AssaultRifle>();
        shotgun = shotgunPrefab.GetComponent<Shotgun>();
        sniper = sniperPrefab.GetComponent<Sniper>();
        flamethrower = flamethrowerPrefab.GetComponent<Flamethrower>();
        knife = knifePrefab.GetComponent<Knife>();
    }

    // Start is called before the first frame update
    void Start()
    {
        inventorySource.playOnAwake = false;
        inventorySource.spatialBlend = 1f;
        inventorySource.volume = 1f;

        SwitchWeapons(assaultRifle.weaponInt);
        knifePrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (shootAutoWeapon == 1) // fire automatic weapons by holding down the trigger
        {
            if (currentWeaponInt == 0)
            {
                assaultRifle.Fire(firePoint);
            }
            else if (currentWeaponInt == 3)
            {
                flamethrower.Fire(firePoint);
            }
        }

        if (refill) // knife hit, refill ammo
        {
            refill = false;
            assaultRifle.currentAmmo += (int) (Config.assaultRifleMaxAmmo * Config.knifeRefillFraction);
            shotgun.currentAmmo += (int) (Config.shotgunMaxAmmo * Config.knifeRefillFraction);
            sniper.currentAmmo += (int) (Config.sniperMaxAmmo * Config.knifeRefillFraction);
            flamethrower.currentAmmo += (int) (Config.flamethrowerMaxAmmo * Config.knifeRefillFraction);
        }
    }

    void SwitchWeapons(int weaponInt)
    {
        if (weaponInt != currentWeaponInt)
        {
            currentWeaponInt = weaponInt;
            inventorySource.clip = switchWeaponAudio;
            inventorySource.Play();
        }

        sniper.scoped = false;
        Config.sniperScopedIn = false;

        if (currentWeaponInt == 0)
        {
            assaultRiflePrefab.SetActive(true);
            shotgunPrefab.SetActive(false);
            sniperPrefab.SetActive(false);
            flamethrowerPrefab.SetActive(false);
            
            assaultRifle.OverrideLastFireTime(); // allow shooting right after swapping
        }
        else if (currentWeaponInt == 1)
        {
            assaultRiflePrefab.SetActive(false);
            shotgunPrefab.SetActive(true);
            sniperPrefab.SetActive(false);
            flamethrowerPrefab.SetActive(false);

            shotgun.OverrideLastFireTime(); // allow shooting right after swapping
        }
        else if (currentWeaponInt == 2)
        {
            assaultRiflePrefab.SetActive(false);
            shotgunPrefab.SetActive(false);
            sniperPrefab.SetActive(true);
            flamethrowerPrefab.SetActive(false);

            sniper.OverrideLastFireTime(); // allow shooting right after swapping
        }
        else if (currentWeaponInt == 3)
        {
            assaultRiflePrefab.SetActive(false);
            shotgunPrefab.SetActive(false);
            sniperPrefab.SetActive(false);
            flamethrowerPrefab.SetActive(true);

            flamethrower.OverrideLastFireTime(); // allow shooting right after swapping
        }
    }

    void ThrowGrenade()
    {
        if (Time.time >= nextGrenadeTime)
        {
            GameObject grenadeObject = Instantiate(grenadePrefab, throwPoint.position, Random.rotation);
            grenadeObject.GetComponent<Rigidbody>().AddForce(throwPoint.transform.forward * Config.grenadeThrowForce);

            nextGrenadeTime = Time.time + Config.grenadeCooldown;

            inventorySource.clip = throwGrenadeAudio;
            inventorySource.Play();
        }
    }

    IEnumerator UseKnife(float duration)
    {
        if (Time.time >= nextKnifeTime)
        {
            knifePrefab.SetActive(true);
            StartCoroutine(knife.SwingKnife(firePoint));
            
            nextKnifeTime = Time.time + Config.knifeCooldown;

            yield return new WaitForSeconds(duration);

            knifePrefab.SetActive(false);
        }
    }

    #region input functions

    public void Shoot(InputAction.CallbackContext con)
    {
        if (con.performed) // key press and semi-auto weapon
       {
            if (currentWeaponInt == 1)
            {
                shotgun.Fire(firePoint);
            }
            else if (currentWeaponInt == 2)
            {
                sniper.Fire(firePoint);
            }
       }

       shootAutoWeapon = con.ReadValue<float>(); // key hold and auto weapon
    }

    public void Aim(InputAction.CallbackContext con)
    {
        if (currentWeaponInt == 2)
        {
            if (con.ReadValue<float>() > 0.5f && !sniper.scoped) // not scoped but should be
            {
                sniper.AimDownSights();
            }
            else if (con.ReadValue<float>() < 0.5f && sniper.scoped) // scoped but should not be
            {
                sniper.Hipfire();
            }
        }
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
        }
    }

    public void Item3(InputAction.CallbackContext con)
    {
        if (Config.sniperUnlocked && con.performed)
        {
            SwitchWeapons(sniper.weaponInt);
        }
    }

    public void Item4(InputAction.CallbackContext con)
    {
        if (Config.flamethrowerUnlocked && con.performed)
        {
            SwitchWeapons(flamethrower.weaponInt);
        }
    }

    public void Throwable(InputAction.CallbackContext con)
    {
        if (Config.grenadeUnlocked && con.performed)
        {
            ThrowGrenade();
        }
    }

    public void Melee(InputAction.CallbackContext con)
    {
        if (Config.knifeUnlocked && con.performed)
        {
            StartCoroutine(UseKnife(Config.knifeDuration));
        }
    }

    #endregion input functions
}
