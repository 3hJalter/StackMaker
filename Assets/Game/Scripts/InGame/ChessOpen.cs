using System.Collections;
using UnityEngine;

public class ChessOpen : MonoBehaviour
{
    private bool _isOpen;
    [SerializeField] private GameObject chessClose;
    [SerializeField] private GameObject chessOpen;

    public void OnInit()
    {
        _isOpen = false;
        chessClose.SetActive(true);
        chessOpen.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_isOpen) return;
        if (!other.CompareTag("Player")) return;
        _isOpen = true;
        StartCoroutine(OpenChess(other.GetComponent<PlayerN>()));
        StartCoroutine(ShowVictoryScreen());
    }

    private IEnumerator OpenChess(PlayerN playerN)
    {
        yield return new WaitForSeconds(0.5f);
        chessClose.SetActive(false);
        chessOpen.SetActive(true);
        playerN.ChangeAnim(PlayerState.Victory);
    }
    
    private IEnumerator ShowVictoryScreen()
    {
        yield return new WaitForSeconds(2.5f);
        UIManager.Instance.currentScreen.ChangeScreen(Screen.VictoryScreen);
    }
}
