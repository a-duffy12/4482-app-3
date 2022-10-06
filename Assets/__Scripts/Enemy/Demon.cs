using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Enemy
{
    [Header("Audio")]
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

    void Start()
    {
        maxHp = Config.demonMaxHp;
        currentHp = maxHp;
        nonflammable = true;

        enemyName = Config.demonName;
        movementSpeed = Config.demonMovementSpeed;
        aggroDistance = Config.demonAggroDistance;
        minDistance = Config.demonMinDistance;
        maxDistance = Config.demonMaxDistance;
        damage = Config.demonDamage;
        attackRate = Config.demonAttackRate;
        startleDelay = Config.demonStartleDelay;
        fireballSpeed = Config.demonFireballSpeed;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        transform.LookAt(player.transform.position);

        if (distanceToPlayer <= (aggroDistance * Config.enemyAggroRadiusModifier)) // only move towards player if within aggro range
        {
            if (Time.time > lastStartleTime + startleDelay)
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
    }

    void Move(float distanceToPlayer)
    {
        if (distanceToPlayer > maxDistance) // wants to get closer to player
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);

            // move audio    
        }
        else if (distanceToPlayer < minDistance) // wants to get further from player
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -1 * movementSpeed * Time.deltaTime);

            // move audio
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

            // attack audio
        }
    }
}
