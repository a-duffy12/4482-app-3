using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject bulletHole;
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
            Vector3 downPellet = Quaternion.Euler(-3.0f, 0, 0) * firePoint.transform.forward;
            Vector3 upPellet = Quaternion.Euler(3.0f, 0, 0) * firePoint.transform.forward;
            Vector3 rightPellet = Quaternion.Euler(0, 3.0f, 0) * firePoint.transform.forward;
            Vector3 leftPellet = Quaternion.Euler(0, -3.0f, 0) * firePoint.transform.forward;
            Vector3 downleftPellet = Quaternion.Euler(-2.25f, -2.25f, 0) * firePoint.transform.forward;
            Vector3 upleftPellet = Quaternion.Euler(2.25f, -2.25f, 0) * firePoint.transform.forward;
            Vector3 downrightPellet = Quaternion.Euler(-2.25f, 2.25f, 0) * firePoint.transform.forward;
            Vector3 uprightPellet = Quaternion.Euler(2.25f, 2.25f, 0) * firePoint.transform.forward;

            if (Physics.Raycast(firePoint.position, firePoint.transform.forward, out RaycastHit hi1t, range, hitMask))
            {
                // hit enemy
                Debug.Log("1");
            }

            if (Physics.Raycast(firePoint.position, downPellet, out RaycastHit hit2, range, hitMask))
            {
                // hit enemy
                Debug.Log("2");
            }

            if (Physics.Raycast(firePoint.position, upPellet, out RaycastHit hit3, range, hitMask))
            {
                // hit enemy
                Debug.Log("3");
            }

            if (Physics.Raycast(firePoint.position, rightPellet, out RaycastHit hit4, range, hitMask))
            {
                // hit enemy
                Debug.Log("4");
            }

            if (Physics.Raycast(firePoint.position, leftPellet, out RaycastHit hit5, range, hitMask))
            {
                // hit enemy
                Debug.Log("5");
            }

            if (Physics.Raycast(firePoint.position, downleftPellet, out RaycastHit hit6, range, hitMask))
            {
                // hit enemy
                Debug.Log("6");
            }

            if (Physics.Raycast(firePoint.position, upleftPellet, out RaycastHit hit7, range, hitMask))
            {
                // hit enemy
                Debug.Log("7");
            }

            if (Physics.Raycast(firePoint.position, downrightPellet, out RaycastHit hit8, range, hitMask))
            {
                // hit enemy
                Debug.Log("8");
            }

            if (Physics.Raycast(firePoint.position, uprightPellet, out RaycastHit hit9, range, hitMask))
            {
                // hit enemy
                Debug.Log("9");
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
