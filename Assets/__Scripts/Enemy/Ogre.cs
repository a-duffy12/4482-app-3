using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre : Enemy
{
    [Header("Audio")]
    public AudioClip hurtAudio;
    public AudioClip moveAudio;
    public AudioClip attackAudio;

    [Header("GameObjects")]
    public GameObject fireParticle;

    float movementSpeed;
    float aggroDistance;
    float attackDistance;
    float damage;
    float attackRate;

    private float lastAttackTime;
    PlayerSystem system;

    void Start()
    {
        system = player.GetComponent<PlayerSystem>();
        fireParticle.SetActive(false);

        enemySource.playOnAwake = false;
        enemySource.spatialBlend = 1f;
        enemySource.volume = 1f;
        enemySource.priority = 120;

        maxHp = Config.ogreMaxHp;
        currentHp = maxHp;

        enemyName = Config.ogreName;
        deathPosition = Config.ogreDeathPosition;
        movementSpeed = Config.ogreMovementSpeed;
        aggroDistance = Config.ogreAggroDistance;
        attackDistance = Config.ogreAttackDistance;
        damage = Config.ogreDamage;
        attackRate = Config.ogreAttackRate;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        transform.LookAt(player.transform.position);

        if (distanceToPlayer <= (aggroDistance * Config.enemyAggroRadiusModifier) && distanceToPlayer > (attackDistance - 0.5f) && Time.time > lastAttackTime + (1/attackRate) && !stunned) // only move towards player if within aggro range, not too close, and is ready to attack
        {
            Move();
        }

        if (distanceToPlayer <= attackDistance && !stunned)
        {
            Attack(distanceToPlayer);
        }

        HandleDamageAudio();
        HandleStun();
        HandleBurn();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);

        if (!enemySource.isPlaying && rb.velocity.magnitude > 0.1f)
        {
            enemySource.clip = moveAudio;
            enemySource.Play();
        }
    }

    void Attack(float distanceToPlayer)
    {
        if (Time.time > lastAttackTime + (1/attackRate))
        {
            if (distanceToPlayer <= attackDistance)
            {
                system.DamagePlayer(damage * Config.difficultyMod, enemyName);
            }

            lastAttackTime = Time.time;

            enemySource.clip = attackAudio;
            enemySource.Play();
        }
    }

    void HandleDamageAudio()
    {
        // play audio relating to enemy hp status
        if (damaged)
        {
            enemySource.clip = hurtAudio;
            enemySource.Play();
            damaged = false;
        }
    }

    void HandleStun()
    {
        if (stunned && Time.time >= unStunTime)
        {
            stunned = false;
        }
    }

    void HandleBurn()
    {
        if (!nonflammable && onFire && Time.time >= unFireTime)
        {
            onFire = false;

            if (fireParticle.activeInHierarchy)
            {
                fireParticle.SetActive(false);
            }
        }
        else if (!nonflammable && onFire && Time.time >= nextFireTickTime)
        {
            DamageEnemy(Config.flamethrowerBurnDamage, "flamethrower");
            nextFireTickTime = Time.time + (1/Config.flamethrowerFireRate);
    
            if (!fireParticle.activeInHierarchy)
            {
                fireParticle.SetActive(true);
            }
        }
    }
}
