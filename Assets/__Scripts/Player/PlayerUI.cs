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

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button returnToMenuButton;

    [Header("Text")]

    [Header("Audio")]

    AudioSource uiSource;

    private bool playing;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;

        pauseMenu.SetActive(false);
        hud.SetActive(true);

        // setup button listeners
        resumeButton.onClick.AddListener(ClosePauseMenu);
        returnToMenuButton.onClick.AddListener(ReturnToMenu);

        playing = true;

        uiSource = GetComponentInChildren<AudioSource>();
        uiSource.playOnAwake = false;
        uiSource.spatialBlend = 1f;
        uiSource.volume = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            
        }
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

        crosshair.SetActive(false);
        pauseMenu.SetActive(true);
    }

    void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        crosshair.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;
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

    #endregion input functions
}
