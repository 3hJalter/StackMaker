using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField] private bool isThrough;

    public void OnInit()
    {
        isThrough = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isThrough) return;
        isThrough = false;
        Debug.Log("Set end game condition to game manager here");
        GameManager.Instance.OnWin();
    }
}
