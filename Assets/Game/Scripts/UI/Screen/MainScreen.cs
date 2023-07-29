using TMPro;
using UnityEngine;

public class MainScreen : BaseScreen
{
    [SerializeField] private GameObject playButton;
    protected override void OnInit()
    {
        base.OnInit();
        playButton.SetActive(true);
        levelText.text = "LEVEL " + GameManager.Instance.Level;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        levelText.text = "LEVEL " + GameManager.Instance.Level;
    }

    public void OnPlayButton()
    {
        Debug.Log("Click play button");
        ChangeScreen(Screen.InGameScreen);
        GameManager.Instance.isInGameRunning = true;
    }

    public override void HideComponents()
    {
        base.HideComponents();
        playButton.SetActive(false);
    }

    public override void ShowComponent()
    {
        base.ShowComponent();
        playButton.SetActive(true);
    }
}
