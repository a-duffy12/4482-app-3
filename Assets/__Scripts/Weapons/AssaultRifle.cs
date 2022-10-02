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
    
    float fireRate;
    float reloadTime;
    int maxAmmo;
    float damage;
    float fireVelocity;
    float range;

    AudioSource audioSource;

    [HideInInspector] public int ammo { get { return currentAmmo; } }
    private int currentAmmo;
    private float lastFireTime;

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

        currentAmmo = maxAmmo;

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 1f;
        audioSource.priority = 150;
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
}
