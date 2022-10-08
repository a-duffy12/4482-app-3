using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private GameObject grenadeModel;
    public string weaponName = "grenade";

    [Header("Audio")]
    public AudioClip explodeAudio;
    
    float fuseTime;
    float blastRadius;
    float damage;
    float knockbackForce;
    float stunDuration;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fuseTime = Config.grenadeFuseTime;
        blastRadius = Config.grenadeBlastRadius;
        damage = Config.grenadeDamage;
        knockbackForce = Config.grenadeKnockbackForce;
        stunDuration = Config.grenadeStunDuration;

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 1f;
        audioSource.priority = 130;

        StartCoroutine(ActiveGrenade(fuseTime));
    }

    IEnumerator ActiveGrenade(float fuseTime)
    {
        yield return new WaitForSeconds(fuseTime);

        audioSource.clip = explodeAudio;
        audioSource.Play();

        Instantiate(explosionParticle, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius); // get all colliders in blast radius

        foreach (Collider col in colliders)
        {
            Enemy enemy = col.transform.GetComponent<Enemy>();

            if (enemy != null)
            {
                float distance = Vector3.Distance(col.transform.position, transform.position);
                Rigidbody rb = col.transform.GetComponent<Rigidbody>();

                enemy.DamageEnemy(damage / (blastRadius - distance + 0.1f) , weaponName);
                rb.AddForce((transform.position + col.transform.position) * (knockbackForce / (blastRadius - distance + 0.1f)));
                enemy.StunEnemy(stunDuration);
            }
        }

        grenadeModel.SetActive(false);
        Destroy(gameObject, audioSource.clip.length);
    }
}
