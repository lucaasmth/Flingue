using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameMaster : MonoBehaviour
{
    public Shader standardShader;
    public TextMeshProUGUI statusText;
    public GameObject pauseMenu;
    
    public bool IsPaused { get; private set; }

    private GameObject _player;
    private Health _playerHealth;
    private List<Health> _enemiesHealth;
    
    private bool _isGameFinished;

    private void Start()
    {
        pauseMenu.SetActive(false);
        statusText.text = "";
        _enemiesHealth = new List<Health>();
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            _enemiesHealth.Add(enemy.GetComponent<Health>());
        }
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerHealth = _player.GetComponent<Health>();
        if(_playerHealth == null) Debug.LogError("No player health");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused) UnpauseGame();
            else PauseGame();
        }
        
        if (!_isGameFinished && AllEnemiesAreDead())
        {
            _isGameFinished = true;
            GameObject.FindGameObjectWithTag("Floor").GetComponent<Renderer>().material.shader = standardShader;
            statusText.text = "You won!\nPress Space to restart";
        }

        if (!_isGameFinished && _playerHealth.IsDead)
        {
            _isGameFinished = true;
            statusText.text = "You died.\nPress Space to restart";
        }
        
        if (_isGameFinished && Input.GetKeyDown(KeyCode.Space))
            ReloadScene();
    }

    private static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private bool AllEnemiesAreDead()
    {
        return _enemiesHealth.All(health => health.IsDead);
    }
    
    private void PauseGame()
    {
        IsPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        IsPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        UnpauseGame();
        SceneManager.LoadScene(0);
    }
}
