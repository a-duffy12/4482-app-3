using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [Header("Stats")]
    public int levelId;
    [HideInInspector] public string levelName;

    [Header("Gameobjects")]
    [SerializeField] private GameObject introPanel;
    [SerializeField] private Button continueButton;
    [SerializeField] private Text levelTitle;

    [Header("Audio")]
    public AudioClip boomAudio;

    Scene scene;
    PlayerInput input;
    AudioSource levelSource;

    [HideInInspector] public bool checkEnemies; // called on enemy death to signal to check if level is won
    private float nextCheckTime;
    private int checkCounter;
    
    void Awake()
    {
        scene = SceneManager.GetActiveScene();
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        levelSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    void Start()
    {
        Config.GetSaveData();
        
        levelName = scene.name;
        levelTitle.text = levelName;

        Time.timeScale = 0f;
        input.SwitchCurrentActionMap("Menu");

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        introPanel.SetActive(true);
        SetupLevel(levelId);

        levelSource.playOnAwake = false;
        levelSource.spatialBlend = 1f;
        levelSource.volume = 1f;

        continueButton.onClick.AddListener(ContinueToLevel);
    }

    void Update()
    {
        if (checkEnemies)
        {
            checkEnemies = false;
            nextCheckTime = Time.time + 0.5f;
            checkCounter++;
        }

        if (Time.time >= nextCheckTime && checkCounter > 0)
        {
            checkCounter--;
            CheckEnemiesPresent();
        }
    }

    void ContinueToLevel()
    {
        SaveLoad.SaveData();

        Time.timeScale = 1f;
        input.SwitchCurrentActionMap("Player");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        introPanel.SetActive(false);

        CheckEnemiesPresent();
    }

    void SetupLevel(int levelId)
    {
        switch (levelId)
        {
            case 0:
                Config.assaultRifleUnlocked = true;
                Config.shotgunUnlocked = false;
                Config.dashUnlocked = false;
                Config.knifeUnlocked = false;
                Config.sniperUnlocked = false;
                Config.grenadeUnlocked = false;
                Config.rewindUnlocked = false;
                Config.flamethrowerUnlocked = false;
                break;

            case 1:
                Config.assaultRifleUnlocked = true;
                Config.shotgunUnlocked = true;
                Config.dashUnlocked = false;
                Config.knifeUnlocked = false;
                Config.sniperUnlocked = false;
                Config.grenadeUnlocked = false;
                Config.rewindUnlocked = false;
                Config.flamethrowerUnlocked = false;
                break;
                
            case 2:
                Config.assaultRifleUnlocked = true;
                Config.shotgunUnlocked = true;
                Config.dashUnlocked = true;
                Config.knifeUnlocked = false;
                Config.sniperUnlocked = false;
                Config.grenadeUnlocked = false;
                Config.rewindUnlocked = false;
                Config.flamethrowerUnlocked = false;
                break;

            case 3:
                Config.assaultRifleUnlocked = true;
                Config.shotgunUnlocked = true;
                Config.dashUnlocked = true;
                Config.knifeUnlocked = true;
                Config.sniperUnlocked = false;
                Config.grenadeUnlocked = false;
                Config.rewindUnlocked = false;
                Config.flamethrowerUnlocked = false;
                break;

            case 4:
                Config.assaultRifleUnlocked = true;
                Config.shotgunUnlocked = true;
                Config.dashUnlocked = true;
                Config.knifeUnlocked = true;
                Config.sniperUnlocked = true;
                Config.grenadeUnlocked = false;
                Config.rewindUnlocked = false;
                Config.flamethrowerUnlocked = false;
                break;

            case 5:
                Config.assaultRifleUnlocked = true;
                Config.shotgunUnlocked = true;
                Config.dashUnlocked = true;
                Config.knifeUnlocked = true;
                Config.sniperUnlocked = true;
                Config.grenadeUnlocked = true;
                Config.rewindUnlocked = false;
                Config.flamethrowerUnlocked = false;
                break;

            case 6:
                Config.assaultRifleUnlocked = true;
                Config.shotgunUnlocked = true;
                Config.dashUnlocked = true;
                Config.knifeUnlocked = true;
                Config.sniperUnlocked = true;
                Config.grenadeUnlocked = true;
                Config.rewindUnlocked = true;
                Config.flamethrowerUnlocked = false;
                break;

            default:
                Config.assaultRifleUnlocked = true;
                Config.shotgunUnlocked = true;
                Config.dashUnlocked = true;
                Config.knifeUnlocked = true;
                Config.sniperUnlocked = true;
                Config.grenadeUnlocked = true;
                Config.rewindUnlocked = true;
                Config.flamethrowerUnlocked = true;
                break;
        }

        SaveLoad.SaveData();
    }

    void CheckEnemiesPresent()
    {
        var objs = FindObjectsOfType<GameObject>();
        List<GameObject> enemies = new List<GameObject>();

        foreach (var obj in objs)
        {
            if (obj.layer == 8) // layer 8 is Enemy
            {
                enemies.Add(obj);
            }
        }

        if (!enemies.Any()) // no more enemies left
        {
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator NextLevel()
    {
        if (levelId >= Config.levelCount) // only update level counter if it is the first time playing the level
        {
            Config.levelCount = levelId + 1;
            SaveLoad.SaveData();
        }

        levelSource.clip = boomAudio;
        levelSource.Play();

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(Config.levelNames[Config.levelCount]);
    }
}
