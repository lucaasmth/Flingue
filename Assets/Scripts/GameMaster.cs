using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public Shader standardShader;
    public TextMeshProUGUI statusText;
    public GameObject pauseMenu;
    public GameObject endGamePanel;
    public TextMeshProUGUI endGameTimeText;
    public TextMeshProUGUI enemiesText;
    public bool spawnEnemies = false;
    public GameObject enemyPrefab;
    
    public bool IsPaused { get; private set; }

    private GameObject _player;
    private Health _playerHealth;
    private List<Health> _enemiesHealth;
    private Timer _timer;
    
    public bool IsGameFinished { get; private set; }
    private bool _enemiesInit = false;

    private void Start()
    {
        pauseMenu.SetActive(false);
        statusText.text = "";
        if (spawnEnemies)
        {
            StartCoroutine(nameof(SpawnEnemiesDelayed), GlobalVariables.GetInt("RANDOM_ENEMIES_COUNT"));
        }
        else
        {
            InitEnemies();
        }
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerHealth = _player.GetComponent<Health>();
        if(_playerHealth == null) Debug.LogError("No player health");
        _timer = GetComponent<Timer>();
    }

    private void InitEnemies()
    {
        _enemiesHealth = new List<Health>();
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            _enemiesHealth.Add(enemy.GetComponent<Health>());
        }
        _enemiesInit = true;
    }

    private void Update()
    {
        if (!_enemiesInit) return;
        
        enemiesText.text = string.Format("Enemies:" + GetAliveEnemiesCount());
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused) UnpauseGame();
            else PauseGame();
        }
        
        if (!IsGameFinished && AllEnemiesAreDead())
        {
            IsGameFinished = true;
            _timer.StopTimer();
            foreach (GameObject floor in GameObject.FindGameObjectsWithTag("Floor"))
            {
                floor.GetComponent<Renderer>().material.shader = standardShader;
            }

            float elapsedTime = _timer.ElapsedTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            
            endGamePanel.SetActive(true);
            endGameTimeText.text = $"Time: {minutes}:{seconds}";
        }

        if (!IsGameFinished && _playerHealth.IsDead)
        {
            _timer.StopTimer();
            IsGameFinished = true;
            statusText.text = "You died.\nPress Space to restart";
        }
        
        if (IsGameFinished && Input.GetKeyDown(KeyCode.Space))
            ReloadScene();
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private int GetAliveEnemiesCount()
    {
        return _enemiesHealth.Count(health => !health.IsDead);
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

    private IEnumerator SpawnEnemiesDelayed(int count)
    {
        yield return new WaitForSeconds(.1f);
        SpawnEnemies(count);
        InitEnemies();
    }
    
    private void SpawnEnemies(int count)
    {
        List<Transform> spawns = new List<Transform>();
        foreach (var spawn in GameObject.FindGameObjectsWithTag("Spawn"))
        {
            spawns.Add(spawn.transform);
        }
        
        for (int i = 0; i < count; i++)
        {
            Transform spawn = spawns[Random.Range(0, spawns.Count)];
            Vector3 scale = spawn.localScale;
            float halfOfX = scale.x / 2f;
            float halfOfZ = scale.z / 2f;
            float x = Random.Range(-halfOfX, halfOfX);
            float z = Random.Range(-halfOfZ, halfOfZ);
            Vector3 spawnPos = new Vector3(x, 1, z);
            Instantiate(enemyPrefab, spawnPos + spawn.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
    }
}
