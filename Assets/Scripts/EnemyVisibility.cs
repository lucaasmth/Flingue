using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
    public Shader invisibleShader;
    public Shader standardShader;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void SetVisible(float duration)
    {
        SetShader(standardShader);
        if (duration > 0) StartCoroutine(SetInvisibleAfterTime(duration));
    }

    private IEnumerator SetInvisibleAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        SetShader(invisibleShader);
    }

    private void SetShader(Shader shader)
    {
        _renderer.material.shader = shader;
    }
}
