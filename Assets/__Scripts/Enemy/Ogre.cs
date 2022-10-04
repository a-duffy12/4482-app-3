using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre : Enemy
{
    [Header("Audio")]
    public AudioClip moveAudio;
    public AudioClip attackAudio;

    float movementSpeed;
    float aggroDistance;
    float attackDistance;
    float damage;

    AudioSource ogreSource;

    void Awake()
    {
        ogreSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        ogreSource.playOnAwake = false;
        ogreSource.spatialBlend = 1f;
        ogreSource.volume = 1f;
        ogreSource.priority = 120;

        maxHp = Config.ogreMovementSpeed;
        currentHp = maxHp;

        movementSpeed = Config.ogreMovementSpeed;
        aggroDistance = Config.ogreAggroDistance;
        attackDistance = Config.ogreAttackDistance;
        damage = Config.ogreDamage;
    }

    void Update()
    {
        // move

        // if in range, attack
    }
}
