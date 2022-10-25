using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    [Header("Audio")]
    public AudioClip hurtAudio;
    public AudioClip moveAudio;
    public AudioClip attackAudio;

    [Header("GameObjects")]
    public GameObject mudballPrefab;
    public Transform firePoint;
    public GameObject fireParticle;

    float movementSpeed;
    float aggroDistance;
    float minDistance;
    float maxDistance;
    float damage;
    float attackRate;
    float mudballSpeed;

    private float lastAttackTime;
    private bool undergound;

    void Start()
    {
        enemySource.playOnAwake = false;
        enemySource.spatialBlend = 1f;
        enemySource.volume = 1f;
        enemySource.priority = 120;

        maxHp = Config.ratMaxHp;
        currentHp = maxHp;

        enemyName = Config.ratName;
        deathPosition = Config.ratDeathPosition;
        movementSpeed = Config.ratMovementSpeed;
        aggroDistance = Config.ratAggroDistance;
        minDistance = Config.ratMinDistance;
        maxDistance = Config.ratMaxDistance;
        damage = Config.ratDamage;
        attackRate = Config.ratAttackRate;
        mudballSpeed = Config.ratMudballSpeed;
    }
    
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        transform.LookAt(player.transform.position);

        //Attack();

        HandleDamageAudio();
        HandleStun();
        HandleBurn();
    }

    void Attack()
    {
        if (Time.time > lastAttackTime + (1/attackRate))
        {
            GameObject mudballObject = Instantiate(mudballPrefab, firePoint.position, firePoint.rotation);
            RatMudball mudball = mudballObject.GetComponent<RatMudball>();
            mudball.Shoot(firePoint.transform.forward, mudballSpeed);

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
