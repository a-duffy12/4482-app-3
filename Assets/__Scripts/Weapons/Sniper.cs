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
    public AudioClip emptyAudio;
    
    float fireRate;
    float reloadTime;
    int maxAmmo;
    float damage;
    float fireVelocity;
    float range;
    float recoil;
    float adsFov;

    AudioSource audioSource;
    Camera eyes;

    [HideInInspector] public int ammo { get { return currentAmmo; } }
    [HideInInspector] public bool scoped;
    private int currentAmmo;
    private float lastFireTime;
    private float resetRecoilTime;
    private Vector3 weaponRotation;
    private bool needsReset;

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
        recoil = Config.sniperRecoil;

        currentAmmo = maxAmmo;

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 0.8f;
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

        if (needsReset && Time.time >= resetRecoilTime)
        {
            ResetRecoil();
        }
    }

    public void Fire(Transform firePoint)
    {
        if (Time.time > (lastFireTime + (1/fireRate)) && currentAmmo > 0) // has ammo and can fire
        {
            if (Physics.Raycast(firePoint.position, firePoint.transform.forward, out RaycastHit hit, range, hitMask))
            {
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (Config.sniperScopedIn)
                    {
                        enemy.DamageEnemy(damage, weaponName);
                    }
                    else
                    {
                        enemy.DamageEnemy(damage * Config.sniperUnAdsDamageMod, weaponName);
                    }
                }
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

            if (!needsReset) // only get weapon position on initial shot
            {
                needsReset = true;
                weaponRotation = transform.localEulerAngles;
            }

            float upRecoil = Random.Range(-0.4f, -0.5f) * recoil; // get veritcal recoil
            float sideRecoil = Random.Range(-0.2f, 0.2f) * recoil; // get horizontal recoil
            
            transform.localEulerAngles += new Vector3(upRecoil, 0, sideRecoil); // add recoil
            resetRecoilTime = Time.time + Config.recoilResetDelay; // set time to lower weapon
        }
        else if (Time.time > (lastFireTime + (1/fireRate))) // no ammo and can fire
        {
            audioSource.clip = emptyAudio;
            audioSource.Play();
        }
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

    void ResetRecoil() // set weapon, camera, and player rotation back to what it was before
    {
        needsReset = false;
        transform.localEulerAngles = weaponRotation;
    }
}
