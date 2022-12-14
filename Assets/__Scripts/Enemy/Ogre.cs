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
    public ParticleSystem fireParticle;

    float movementSpeed;
    float aggroDistance;
    float attackDistance;
    float damage;
    float attackRate;

    private float lastAttackTime;
    private LayerMask seeMask;
    private bool playerVisible;
    PlayerSystem system;

    void Start()
    {
        system = player.GetComponent<PlayerSystem>();
        fireParticle.Stop();

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

        seeMask = LayerMask.GetMask("Player", "Ground");
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        transform.LookAt(player.transform.position);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, aggroDistance, seeMask))
        {
            PlayerController player = hit.collider.gameObject.GetComponent<PlayerController>();
            playerVisible = player != null;
        }

        if (playerVisible && distanceToPlayer <= (aggroDistance * Config.enemyAggroRadiusModifier) && distanceToPlayer > (attackDistance - 0.5f) && Time.time > lastAttackTime + (1/attackRate) && !stunned) // only move towards player if within aggro range, not too close, and is ready to attack
        {
            Move();
        }

        if (playerVisible && distanceToPlayer <= attackDistance && !stunned)
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

        if (!enemySource.isPlaying && rb.velocity.magnitude > 0.1f && Time.timeScale > 0.1f)
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
            
            if (fireParticle.isPlaying)
            {
                fireParticle.Stop();
            }
        }
        else if (!nonflammable && onFire && Time.time >= nextFireTickTime)
        {
            DamageEnemy(Config.flamethrowerBurnDamage, "flamethrower");
            nextFireTickTime = Time.time + (1/Config.flamethrowerFireRate);
    
            if (!fireParticle.isPlaying)
            {
                fireParticle.Play();
            }
        }
    }
}
