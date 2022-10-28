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
    public AudioClip emptyAudio;
    
    float fireRate;
    float reloadTime;
    int maxAmmo;
    float damage;
    float fireVelocity;
    float range;

    AudioSource audioSource;

    [HideInInspector] public int currentAmmo;
    private float lastFireTime;
    private bool cycle;
    private int cycleCounter;

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

    void Update()
    {
        if (currentAmmo > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
    }

    void FixedUpdate()
    {
        if (cycleCounter == 20)
        {
            cycle = false;
        }

        if (cycle && cycleCounter < 20)
        {
            cycleCounter++;
            transform.localEulerAngles = new Vector3(-18f * cycleCounter, 0, 0);
        }
    }

    public void Fire(Transform firePoint)
    {
        if (Time.time > (lastFireTime + (1/fireRate)) && currentAmmo > 0) // has ammo and can fire
        {
            Vector3 downPellet = Quaternion.Euler(-3.0f, 0, 0) * firePoint.transform.forward;
            Vector3 upPellet = Quaternion.Euler(3.0f, 0, 0) * firePoint.transform.forward;
            Vector3 rightPellet = Quaternion.Euler(0, 3.0f, 0) * firePoint.transform.forward;
            Vector3 leftPellet = Quaternion.Euler(0, -3.0f, 0) * firePoint.transform.forward;
            Vector3 downleftPellet = Quaternion.Euler(-2.25f, -2.25f, 0) * firePoint.transform.forward;
            Vector3 upleftPellet = Quaternion.Euler(2.25f, -2.25f, 0) * firePoint.transform.forward;
            Vector3 downrightPellet = Quaternion.Euler(-2.25f, 2.25f, 0) * firePoint.transform.forward;
            Vector3 uprightPellet = Quaternion.Euler(2.25f, 2.25f, 0) * firePoint.transform.forward;

            if (Physics.Raycast(firePoint.position, firePoint.transform.forward, out RaycastHit hit1, range, hitMask))
            {
                Enemy enemy = hit1.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damage, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, downPellet, out RaycastHit hit2, range, hitMask))
            {
                Enemy enemy = hit2.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damage, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, upPellet, out RaycastHit hit3, range, hitMask))
            {
                Enemy enemy = hit3.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damage, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, rightPellet, out RaycastHit hit4, range, hitMask))
            {
                Enemy enemy = hit4.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damage, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, leftPellet, out RaycastHit hit5, range, hitMask))
            {
                Enemy enemy = hit5.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damage, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, downleftPellet, out RaycastHit hit6, range, hitMask))
            {
                Enemy enemy = hit6.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damage, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, upleftPellet, out RaycastHit hit7, range, hitMask))
            {
                Enemy enemy = hit7.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damage, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, downrightPellet, out RaycastHit hit8, range, hitMask))
            {
                Enemy enemy = hit8.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damage, weaponName);
                }
            }

            if (Physics.Raycast(firePoint.position, uprightPellet, out RaycastHit hit9, range, hitMask))
            {
                Enemy enemy = hit9.collider.gameObject.GetComponent<Enemy>();
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

            if (!cycle) // spin shotgun if isn't already spinning
            {
                cycle = true;
                cycleCounter = 1;
                transform.localEulerAngles = new Vector3(-18f * cycleCounter, 0, 0);
            }
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
