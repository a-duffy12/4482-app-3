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
    [SerializeField] private GameObject crosshair;

    [Header("Buttons and Sliders")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button returnToMenuButton;
    [SerializeField] private Slider fovSlider;
    [SerializeField] private Slider sensSlider;
    [SerializeField] private Button easyButton;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button funButton;

    [Header("Text")]
    [SerializeField] private Text fovText;
    [SerializeField] private Text sensText;
    [SerializeField] private Text difficultyText;

    [Header("Audio")]

    AudioSource uiSource;
    Camera eyes; // player camera
    PlayerInput input; // component that manages input actions

    private bool playing;

    private string easyText = "EASY";
    private string normalText = "NORMAL";
    private string hardText = "HARD";
    private string funText = "FUN";

    // Start is called before the first frame update
    void Start()
    {
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

        playing = true;

        eyes = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        uiSource = GameObject.FindGameObjectWithTag("UI").GetComponent<AudioSource>();
        uiSource.playOnAwake = false;
        uiSource.spatialBlend = 1f;
        uiSource.volume = 1f;
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

        // load saved settings

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

    void ReturnToMenu()
    {
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
