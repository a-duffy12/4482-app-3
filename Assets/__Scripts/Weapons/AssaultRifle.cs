using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private ParticleSystem muzzleFlash;
    public string weaponName = "assault_rifle";
    public int weaponInt = 0;
    public bool automatic = true;

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

    AudioSource audioSource;

    [HideInInspector] public int ammo { get { return currentAmmo; } }
    private int currentAmmo;
    private float lastFireTime;
    private float resetRecoilTime;
    private Vector3 weaponRotation;
    private bool needsReset;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fireRate = Config.assaultRifleFireRate;
        reloadTime = Config.assaultRifleReloadTime;
        maxAmmo = Config.assaultRifleMaxAmmo;
        damage = Config.assaultRifleDamage;
        fireVelocity = Config.assaultRifleFireVelocity;
        range = Config.assaultRifleRange;
        recoil = Config.assaultRifleRecoil;

        currentAmmo = maxAmmo;

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 0.5f;
        audioSource.priority = 150;
    }

    void Update()
    {
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
                    enemy.DamageEnemy(damage, weaponName);
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

            if (!needsReset) // only get weapon position on initial shot
            {
                needsReset = true;
                weaponRotation = transform.localEulerAngles;
            }

            float upRecoil = Random.Range(-0.1f, -0.5f) * recoil; // get veritcal recoil
            float sideRecoil = Random.Range(-0.7f, 0.7f) * recoil; // get horizontal recoil
            
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

    void ResetRecoil() // set weapon, camera, and player rotation back to what it was before
    {
        needsReset = false;
        transform.localEulerAngles = weaponRotation;
    }
}
