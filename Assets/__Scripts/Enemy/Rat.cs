using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    [Header("Audio")]
    public AudioClip hurtAudio;
    public AudioClip moveAudio;
    public AudioClip attackAudio;
    public AudioClip digAudio;
    public AudioClip tunnelAudio;
    public AudioClip resurfaceAudio;

    [Header("GameObjects")]
    public GameObject mudballPrefab;
    public Transform firePoint;
    public ParticleSystem fireParticle;

    float movementSpeed;
    float aggroDistance;
    float minDistance;
    float maxDistance;
    float damage;
    float attackRate;
    float mudballSpeed;
    float surfaceTime;

    private float lastAttackTime;
    private float lastTunnelTime;
    private bool undergound;
    private int tunnelCounter;

    void Start()
    {
        fireParticle.Stop();

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
        surfaceTime = Config.ratSurfaceTime;
    }
    
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        transform.LookAt(player.transform.position);

        if (distanceToPlayer <= (aggroDistance * Config.enemyAggroRadiusModifier)) // only interact with player if they are within range
        {
            if (Time.time - surfaceTime > lastTunnelTime && !undergound && !stunned) // rat has been above ground for too long
            {
                StartTunnel();
            }
            else if (distanceToPlayer <= minDistance && !undergound && !stunned) // rat wants to get away from player
            {
                StartTunnel();
            }
            else if (distanceToPlayer >= maxDistance && !undergound && !stunned) // rat wants to get in range of player
            {
                Walk();
            }
            else if (!undergound && !stunned) // rat wants to attack
            {
                Attack();
            }
        }

        if (undergound && !enemySource.isPlaying && rb.velocity.magnitude > 0.1f)
        {
            enemySource.clip = tunnelAudio;
            enemySource.Play();
        }

        HandleDamageAudio();
        HandleStun();
        HandleBurn();
    }

    void FixedUpdate()
    {
        if (undergound)
        {
            if (tunnelCounter < 20) // down for 400ms
            {
                tunnelCounter++;
                transform.position -= (transform.up * 0.15f);
            }

            if (tunnelCounter == 20) // tunnel around
            {
                tunnelCounter = 0;
                StartCoroutine(TunnelPort());
            }
        }
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

    void Walk()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);

        if (!enemySource.isPlaying && rb.velocity.magnitude > 0.1f)
        {
            enemySource.clip = moveAudio;
            enemySource.Play();
        }
    }

    void StartTunnel()
    {
        if (!undergound)
        {
            undergound = true;
            rb.useGravity = false;
            rb.isKinematic = true;
            tunnelCounter = 0;
            lastTunnelTime = Time.time;

            enemySource.clip = digAudio;
            enemySource.Play();
        }        
    }

    IEnumerator TunnelPort()
    {
        Vector3 adjustment = (player.transform.forward * Random.Range(-7, -12)) + (player.transform.right * Random.Range(-3, 3));
        adjustment.y = 2.0f;
        transform.position = player.transform.position + adjustment;

        yield return new WaitForSeconds(Config.ratTunnelTime);

        enemySource.clip = resurfaceAudio;
        enemySource.Play();

        undergound = false;
        rb.useGravity = true;
        rb.isKinematic = false;
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
