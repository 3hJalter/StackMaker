using System;
using System.Security.Cryptography;
using UnityEngine;

public class BrickN : MonoBehaviour
{
    private bool _isAttach;
    [SerializeField] private GameObject brick;
    public void OnInit()
    {
        brick.SetActive(true);
        _isAttach = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_isAttach) return;
        if (other.CompareTag("Player"))
        {
            AttachBrickToPlayer(other.GetComponent<PlayerN>());
        }
    }

    private void AttachBrickToPlayer(PlayerN playerN)
    {
        _isAttach = true;
        // Change to pooling if need optimize
        brick.SetActive(false);
        playerN.AttachBrick();
    }
}
