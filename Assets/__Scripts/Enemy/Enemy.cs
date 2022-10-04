using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHp;
    public bool invulnerable;
    public bool nonflammable;
    [HideInInspector] public bool onFire;

    [Header("Audio")]
    public AudioClip damageAudio;
    public AudioClip deathAudio;

    [HideInInspector] public float hp { get { return currentHp; } }
    [HideInInspector] public float currentHp;

    GameObject player;
    AudioSource enemySource;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemySource = GetComponent<AudioSource>();
    }

    void Start()
    {
        enemySource.playOnAwake = false;
        enemySource.spatialBlend = 1f;
        enemySource.volume = 1f;
        enemySource.priority = 120;
    }

    public void DamageEnemy(float damage)
    {
        currentHp -= damage;
        
        if (currentHp <= 0)
        {
            // death audio
            Destroy(gameObject, 0.5f);
        }
        else
        {
            // damage audio
        }
    }
}
