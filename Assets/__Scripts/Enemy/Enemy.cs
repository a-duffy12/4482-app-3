using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public string enemyName;
    public float maxHp;
    public float deathPosition;
    public bool invulnerable;
    public bool nonflammable;
    [HideInInspector] public bool onFire;

    [Header("Gameobjects")]
    public Image healthBar;
    public Canvas healthBarCanvas;
    public ParticleSystem deathParticle;

    [HideInInspector] public float hp { get { return currentHp; } }
    [HideInInspector] public float currentHp;
    [HideInInspector] public bool damaged;

    [HideInInspector] public GameObject player;
    [HideInInspector] public AudioSource enemySource;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public PlayerSystem playerSystem;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemySource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        healthBar.fillAmount = Mathf.Clamp(currentHp/maxHp, 0, maxHp);
    }

    void Update()
    {
        // make health bar canvas always face the player
        Vector3 healthBarFaceDirection = player.transform.position - healthBarCanvas.transform.position;
        healthBarFaceDirection.x = healthBarFaceDirection.z = 0f;
        healthBarCanvas.transform.LookAt(player.transform.position - healthBarFaceDirection);
        healthBarCanvas.transform.Rotate(0, 180, 0);
    }

    public void DamageEnemy(float damage, string blow = "")
    {
        currentHp -= damage;
        healthBar.fillAmount = Mathf.Clamp(currentHp/maxHp, 0, maxHp);
        
        if (currentHp <= 0)
        {
            playerSystem = player.GetComponent<PlayerSystem>();
            
            if (blow == "assault_rifle")
            {
                playerSystem.DamagePlayer(Math.Min(Config.assaultRifleHpReturnMod * maxHp, Config.assaultRifleMaxHpReturn));
            }
            else if (blow == "shotgun")
            {
                playerSystem.DamagePlayer(Math.Min(Config.shotgunHpReturnMod * maxHp, Config.shotgunHpMaxReturn));
            }
            else if (blow == "sniper")
            {
                playerSystem.DamagePlayer(Math.Min(Config.sniperRifleHpReturnMod * maxHp, Config.sniperRifleMaxHpReturn));
            }
            else if (blow == "flamethrower")
            {
                playerSystem.DamagePlayer(Math.Min(Config.flamethrowerRifleHpReturnMod * maxHp, Config.flamethrowerRifleMaxHpReturn));
            }

            if (Config.nsfwEnabled) // only show gore if nsfw is enabled
            {
                Instantiate(deathParticle, transform.position + new Vector3(0, deathPosition, 0), transform.rotation);
            }
            
            Destroy(gameObject);
        }
        else
        {
            damaged = true;
        }
    }
}
