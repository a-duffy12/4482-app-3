using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSystem : MonoBehaviour
{
    [Header("Stats")]
    public float maxHp;

    [Header("Audio")]
    public AudioClip damageAudio;
    public AudioClip deathAudio;

    [HideInInspector] public float hp { get { return currentHp; } }
    private float currentHp;

    AudioSource playerSource;

    void Awake()
    {
        playerSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSource.playOnAwake = false;
        playerSource.spatialBlend = 1f;
        playerSource.volume = 0.8f;
        playerSource.priority = 100;

        maxHp = Config.playerMaxHp;
        currentHp = maxHp;
    }

    public void DamagePlayer(float damage, string attacker = "")
    {
        currentHp -= damage;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        
        if (currentHp <= 0)
        {
            // death audio
            if (attacker == Config.ogreName)
            {
                Debug.Log("wow you suck lmao");
            }
            
            Time.timeScale = 0f;
        }
        else
        {
            // damage audio
            Debug.Log("hp: " + currentHp);
        }
    }

    #region input functions

    #endregion input functions
}
