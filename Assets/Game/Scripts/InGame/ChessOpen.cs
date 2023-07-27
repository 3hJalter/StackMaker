using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessOpen : MonoBehaviour
{
    private bool _isOpen;
    [SerializeField] private GameObject chessClose;
    [SerializeField] private GameObject chessOpen;
    private void OnTriggerEnter(Collider other)
    {
        if (_isOpen) return;
        if (!other.CompareTag("Player")) return;
        _isOpen = true;
        StartCoroutine(OpenChess(other.GetComponent<PlayerN>()));
    }

    private IEnumerator OpenChess(PlayerN playerN)
    {
        yield return new WaitForSeconds(0.5f);
        chessClose.SetActive(false);
        chessOpen.SetActive(true);
        playerN.ChangeAnim(PlayerState.Victory);
    }
}
