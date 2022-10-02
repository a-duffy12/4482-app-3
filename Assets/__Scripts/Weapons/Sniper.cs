using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject bulletHole;
    [SerializeField] private GameObject adsOverlay;
    public string weaponName = "sniper";
    public int weaponInt = 2;
    public bool automatic = false;

    [Header("Audio")]
    public AudioClip fireAudio;
    
    float fireRate;
    float reloadTime;
    int maxAmmo;
    float damage;
    float fireVelocity;
    float range;
    float adsFov;

    AudioSource audioSource;
    Camera eyes;

    [HideInInspector] public int ammo { get { return currentAmmo; } }
    [HideInInspector] public bool scoped;
    private int currentAmmo;
    private float lastFireTime;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        eyes = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fireRate = Config.sniperFireRate;
        reloadTime = Config.sniperReloadTime;
        maxAmmo = Config.sniperMaxAmmo;
        damage = Config.sniperDamage;
        fireVelocity = Config.sniperFireVelocity;
        range = Config.sniperRange;
        adsFov = Config.sniperAdsFov;

        currentAmmo = maxAmmo;

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 1f;
        audioSource.priority = 150;

        adsOverlay.SetActive(false);
    }

    void Update()
    {
        if (scoped)
        {
            adsOverlay.SetActive(true);
        }
        else
        {
            adsOverlay.SetActive(false);
        }
    }

    public void Fire(Transform firePoint)
    {
        if (Time.time > (lastFireTime + (1/fireRate)) && currentAmmo > 0) // has ammo and can fire
        {
            if (Physics.Raycast(firePoint.position, firePoint.transform.forward, out RaycastHit hit, range, hitMask))
            {
                // hit enemy
            }

            currentAmmo--;
            lastFireTime = Time.time;

            audioSource.clip = fireAudio;
            audioSource.Play();

            if (muzzleFlash.isPlaying)
            {
                muzzleFlash.Stop();
            }
            muzzleFlash.Play();
            
            Hipfire(); // force unscope after shooting
        }
    }

    public void Reload()
    {
        Debug.Log("Reload " + weaponName);
    }

    public void OverrideLastFireTime() // allows weapon to fire as soon as it is swapped to
    {
        lastFireTime = Time.time - (1/fireRate);
    }

    public void AimDownSights() // go from hipfire to ads
    {
        scoped = true;
        Config.sniperScopedIn = true;
        eyes.fieldOfView = adsFov;

        // play audio
    }

    public void Hipfire() // go from ads to hipfire
    {
        scoped = false;
        Config.sniperScopedIn = false;
        eyes.fieldOfView = Config.fieldOfView;

        // play audio
    }
}
