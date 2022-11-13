using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : Enemy
{
    [Header("Audio")]
    public AudioClip hurtAudio;
    public AudioClip moveAudio;
    public AudioClip lungeAudio;
    public AudioClip spitAudio;

    [Header("GameObjects")]
    public ParticleSystem fireParticle;
    [SerializeField] private LayerMask hitMask;

    float movementSpeed;
    float aggroDistance;
    float minDistance;
    float maxDistance;
    float lungeDistance;
    float lungeDamage;
    float lungeRate;
    float blindDistance;
    float blindDuration;
    float blindRate;

    private float lastLungeTime;
    private float lastBlindTime;
    private LayerMask seeMask;
    private bool playerVisible;

    void Start()
    {
        enemySource.playOnAwake = false;
        enemySource.spatialBlend = 1f;
        enemySource.volume = 1f;
        enemySource.priority = 120;

        maxHp = Config.beetleMaxHp;
        currentHp = maxHp;

        enemyName = Config.beetleName;
        deathPosition = Config.beetleDeathPosition;
        movementSpeed = Config.beetleMovementSpeed;
        aggroDistance = Config.beetleAggroDistance;
        maxDistance = Config.beetleMaxDistance;
        lungeDistance = Config.beetleLungeDistance;
        lungeDamage = Config.beetleLungeDamage;
        lungeRate = Config.beetleLungeRate;
        blindDistance = Config.beetleBlindDistance;
        blindDuration = Config.beetleBlindDuration;
        blindRate = Config.beetleBlindRate;

        unstunnable = true; // beetles cannot be stunned

        seeMask = LayerMask.GetMask("Player", "Ground");
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        invulnerable = distanceToPlayer > maxDistance; // beetle is invulnerable when it is walking toward the player

        transform.LookAt(player.transform.position);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, aggroDistance, seeMask))
        {
            PlayerController player = hit.collider.gameObject.GetComponent<PlayerController>();
            playerVisible = player != null;
        }

        if (playerVisible && distanceToPlayer <= (aggroDistance * Config.enemyAggroRadiusModifier))
        {
            Move(distanceToPlayer);
            Attack(distanceToPlayer);
        }

        HandleDamageAudio();
        HandleBurn();
    }

    void Move(float distanceToPlayer)
    {
        if (distanceToPlayer > maxDistance) // wants to get closer to player
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }

        if (!enemySource.isPlaying && rb.velocity.magnitude > 0.1f && Time.timeScale > 0.1f)
        {
            enemySource.clip = moveAudio;
            enemySource.Play();
        }
    }

    void Attack(float distanceToPlayer)
    {
        if (distanceToPlayer <= lungeDistance && Time.time > lastLungeTime + (1/lungeRate)) // lunge attack
        {
            playerSystem = player.GetComponent<PlayerSystem>();

            transform.position += transform.forward;
            playerSystem.DamagePlayer(lungeDamage * Config.difficultyMod, enemyName);

            enemySource.clip = lungeAudio;
            enemySource.Play();

            lastLungeTime = Time.time;
        }

        if (distanceToPlayer > lungeDistance && distanceToPlayer <= blindDistance && Time.time > lastBlindTime + (1/blindRate)) // blind attack
        {
            playerSystem = player.GetComponent<PlayerSystem>();

            if (!playerSystem.blind)
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, blindDistance, hitMask))
                {
                    playerSystem.blind = true;
                    StartCoroutine(playerSystem.BlindPlayer(blindDuration));
                }

                enemySource.clip = spitAudio;
                enemySource.Play();

                lastBlindTime = Time.time;
            }
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
