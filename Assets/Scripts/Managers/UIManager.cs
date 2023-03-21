using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Button")]
    public Button startButton;
    public Button settingsButton;
    public Button quitButton;
    public Button returnToMenuButton;
    public Button returnToGameButton;

    [Header("Menu")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;

    [Header("Text")]
    public Text missilesText;
    public Text volSliderText;
    public Text musicSliderText;
    public Text sfxSliderText;
    public Image healthBar;

    [Header("Slider")]
    public Slider volSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioClip pauseSound;

    void StartGame()
    {
        SceneManager.LoadScene("Level");
    }

    void ShowSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);

        if(volSlider && volSliderText)
        {
            float value;
            audioMixer.GetFloat("MasterVol", out value);
            volSlider.value = value + 80;
            volSliderText.text = (value + 80).ToString();
        }

        if(musicSlider && musicSliderText)
        {
            float value;
            audioMixer.GetFloat("MusicVol", out value);
            musicSlider.value = value + 80;
            musicSliderText.text = (value + 80).ToString();
        }
        
        if(sfxSlider && sfxSliderText)
        {
            float value;
            audioMixer.GetFloat("SFXVol", out value);
            sfxSlider.value = value + 80;
            sfxSliderText.text = (value + 80).ToString();
        }
    }

    void ShowMainMenu()
    {
        if (SceneManager.GetActiveScene().name == "Level")
        {
            SceneManager.LoadScene("Title");
        }
        else
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    void OnVolValueChanged(float value)
    {
        if (volSliderText)
        {
            volSliderText.text = value.ToString();
            audioMixer.SetFloat("MasterVol", value - 80);
        }
    }

    void OnMusicValueChanged(float value)
    {
        if (musicSliderText)
        {
            musicSliderText.text = value.ToString();
            audioMixer.SetFloat("MusicVol", value - 80);
        }
    }
    void OnSFXValueChanged(float value)
    {
        if (sfxSliderText)
        {
            sfxSliderText.text = value.ToString();
            audioMixer.SetFloat("SFXVol", value - 80);
        }
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    void UpdateLifeText(int value)
    {
        healthBar.fillAmount = (float)GameManager.instance.lives / (float)GameManager.instance.maxLives;
    }

    void UpdateMissileText(int value)
    {
        missilesText.text = value.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (startButton)
        {
            startButton.onClick.AddListener(StartGame);
        }
        if (settingsButton)
        {
            settingsButton.onClick.AddListener(ShowSettingsMenu);
        }
        if (quitButton)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        if (volSlider)
        {
            volSlider.onValueChanged.AddListener(OnVolValueChanged);
        }
        if (musicSlider)
        {
            musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
        }
        if (sfxSlider)
        {
            sfxSlider.onValueChanged.AddListener(OnSFXValueChanged);
        }
        if (returnToGameButton)
        {
            returnToGameButton.onClick.AddListener(ResumeGame);
        }
        if (returnToMenuButton)
        {
            returnToMenuButton.onClick.AddListener(ShowMainMenu);
        }
        if (healthBar)
        {
            GameManager.instance.onLifeValueChanged.AddListener(UpdateLifeText);
        }
        if (missilesText)
        {
            GameManager.instance.onMissileValueChanged.AddListener(UpdateMissileText);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu)
        {
            Time.timeScale = 1;
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (pauseMenu.activeSelf)
            {
                GameManager.instance.playerInstance.GetComponent<AudioSourceManager>().PlayOneShot(pauseSound, false);
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}
