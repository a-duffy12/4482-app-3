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
    float attackRate;

    private float lastAttackTime;
    PlayerSystem system;

    void Start()
    {
        system = player.GetComponent<PlayerSystem>();

        maxHp = Config.ogreMaxHp;
        currentHp = maxHp;

        enemyName = Config.ogreName;
        movementSpeed = Config.ogreMovementSpeed;
        aggroDistance = Config.ogreAggroDistance;
        attackDistance = Config.ogreAttackDistance;
        damage = Config.ogreDamage;
        attackRate = Config.ogreAttackRate;
    }

    void Update()
    {
        float distancetoplayer = Vector3.Distance(player.transform.position, transform.position);

        transform.LookAt(player.transform.position);

        if (distancetoplayer <= (aggroDistance * Config.enemyAggroRadiusModifier) && distancetoplayer > (attackDistance - 0.5f) && Time.time > lastAttackTime + (1/attackRate)) // only move towards player if within aggro range, not too close, and is ready to attack
        {
            Move();
        }

        if (distancetoplayer <= attackDistance)
        {
            Attack(distancetoplayer);
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);

        // move audio
    }

    void Attack(float distancetoplayer)
    {
        if (Time.time > lastAttackTime + (1/attackRate))
        {
            if (distancetoplayer <= attackDistance)
            {
                system.DamagePlayer(damage * Config.difficultyMod, enemyName);
            }

            lastAttackTime = Time.time;

            // attack audio
        }
    }
}
