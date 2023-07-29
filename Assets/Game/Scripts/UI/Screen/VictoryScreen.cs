using UnityEngine;

public class VictoryScreen : BaseScreen
{
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject restartLevelButton;

    protected override void OnInit()
    {
        base.OnInit();
        nextLevelButton.SetActive(true);
        restartLevelButton.SetActive(true);
    }

    public void OnNextLevelButton()
    {
        ChangeScreen(Screen.InGameScreen);
        UIManager.Instance.ShowWaitBg();
        
    }

    public void OnRestartLevelButton()
    {
        if (GameManager.Instance.Level != 0) GameManager.Instance.Level -= 1;
        else GameManager.Instance.Level = GameManager.Instance.TextLevel.levelText.Count - 1;
        PlayerPrefs.SetInt("level", GameManager.Instance.Level);
        GameManager.Instance.isReset = true;
        ChangeScreen(Screen.InGameScreen);
        UIManager.Instance.ShowWaitBg();
        
    }

    public void OnGoToMainMenu()
    {
        ChangeScreen(Screen.MainScreen);
        UIManager.Instance.ShowWaitBg();
        
    }
}
