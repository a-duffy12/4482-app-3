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
    private float stabCounter;

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
        if (stabCounter == 30)
        {
            stab = false;
        }

        if (stab && stabCounter < 15)
        {
            stabCounter++;
            transform.position += (transform.right * 0.1f);
        }
        else if (stab && stabCounter >= 15 && stabCounter < 30)
        {
            stabCounter++;
            transform.position += (transform.right * -0.1f);
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
        Debug.Log("stab");

        if (Physics.Raycast(firePoint.position, firePoint.transform.forward, out RaycastHit hit, range, hitMask))
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage, weaponName); // deal big damage
        
                inventory.refill = true; // tell inventory to refill ammo
            }
        }        
    }
}
