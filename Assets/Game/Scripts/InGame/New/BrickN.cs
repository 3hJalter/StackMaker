using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickN : MonoBehaviour
{
    private bool _isAttach;
    
    private void OnTriggerEnter(Collider other)
    {
        if (_isAttach) return;
        if (other.CompareTag("Player"))
        {
            AttachBrickToPlayer();
        }
    }

    private void AttachBrickToPlayer()
    {
        _isAttach = true;
        Debug.Log("Attach Brick");
    }
}
