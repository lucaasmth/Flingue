using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int startSceneNumber;
    public GameObject mainPanel;
    public GameObject levelsPanel;
    public GameObject randomLevelPanel;
    public GameObject optionsPanel;

    public Slider enemiesCountSlider;
    public TextMeshProUGUI enemiesCountText;

    public AudioMixer audioMixer;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    public void Start()
    {
        mainPanel.SetActive(true);
        levelsPanel.SetActive(false);
        randomLevelPanel.SetActive(false);
        optionsPanel.SetActive(false);
        
        enemiesCountSlider.value = GlobalVariables.GetInt("RANDOM_ENEMIES_COUNT");
        enemiesCountText.text = Mathf.FloorToInt(enemiesCountSlider.value).ToString();
    }

    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);
    }

    public void LoadRandomLevel()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

    public void ShowLevelsPanel()
    {
        mainPanel.SetActive(false);
        levelsPanel.SetActive(true);
        randomLevelPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        levelsPanel.SetActive(false);
        randomLevelPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    public void ShowRandomLevelPanel()
    {
        mainPanel.SetActive(false);
        levelsPanel.SetActive(false);
        randomLevelPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void ShowOptionsPanel()
    {
        mainPanel.SetActive(false);
        levelsPanel.SetActive(false);
        randomLevelPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void ChangeEnemiesCount()
    {
        int value = Mathf.FloorToInt(enemiesCountSlider.value);
        enemiesCountText.text = value.ToString();
        GlobalVariables.SetInt("RANDOM_ENEMIES_COUNT", value);
    }

    public void ChangeMusicVolume()
    {
        audioMixer.SetFloat("MusicVolume", musicVolumeSlider.value);
    }
    
    public void ChangeSfxVolume()
    {
        audioMixer.SetFloat("SFXVolume", sfxVolumeSlider.value);
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }
}
