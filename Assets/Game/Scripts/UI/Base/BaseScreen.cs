using System;
using TMPro;
using UnityEngine;

public class BaseScreen : MonoBehaviour
{
    protected bool IsInit { get; private set; }
    [SerializeField] protected TextMeshProUGUI levelText;
    public TextMeshProUGUI LevelText => levelText;

    public Transform Root => root;
    [SerializeField] private Transform root;
    
    public void Start()
    {
        OnInit();
    }

    public GameObject CreateScreen()
    {
        return Instantiate(gameObject);
    }

    public void ChangeScreen(Screen screen)
    {
        gameObject.SetActive(false);
        // Logic to change screen base on the Screen enum by calling UI Manager
        var nextScreen = UIManager.Instance.ScreenUI.screenList[(int)screen];
        if (!UIManager.Instance.ScreenObject.ContainsKey(screen))
            UIManager.Instance.ScreenObject.Add(screen, nextScreen.CreateScreen());
        else UIManager.Instance.ScreenObject[screen].SetActive(true);
        UIManager.Instance.currentScreen = UIManager.Instance.ScreenObject[screen].GetComponent<BaseScreen>();
    }

    protected virtual void OnInit()
    {
        if (IsInit) return;
        IsInit = true;
        root = gameObject.transform;
        root.SetParent(UIManager.Instance.ScreenContainer);
    }

    protected virtual void OnEnable()
    {
        OnInit();
    }

    protected virtual void OnDisable()
    {
        Debug.Log("Do something when disable screen");
    }

    public virtual void HideComponents()
    {
        levelText.gameObject.SetActive(false);
    }

    public virtual void ShowComponent()
    {
        levelText.gameObject.SetActive(true);
    }
}
