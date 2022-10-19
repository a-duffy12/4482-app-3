using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject hud;

    [Header("Buttons and Sliders")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button returnToMenuButton;
    [SerializeField] private Slider fovSlider;
    [SerializeField] private Slider sensSlider;
    [SerializeField] private Button easyButton;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button funButton;
    [SerializeField] private Button nsfwButton;
    [SerializeField] private Button redButton;
    [SerializeField] private Button greenButton;
    [SerializeField] private Button blueButton;
    [SerializeField] private Button purpleButton;

    [Header("Text")]
    [SerializeField] private Text fovText;
    [SerializeField] private Text sensText;
    [SerializeField] private Text difficultyText;
    [SerializeField] private Text nsfwText;
    [SerializeField] private Text crosshairColorText;

    [Header("Gameobjects")]
    [SerializeField] private Image crosshairDot;

    AudioSource uiSource;
    Camera eyes; // player camera
    PlayerInput input; // component that manages input actions

    private bool playing;

    private string easyText = "Difficulty - EASY";
    private string normalText = "Difficulty - NORMAL";
    private string hardText = "Difficulty - HARD";
    private string funText = "Difficulty - FUN";
    private string redText = "Crosshair Color - RED";
    private string greenText = "Crosshair Color - GREEN";
    private string blueText = "Crosshair Color - BLUE";
    private string purpleText = "Crosshair Color - PURPLE";

    void Awake()
    {
        // load saved settings
        Config.GetSaveData();

        eyes = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        uiSource = GameObject.FindGameObjectWithTag("UI").GetComponent<AudioSource>();
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        eyes.fieldOfView = Config.fieldOfView;

        Time.timeScale = 1f;

        pauseMenu.SetActive(false);
        hud.SetActive(true);

        // setup button listeners
        resumeButton.onClick.AddListener(ClosePauseMenu);
        returnToMenuButton.onClick.AddListener(ReturnToMenu);
        easyButton.onClick.AddListener(Easy);
        normalButton.onClick.AddListener(Normal);
        hardButton.onClick.AddListener(Hard);
        funButton.onClick.AddListener(Fun);
        nsfwButton.onClick.AddListener(Nsfw);
        redButton.onClick.AddListener(Red);
        greenButton.onClick.AddListener(Green);
        blueButton.onClick.AddListener(Blue);
        purpleButton.onClick.AddListener(Purple);

        uiSource.playOnAwake = false;
        uiSource.spatialBlend = 1f;
        uiSource.volume = 1f;

        // set settings values
        fovSlider.minValue = 60;
        fovSlider.maxValue = 110;
        fovSlider.value = Config.fieldOfView;
        sensSlider.minValue = 0.1f;
        sensSlider.maxValue = 100f;
        sensSlider.value = Config.sensitivity;

        // set settings text values
        fovText.text = fovSlider.value.ToString("0");
        sensText.text = (sensSlider.value/10).ToString("0.0");

        if (Config.difficultyLevel == Difficulty.Level.Easy)
        {
           difficultyText.text = easyText; 
        }
        else if (Config.difficultyLevel == Difficulty.Level.Normal)
        {
           difficultyText.text = normalText; 
        }
        else if (Config.difficultyLevel == Difficulty.Level.Hard)
        {
           difficultyText.text = hardText; 
        }
        else if (Config.difficultyLevel == Difficulty.Level.Fun)
        {
           difficultyText.text = funText; 
        }

        if (Config.nsfwEnabled)
        {
            nsfwText.text = "Enabled";
        }
        else
        {
            nsfwText.text = "Disabled";
        }

        if (Config.crosshairColor == "red")
        {
            crosshairColorText.text = redText;
            crosshairDot.color = new Color32(255, 0, 0, 255);
        }
        else if (Config.crosshairColor == "green")
        {
            crosshairColorText.text = greenText;
            crosshairDot.color = new Color32(0, 255, 0, 255);
        }
        else if (Config.crosshairColor == "blue")
        {
            crosshairColorText.text = blueText;
            crosshairDot.color = new Color32(0, 200, 255, 255);
        }
        else if (Config.crosshairColor == "purple")
        {
            crosshairColorText.text = purpleText;
            crosshairDot.color = new Color32(200, 0, 255, 255);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMenu.activeInHierarchy) // game is paused
        {
            fovText.text = fovSlider.value.ToString("0");
            sensText.text = (sensSlider.value/10).ToString("0.0");
            Config.fieldOfView = fovSlider.value;
            Config.sensitivity = sensSlider.value;
            eyes.fieldOfView = Config.fieldOfView;
        }
    }

    void Easy()
    {
        Config.difficultyLevel = Difficulty.Level.Easy;
        Config.difficultyMod = 0.2f;
        difficultyText.text = easyText;
    }

    void Normal()
    {
        Config.difficultyLevel = Difficulty.Level.Normal;
        Config.difficultyMod = 0.4f;
        difficultyText.text = normalText;
    }

    void Hard()
    {
        Config.difficultyLevel = Difficulty.Level.Hard;
        Config.difficultyMod = 0.6f;
        difficultyText.text = hardText;
    }

    void Fun()
    {
        Config.difficultyLevel = Difficulty.Level.Fun;
        Config.difficultyMod = 1f;
        difficultyText.text = funText;
    }

    void Nsfw()
    {
        if (Config.nsfwEnabled)
        {
            Config.nsfwEnabled = false;
            nsfwText.text = "Disabled";
        }
        else
        {
            Config.nsfwEnabled = true;
            nsfwText.text = "Enabled";
        }
    }

    void Red()
    {
        Config.crosshairColor = "red";
        crosshairColorText.text = redText;
        crosshairDot.color = new Color32(255, 0, 0, 255);
    }

    void Green()
    {
        Config.crosshairColor = "green";
        crosshairColorText.text = greenText;
        crosshairDot.color = new Color32(0, 255, 0, 255);
    }

    void Blue()
    {
        Config.crosshairColor = "blue";
        crosshairColorText.text = blueText;
        crosshairDot.color = new Color32(0, 200, 255, 255);
    }

    void Purple()
    {
        Config.crosshairColor = "purple";
        crosshairColorText.text = purpleText;
        crosshairDot.color = new Color32(200, 0, 255, 255);
    }

    void ReturnToMenu()
    {
        SaveLoad.SaveData();
        SceneManager.LoadScene("Menu");
    }

    void OpenPauseMenu()
    {
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        pauseMenu.SetActive(true);
    }

    void ClosePauseMenu()
    {
        SaveLoad.SaveData();
        pauseMenu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;
    }

    void OpenHUD()
    {
        hud.SetActive(true);
    }

    void CloseHUD()
    {
        hud.SetActive(false);
    }

    #region input functions

    public void TogglePauseMenu(InputAction.CallbackContext con)
    {
        if (con.performed && !pauseMenu.activeInHierarchy) // not paused
        {
            OpenPauseMenu();
        }
        else if (con.performed && pauseMenu.activeInHierarchy) // paused
        {
            ClosePauseMenu();
        }
    }

    public void ToggleHUD(InputAction.CallbackContext con)
    {
        if (con.performed && !hud.activeInHierarchy) // hud invisible
        {
            OpenHUD();
        }
        else if (con.performed && hud.activeInHierarchy) // hud visible
        {
            CloseHUD();
        }
    }

    #endregion input functions
}
