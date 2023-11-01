using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private AudioSource _audioSource;
    
    private void Awake()
    {
        var old = GameObject.FindGameObjectWithTag("Music");
        if(old != gameObject) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
    }
}
