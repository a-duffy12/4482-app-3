using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject levelsMenu;

    [Header("Buttons and Sliders")]
    [SerializeField] private Button boomButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button levelsButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button returnToMenuOptionsButton;
    [SerializeField] private Button returnToMenuLevelsButton;
    [SerializeField] private Slider fovSlider;
    [SerializeField] private Slider sensSlider;
    [SerializeField] private Button easyButton;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button funButton;
    [SerializeField] private Button nsfwButton;
    [SerializeField] private Button fpsButton;
    [SerializeField] private Button redButton;
    [SerializeField] private Button greenButton;
    [SerializeField] private Button blueButton;
    [SerializeField] private Button purpleButton;
    [SerializeField] private List<Button> levelButtons;

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
    [SerializeField] private Text fpsText;
    [SerializeField] private Text fpsDisplayText;
    [SerializeField] private Text crosshairColorText;
    [SerializeField] private List<Text> levelTexts;

    [Header("Audio")]
    public AudioClip boomAudio;

    PlayerInput input; // component that manages input actions
    InputActionRebindingExtensions.RebindingOperation ro;

    private bool playing;
    private AudioSource source;

    private string easyText = "Difficulty - EASY";
    private string normalText = "Difficulty - NORMAL";
    private string hardText = "Difficulty - HARD";
    private string funText = "Difficulty - FUN";
    private string redText = "Crosshair Color - RED";
    private string greenText = "Crosshair Color - GREEN";
    private string blueText = "Crosshair Color - BLUE";
    private string purpleText = "Crosshair Color - PURPLE";

    private List<int> levelNumbers;

    void Awake()
    {
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        source = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // load saved settings
        Config.GetSaveData();
        LoadKeybinds(input);
        input.SwitchCurrentActionMap("Menu");

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        levelsMenu.SetActive(false);

        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.volume = 1f;

        // setup button listeners
        boomButton.onClick.AddListener(Boom);
        playButton.onClick.AddListener(Play);
        levelsButton.onClick.AddListener(Levels);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(Quit);
        returnToMenuOptionsButton.onClick.AddListener(ReturnToMenu);
        returnToMenuLevelsButton.onClick.AddListener(ReturnToMenu);
        easyButton.onClick.AddListener(Easy);
        normalButton.onClick.AddListener(Normal);
        hardButton.onClick.AddListener(Hard);
        funButton.onClick.AddListener(Fun);
        nsfwButton.onClick.AddListener(Nsfw);
        fpsButton.onClick.AddListener(Fps);
        redButton.onClick.AddListener(Red);
        greenButton.onClick.AddListener(Green);
        blueButton.onClick.AddListener(Blue);
        purpleButton.onClick.AddListener(Purple);

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
        }
        else if (Config.crosshairColor == "green")
        {
            crosshairColorText.text = greenText;
        }
        else if (Config.crosshairColor == "blue")
        {
            crosshairColorText.text = blueText;
        }
        else if (Config.crosshairColor == "purple")
        {
            crosshairColorText.text = purpleText;
        }

        // set fps text value
        if (Config.showFps)
        {
            fpsText.text = "Enabled";
        }
        else
        {
            fpsText.text = "Disabled";
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

        if (Config.levelNames.Count == levelButtons.Count && Config.levelNames.Count == levelTexts.Count)
        {
            for (int i = 0; i < Config.levelNames.Count; i++)
            {
                
                levelTexts[i].text = Config.levelNames[i];
            }

            levelButtons[0].onClick.AddListener(() => PlayLevel(0));
            levelButtons[1].onClick.AddListener(() => PlayLevel(1));
            levelButtons[2].onClick.AddListener(() => PlayLevel(2));
            levelButtons[3].onClick.AddListener(() => PlayLevel(3));
            levelButtons[4].onClick.AddListener(() => PlayLevel(4));
            levelButtons[5].onClick.AddListener(() => PlayLevel(5));
            levelButtons[6].onClick.AddListener(() => PlayLevel(6));
            levelButtons[7].onClick.AddListener(() => PlayLevel(7));
        }
        else
        {
            Debug.Log("Incorrect number of levels, level menu will not work!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (optionsMenu.activeInHierarchy) // game is paused
        {
            fovText.text = fovSlider.value.ToString("0");
            sensText.text = (sensSlider.value/10).ToString("0.0");
            Config.fieldOfView = fovSlider.value;
            Config.sensitivity = sensSlider.value;
        }
    }

    void Boom()
    {
        source.clip = boomAudio;
        source.Play();
    }

    void Play()
    {
        SceneManager.LoadScene(Config.levelNames[Config.levelCount]);
    }

    void Levels()
    {
        if (!levelsMenu.activeInHierarchy)
        {
            levelsMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
    }

    void OpenOptions()
    {
        if (!optionsMenu.activeInHierarchy)
        {
            optionsMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
    }

    void Quit()
    {
        SaveLoad.SaveData();
        Application.Quit();
    }

    void ReturnToMenu()
    {
        SaveLoad.SaveData();

        if (optionsMenu.activeInHierarchy)
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }

        if (levelsMenu.activeInHierarchy)
        {
            mainMenu.SetActive(true);
            levelsMenu.SetActive(false);
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
    }

    void Green()
    {
        Config.crosshairColor = "green";
        crosshairColorText.text = greenText;
    }

    void Blue()
    {
        Config.crosshairColor = "blue";
        crosshairColorText.text = blueText;
    }

    void Purple()
    {
        Config.crosshairColor = "purple";
        crosshairColorText.text = purpleText;
    }

    void Fps()
    {
        if (Config.showFps)
        {
            Config.showFps = false;
            fpsText.text = "Disabled";
        }
        else
        {
            Config.showFps = true;
            fpsText.text = "Enabled";
        }
    }

    void PlayLevel(int levelNumber)
    {
        SceneManager.LoadScene(Config.levelNames[levelNumber]);
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
}
