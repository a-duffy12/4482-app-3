using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSystem : MonoBehaviour
{
    [Header("Stats")]
    public float maxHp;

    [Header("GameObjects")]
    [SerializeField] private GameObject damageOverlay;
    [SerializeField] private Image healthBar;

    [Header("Audio")]
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
        playerSource.volume = 1f;
        playerSource.priority = 100;

        maxHp = Config.playerMaxHp;
        currentHp = maxHp;

        damageOverlay.SetActive(false);

        healthBar.fillAmount = Mathf.Clamp(currentHp/maxHp, 0, maxHp);
    }

    public void DamagePlayer(float damage, string attacker = "")
    {
        currentHp -= damage;
        healthBar.fillAmount = Mathf.Clamp(currentHp/maxHp, 0, maxHp);

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        else
        {
            StartCoroutine(DamagePlayerOverlay(0.1f)); // show damage overlay
        }
        
        if (currentHp <= 0)
        {
            playerSource.clip = deathAudio;
            playerSource.Play();

            if (attacker == Config.ogreName)
            {
                Debug.Log("wow you suck lmao");
            }
            else if (attacker == Config.demonName)
            {
                Debug.Log("welcome to hell buddy");
            }
            
            Time.timeScale = 0f;
        }
    }

    IEnumerator DamagePlayerOverlay(float duration)
    {
        damageOverlay.SetActive(true);
        
        yield return new WaitForSeconds(duration);

        damageOverlay.SetActive(false);
    }

    #region input functions

    #endregion input functions
}
