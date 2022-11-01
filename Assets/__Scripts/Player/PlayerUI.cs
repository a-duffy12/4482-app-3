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

    [Header("Keybinds")]
    [SerializeField] private InputActionReference forwardAction;
    [SerializeField] private Button forwardButton;
    [SerializeField] private Text forwardText;
    [SerializeField] private InputActionReference backwardAction;
    [SerializeField] private Button backwardButton;
    [SerializeField] private Text backwardText;
    [SerializeField] private InputActionReference leftAction;
    [SerializeField] private Button leftButton;
    [SerializeField] private Text leftText;
    [SerializeField] private InputActionReference rightAction;
    [SerializeField] private Button rightButton;
    [SerializeField] private Text rightText;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Text jumpText;
    [SerializeField] private InputActionReference crouchAction;
    [SerializeField] private Button crouchButton;
    [SerializeField] private Text crouchText;
    [SerializeField] private InputActionReference dashAction;
    [SerializeField] private Button dashButton;
    [SerializeField] private Text dashText;
    [SerializeField] private InputActionReference rewindAction;
    [SerializeField] private Button rewindButton;
    [SerializeField] private Text rewindText;
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private Button shootButton;
    [SerializeField] private Text shootText;
    [SerializeField] private InputActionReference scopeAction;
    [SerializeField] private Button scopeButton;
    [SerializeField] private Text scopeText;
    [SerializeField] private InputActionReference assaultRifleAction;
    [SerializeField] private Button assaultRifleButton;
    [SerializeField] private Text assaultRifleText;
    [SerializeField] private InputActionReference shotgunAction;
    [SerializeField] private Button shotgunButton;
    [SerializeField] private Text shotgunText;
    [SerializeField] private InputActionReference sniperAction;
    [SerializeField] private Button sniperButton;
    [SerializeField] private Text sniperText;
    [SerializeField] private InputActionReference flamethrowerAction;
    [SerializeField] private Button flamethrowerButton;
    [SerializeField] private Text flamethrowerText;
    [SerializeField] private InputActionReference knifeAction;
    [SerializeField] private Button knifeButton;
    [SerializeField] private Text knifeText;
    [SerializeField] private InputActionReference grenadeAction;
    [SerializeField] private Button grenadeButton;
    [SerializeField] private Text grenadeText;
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Text pauseText;
    [SerializeField] private InputActionReference toggleHudAction;
    [SerializeField] private Button toggleHudButton;
    [SerializeField] private Text toggleHudText;

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
    InputActionRebindingExtensions.RebindingOperation ro;

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
        eyes = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        uiSource = GameObject.FindGameObjectWithTag("UI").GetComponent<AudioSource>();
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // load saved settings
        Config.GetSaveData();
        LoadKeybinds(input);

        eyes.fieldOfView = Config.fieldOfView;

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

        // set difficulty text value
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

        // set nsfw text value
        if (Config.nsfwEnabled)
        {
            nsfwText.text = "Enabled";
        }
        else
        {
            nsfwText.text = "Disabled";
        }

        // set crosshair color value
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

        // set keybindings text values
        SetBindText(forwardAction, forwardText);
        SetBindText(backwardAction, backwardText);
        SetBindText(leftAction, leftText);
        SetBindText(rightAction, rightText);
        SetBindText(jumpAction, jumpText);
        SetBindText(crouchAction, crouchText);
        SetBindText(dashAction, dashText);
        SetBindText(rewindAction, rewindText);
        SetBindText(shootAction, shootText);
        SetBindText(scopeAction, scopeText);
        SetBindText(assaultRifleAction, assaultRifleText);
        SetBindText(shotgunAction, shotgunText);
        SetBindText(sniperAction, sniperText);
        SetBindText(flamethrowerAction, flamethrowerText);
        SetBindText(knifeAction, knifeText);
        SetBindText(grenadeAction, grenadeText);
        SetBindText(pauseAction, pauseText);
        SetBindText(toggleHudAction, toggleHudText);

        // set button listeners for keybinds
        forwardButton.onClick.AddListener(() => { Rebind(forwardAction, forwardText); });
        backwardButton.onClick.AddListener(() => { Rebind(backwardAction, backwardText); });
        leftButton.onClick.AddListener(() => { Rebind(leftAction, leftText); });
        rightButton.onClick.AddListener(() => { Rebind(rightAction, rightText); });
        jumpButton.onClick.AddListener(() => { Rebind(jumpAction, jumpText); });
        crouchButton.onClick.AddListener(() => { Rebind(crouchAction, crouchText); });
        dashButton.onClick.AddListener(() => { Rebind(dashAction, dashText); });
        rewindButton.onClick.AddListener(() => { Rebind(rewindAction, rewindText); });
        shootButton.onClick.AddListener(() => { Rebind(shootAction, shootText); });
        scopeButton.onClick.AddListener(() => { Rebind(scopeAction, scopeText); });
        assaultRifleButton.onClick.AddListener(() => { Rebind(assaultRifleAction, assaultRifleText); });
        shotgunButton.onClick.AddListener(() => { Rebind(shotgunAction, shotgunText); });
        sniperButton.onClick.AddListener(() => { Rebind(sniperAction, sniperText); });
        flamethrowerButton.onClick.AddListener(() => { Rebind(flamethrowerAction, flamethrowerText); });
        knifeButton.onClick.AddListener(() => { Rebind(knifeAction, knifeText); });
        grenadeButton.onClick.AddListener(() => { Rebind(grenadeAction, grenadeText); });
        pauseButton.onClick.AddListener(() => { Rebind(pauseAction, pauseText); });
        toggleHudButton.onClick.AddListener(() => { Rebind(toggleHudAction, toggleHudText); });
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
        input.SwitchCurrentActionMap("Menu");

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

        input.SwitchCurrentActionMap("Player");
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

    void SetBindText(InputActionReference action, Text text)
    {
        int bindIndex = action.action.GetBindingIndexForControl(action.action.controls[0]); // get index of bind passed in
        text.text = InputControlPath.ToHumanReadableString(action.action.bindings[bindIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice); // set text to new value
    }

    public void Rebind(InputActionReference action, Text text)
    {
        action.action.Disable(); // disable action momentarily
        text.text = "- - -";

        ro = action.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(action, text))
            .Start();
    }

    private void RebindComplete(InputActionReference action, Text text)
    {
        ro.Dispose();
        SetBindText(action, text);
        SaveKeybinds(input);
        action.action.Enable(); // re-enable action
    }

    void SaveKeybinds(PlayerInput input)
    {
        Config.keybinds = input.actions.SaveBindingOverridesAsJson();
    }

    void LoadKeybinds(PlayerInput input)
    {
        input.actions.LoadBindingOverridesFromJson(Config.keybinds);
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
