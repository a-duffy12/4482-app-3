using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSystem : MonoBehaviour
{
    [Header("Stats")]
    public float maxHp;

    [Header("GameObjects")]
    [SerializeField] private GameObject damageOverlay;
    [SerializeField] private Image healthBar;
    [SerializeField] private Text deathText;

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
        deathText.gameObject.SetActive(false);

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

            int deathTime = (int)Time.time % 10;

            if (attacker == Config.ogreName)
            {
                if (deathTime >= 0 && deathTime < 5)
                {
                    deathText.text = "Nom nom nom";
                }
                else if (deathTime >= 5 && deathTime < 9)
                {
                    deathText.text = "One eye > two eyes";
                }
                else if (deathTime == 9)
                {
                    deathText.text = "Shrek sends his regards";
                }
            }
            else if (attacker == Config.demonName)
            {
                if (deathTime >= 0 && deathTime < 5)
                {
                    deathText.text = "Crispy, just the way we like it";
                }
                else if (deathTime >= 5 && deathTime < 9)
                {
                    deathText.text = "NO SCOPED!";
                }
                else if (deathTime == 9)
                {
                    deathText.text = "Welcome to hell buddy";
                }
            }
            else if (attacker == Config.soulName)
            {
                if (deathTime >= 0 && deathTime < 5)
                {
                    deathText.text = "They all fall down...";
                }
                else if (deathTime >= 5 && deathTime < 9)
                {
                    deathText.text = "So glad you could join us...";
                }
                else if (deathTime == 9)
                {
                    deathText.text = "One of us! One of us!";
                }
            }
            else if (attacker == Config.ratName)
            {
                if (deathTime >= 0 && deathTime < 5)
                {
                    deathText.text = "You need dodgeball practice";
                }
                else if (deathTime >= 5 && deathTime < 9)
                {
                    deathText.text = "REEEEETT";
                }
                else if (deathTime == 9)
                {
                    deathText.text = "That's for kidnapping Stuart Little";
                }
            }
            else if (attacker == Config.voidName)
            {
                if (deathTime >= 0 && deathTime < 5)
                {
                    deathText.text = "Bye bye!";
                }
                else if (deathTime >= 5 && deathTime < 9)
                {
                    deathText.text = "Help I've fallen but I cannot get up";
                }
                else if (deathTime == 9)
                {
                    deathText.text = "You can't quit the game that way";
                }
            }

            deathText.gameObject.SetActive(true);
            
            Time.timeScale = 0.0001f;
            StartCoroutine(KillPlayer());
        }
    }

    IEnumerator DamagePlayerOverlay(float duration)
    {
        damageOverlay.SetActive(true);
        
        yield return new WaitForSeconds(duration);

        damageOverlay.SetActive(false);
    }

    IEnumerator KillPlayer()
    {
        yield return new WaitForSeconds(0.0005f);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    #region input functions

    #endregion input functions
}
