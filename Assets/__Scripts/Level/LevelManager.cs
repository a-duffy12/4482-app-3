using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [Header("Stats")]
    public int levelId;
    [HideInInspector] public string levelName;

    [Header("Gameobjects")]
    [SerializeField] private GameObject introPanel;
    [SerializeField] private Button continueButton;

    Scene scene;
    PlayerInput input;
    
    void Awake()
    {
        scene = SceneManager.GetActiveScene();
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    void Start()
    {
        Config.GetSaveData();

        Config.levelCount = levelId;
        levelName = scene.name;
        Debug.Log("level count = "  + Config.levelCount.ToString());

        Time.timeScale = 0f;
        input.SwitchCurrentActionMap("Menu");

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        introPanel.SetActive(true);
        SetupLevel(levelId);

        continueButton.onClick.AddListener(ContinueToLevel);
    }

    void Update()
    {
        // when all enemies are dead, start coroutine
    }

    void ContinueToLevel()
    {
        SaveLoad.SaveData();

        Time.timeScale = 1f;
        input.SwitchCurrentActionMap("Player");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        introPanel.SetActive(false);
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
    }

    IEnumerator NextLevel()
    {
        return null;
    }
}
