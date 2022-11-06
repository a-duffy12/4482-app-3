using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Enemy
{
    [Header("Audio")]
    public AudioClip hurtAudio;
    public AudioClip moveAudio;
    public AudioClip attackAudio;

    [Header("GameObjects")]
    public GameObject fireballPrefab;
    public Transform firePoint;

    float movementSpeed;
    float aggroDistance;
    float minDistance;
    float maxDistance;
    float damage;
    float attackRate;
    float startleDelay;
    float fireballSpeed;

    private float lastAttackTime;
    private float lastStartleTime;
    private bool startled;
    private LayerMask seeMask;
    private bool playerVisible;

    void Start()
    {
        enemySource.playOnAwake = false;
        enemySource.spatialBlend = 1f;
        enemySource.volume = 1f;
        enemySource.priority = 120;

        maxHp = Config.demonMaxHp;
        currentHp = maxHp;

        enemyName = Config.demonName;
        deathPosition = Config.demonDeathPosition;
        movementSpeed = Config.demonMovementSpeed;
        aggroDistance = Config.demonAggroDistance;
        minDistance = Config.demonMinDistance;
        maxDistance = Config.demonMaxDistance;
        damage = Config.demonDamage;
        attackRate = Config.demonAttackRate;
        startleDelay = Config.demonStartleDelay;
        fireballSpeed = Config.demonFireballSpeed;

        nonflammable = true; // demons cannot be lit on fire

        seeMask = LayerMask.GetMask("Player");
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

        if (playerVisible && distanceToPlayer <= (aggroDistance * Config.enemyAggroRadiusModifier)) // only move towards player if within aggro range
        {
            if (Time.time > lastStartleTime + startleDelay && !stunned)
            {
                Move(distanceToPlayer);
                Attack(distanceToPlayer);
                startled = false;
            }
        }

        if (distanceToPlayer <= minDistance) // demon gets startled when you get to close
        {
            if (!startled)
            {
                lastStartleTime = Time.time;
                startled = true;
            }
        }

        HandleDamageAudio();
        HandleStun();
    }

    void Move(float distanceToPlayer)
    {
        if (distanceToPlayer > maxDistance) // wants to get closer to player
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
        else if (distanceToPlayer < minDistance) // wants to get further from player
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -1 * movementSpeed * Time.deltaTime);
        }

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
            GameObject fireballObject = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
            DemonFireball fireball = fireballObject.GetComponent<DemonFireball>();
            fireball.Shoot(firePoint.transform.forward, fireballSpeed);

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
}
