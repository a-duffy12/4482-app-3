using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonFireball : MonoBehaviour
{
    [Header("Stats")]
    public float flyTime;

    [Header("Audio")]
    public AudioClip fireballImpactAudio;
    
    Rigidbody rb;
    AudioSource fireballSource;

    float fireTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        fireballSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fireballSource.playOnAwake = false;
        fireballSource.spatialBlend = 1f;
        fireballSource.volume = 1f;
        fireballSource.priority = 130;

        flyTime = Config.demonFireballFlyTime;
        fireTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > fireTime + flyTime)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector3 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    void OnCollisionEnter(Collision collision)
    {
        PlayerSystem system = collision.collider.GetComponent<PlayerSystem>();

        if (system != null)
        {
            system.DamagePlayer(Config.demonDamage * Config.difficultyMod, Config.demonName);
            
            fireballSource.clip = fireballImpactAudio;
            fireballSource.Play();
        }
    }
}
