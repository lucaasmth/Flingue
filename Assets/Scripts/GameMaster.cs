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
    
    private GameObject _player;
    private Health _playerHealth;
    private List<Health> _enemiesHealth;
    
    private bool _isGameFinished;

    private void Start()
    {
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

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private bool AllEnemiesAreDead()
    {
        return _enemiesHealth.All(health => health.IsDead);
    }
}
