using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadN : MonoBehaviour
{
    [SerializeField]
    private bool isGoThrough;

    private void SetNewColor()
    {
        var render = GetComponentInChildren<Renderer>(true);
        if (render != null) render.material.color = Color.cyan;
    }

    private void DetachBrickFromPlayer()
    {
        Debug.Log("Detach Brick");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isGoThrough) return;
        if (!other.CompareTag("Player")) return;
        isGoThrough = true;
        SetNewColor();
    }
}
