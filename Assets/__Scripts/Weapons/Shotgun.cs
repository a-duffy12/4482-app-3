using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private ParticleSystem muzzleFlash;
    public string weaponName = "shotgun";
    public int weaponInt = 1;
    public bool automatic = false;

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
        fireRate = Config.shotgunFireRate;
        reloadTime = Config.shotgunReloadTime;
        maxAmmo = Config.shotgunMaxAmmo;
        damage = Config.shotgunDamage;
        fireVelocity = Config.shotgunFireVelocity;
        range = Config.shotgunRange;

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
