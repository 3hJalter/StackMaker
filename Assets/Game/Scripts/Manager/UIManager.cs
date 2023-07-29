using System.Collections;
using System.Collections.Generic;
using Game.ScriptableObjects.Editor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{

    // Screen container
    [SerializeField] private Transform screenContainer;
    [SerializeField] private Transform popupContainer;
    public Transform ScreenContainer => screenContainer;
    public Transform PopupContainer => popupContainer;
    // Screen List
    [SerializeField] private ScreenUI screenUI;
    public Dictionary<Screen, GameObject> ScreenObject { get; private set; }
    public BaseScreen currentScreen;
    public ScreenUI ScreenUI => screenUI;

    // Popup List 
    [SerializeField] private PopupUI popupUI;
    public Dictionary<Popup, GameObject> PopupObject { get; private set; }
    public PopupUI PopupUI => popupUI;
    
    
    [SerializeField] private Image waitBg;
    // Start is called before the first frame update
    private const float MinScaleWaitBg = 0.2f;
    private const float MaxScaleWaitBg = 15f;

    private void Start()
    {
        OnInitOne();
        OnInit();
    }

    private void OnInitOne()
    {
        ScreenObject = new Dictionary<Screen, GameObject>();
        PopupObject = new Dictionary<Popup, GameObject>();
        var screen = screenUI.screenList[(int) Screen.MainScreen];
        ScreenObject.Add(Screen.MainScreen, screen.CreateScreen());
        currentScreen = ScreenObject[Screen.MainScreen].GetComponent<BaseScreen>();
    }
    
    public void OnInit()
    {
        waitBg.gameObject.SetActive(true);
        if (currentScreen is MainScreen or InGameScreen)
        {
            currentScreen.LevelText.text = "LEVEL " + GameManager.Instance.Level;
        }
    }

    public IEnumerator ScaleUpWaitBg()
    {
        if (currentScreen != null) currentScreen.ShowComponent();
        if (Mathf.Abs(waitBg.rectTransform.localScale.x - MaxScaleWaitBg) < 0.01f) yield return null;
        waitBg.gameObject.SetActive(true);
        waitBg.rectTransform.localScale = Vector3.one * MinScaleWaitBg;
        var time = 1f;
        const float offset = MaxScaleWaitBg - MinScaleWaitBg;
        while (time > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            time -= Time.deltaTime;
            waitBg.rectTransform.localScale += Vector3.one * (offset * Time.deltaTime);
            if (!(waitBg.rectTransform.localScale.x >= MaxScaleWaitBg)) continue;
            waitBg.rectTransform.localScale = Vector3.one * MaxScaleWaitBg;
            waitBg.gameObject.SetActive(false);
            GameManager.Instance.isInGameRunning = currentScreen is InGameScreen;
            yield return null;
        }
    }
    
    public IEnumerator ScaleDownWaitBg()
    {
        currentScreen.HideComponents();
        GameManager.Instance.isInGameRunning = false;
        waitBg.gameObject.SetActive(true);
        var time = 1f;
        const float offset = MaxScaleWaitBg - MinScaleWaitBg;
        while (time > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            time -= Time.deltaTime;
            waitBg.rectTransform.localScale -= Vector3.one * (offset * Time.deltaTime);
            if (!(waitBg.rectTransform.localScale.x <= MinScaleWaitBg)) continue;
            waitBg.rectTransform.localScale = Vector3.one * MinScaleWaitBg;
            GameManager.Instance.OnInit();
            yield return null;
        }
    }


    public void ShowWaitBg()
    {
        StartCoroutine(ScaleDownWaitBg());
    }
}
