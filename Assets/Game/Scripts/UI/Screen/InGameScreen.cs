using TMPro;
using UnityEngine;

public class InGameScreen : BaseScreen
{
    [SerializeField] private GameObject swipeIconObject;
    [SerializeField] public GameObject settingButton;
    [SerializeField] public GameObject restartButton;
    // [SerializeField] private TextMeshProUGUI levelText;

    protected override void OnInit()
    {
        swipeIconObject.SetActive(!IsInit);
        base.OnInit();
        settingButton.SetActive(true);
        restartButton.SetActive(true);
        levelText.text = "LEVEL " + GameManager.Instance.Level;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        levelText.text = "LEVEL " + GameManager.Instance.Level;
    }

    public void OnSettingButton()
    {
        GameManager.Instance.isInGameRunning = false;
        if (UIManager.Instance.PopupObject.ContainsKey(Popup.SettingPopup))
            UIManager.Instance.PopupObject[Popup.SettingPopup].SetActive(true);
        else
        {
            var popup = UIManager.Instance.PopupUI.popupList[(int) Popup.SettingPopup];
            popup.GetComponent<BasePopup>().CreatePopup(Popup.SettingPopup);
        }
        
    }

    public void OnRestartButton()
    {
        GameManager.Instance.isReset = true;
        UIManager.Instance.ShowWaitBg();
    }

    public override void HideComponents()
    {
        base.HideComponents();
        settingButton.SetActive(false);
        restartButton.SetActive(false);
    }

    public override void ShowComponent()
    {
        base.ShowComponent();
        settingButton.SetActive(true);
        restartButton.SetActive(true);
    }
}
