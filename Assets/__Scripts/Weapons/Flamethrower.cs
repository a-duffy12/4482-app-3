using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private ParticleSystem sparkFlash;
    public string weaponName = "flamethrower";
    public int weaponInt = 3;
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
    float burnTime;

    AudioSource audioSource;

    [HideInInspector] public int currentAmmo;
    private float lastFireTime;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fireRate = Config.flamethrowerFireRate;
        reloadTime = Config.flamethrowerReloadTime;
        maxAmmo = Config.flamethrowerMaxAmmo;
        damage = Config.flamethrowerDamage;
        fireVelocity = Config.flamethrowerFireVelocity;
        range = Config.flamethrowerRange;
        burnTime = Config.flamethrowerBurnTime;

        currentAmmo = maxAmmo;

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 1f;
        audioSource.priority = 150;
    }

    void Update()
    {
        if (currentAmmo > maxAmmo)
        {
            currentAmmo = maxAmmo;
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
                    enemy.IgniteEnemy(burnTime);
                }
            }

            currentAmmo--;
            lastFireTime = Time.time;

            audioSource.clip = fireAudio;
            audioSource.Play();
            
            if (sparkFlash.isPlaying)
            {
                sparkFlash.Stop();
            }
            sparkFlash.Play();
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
}
