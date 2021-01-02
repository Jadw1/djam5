using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Color hitColor = Color.red;
    public float lerpFactor = 0.5f;
    
    private Color _targetColor;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _targetColor = _renderer.material.color;
    }

    private void Update()
    {
        _renderer.material.color = Color.Lerp(_renderer.material.color, _targetColor, lerpFactor * Time.deltaTime);
    }

    public void DoDamage(float amount)
    {
        Debug.Log($"Received damage: {amount}");
        _renderer.material.color = hitColor;
    }
}
