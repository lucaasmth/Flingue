using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Timer : MonoBehaviour
{
    public float ElapsedTime { get; private set; }
    public TextMeshProUGUI timerText;
    private bool _timerStopped;
    
    // Start is called before the first frame update
    private void Start()
    {
        _timerStopped = false;
        ElapsedTime = 0f;
        StartCoroutine(UpdateUICoroutine());
    }

    // Update is called once per frame
    private void Update()
    {
        if(!_timerStopped) ElapsedTime += Time.deltaTime;
    }

    private IEnumerator UpdateUICoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        int seconds = Mathf.FloorToInt(ElapsedTime % 60);
        int minutes = Mathf.FloorToInt(ElapsedTime / 60);
        
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StopTimer()
    {
        _timerStopped = true;
    }
}
