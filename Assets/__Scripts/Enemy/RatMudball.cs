using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatMudball : MonoBehaviour
{
    [Header("Stats")]
    public float flyTime;

    [Header("Audio")]
    public AudioClip mudballImpactAudio;
    
    Rigidbody rb;
    AudioSource mudballSource;

    float fireTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mudballSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mudballSource.playOnAwake = false;
        mudballSource.spatialBlend = 1f;
        mudballSource.volume = 1f;
        mudballSource.priority = 130;

        flyTime = Config.ratMudballFlyTime;
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
            system.DamagePlayer(Config.ratDamage * Config.difficultyMod, Config.ratName);
            
            mudballSource.clip = mudballImpactAudio;
            mudballSource.Play();
        }
    }
}
