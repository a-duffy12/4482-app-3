using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private LayerMask hitMask;
    public string weaponName = "knife";

    [Header("Audio")]
    public AudioClip knifeAudio;

    float damage;
    float range;

    AudioSource audioSource;
    PlayerInventory inventory;

    private bool stab;
    private int stabCounter;

     void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        damage = Config.knifeDamage;
        range = Config.knifeRange;

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 1f;
        audioSource.priority = 150;
    }

    void FixedUpdate()
    {
        if (stabCounter == 20)
        {
            stab = false;
        }

        if (stab && stabCounter < 10)
        {
            stabCounter++;
            transform.position += (transform.right * 0.15f);
        }
        else if (stab && stabCounter >= 10 && stabCounter < 20)
        {
            stabCounter++;
            transform.position += (transform.right * -0.15f);
        }
    }

    public IEnumerator SwingKnife(Transform firePoint)
    {
        audioSource.clip = knifeAudio;
        audioSource.Play();

        if (!stab)
        {
            stab = true;
            stabCounter = 0;
        }

        yield return new WaitForSeconds(Config.knifeDuration * 0.4f);

        if (Physics.Raycast(firePoint.position, firePoint.transform.forward, out RaycastHit hit1, range, hitMask))
        {
            Enemy enemy = hit1.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage, weaponName); // deal big damage
        
                inventory.refill = true; // tell inventory to refill ammo
            }
        }
        else if (Physics.Raycast(firePoint.position, Quaternion.Euler(0, 10.0f, 0) * firePoint.transform.forward, out RaycastHit hit2, range, hitMask))        
        {
           Enemy enemy = hit2.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage, weaponName); // deal big damage
        
                inventory.refill = true; // tell inventory to refill ammo
            } 
        }
        else if (Physics.Raycast(firePoint.position, Quaternion.Euler(0, -10.0f, 0) * firePoint.transform.forward, out RaycastHit hit3, range, hitMask))        
        {
           Enemy enemy = hit3.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage, weaponName); // deal big damage
        
                inventory.refill = true; // tell inventory to refill ammo
            } 
        }
        else if (Physics.Raycast(firePoint.position, Quaternion.Euler(10.0f, 0, 0) * firePoint.transform.forward, out RaycastHit hit4, range, hitMask))        
        {
           Enemy enemy = hit4.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage, weaponName); // deal big damage
        
                inventory.refill = true; // tell inventory to refill ammo
            } 
        }
        else if (Physics.Raycast(firePoint.position, Quaternion.Euler(-10.0f, 0, 0) * firePoint.transform.forward, out RaycastHit hit5, range, hitMask))        
        {
           Enemy enemy = hit5.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage, weaponName); // deal big damage
        
                inventory.refill = true; // tell inventory to refill ammo
            } 
        }
    }
}
