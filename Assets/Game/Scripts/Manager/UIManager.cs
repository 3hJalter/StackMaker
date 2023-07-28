using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] public Button settingButton;
    [SerializeField] public Button restartButton;
    [SerializeField] private Text levelText;

    public Text LevelText
    {
        get => levelText;
        set => levelText = value;
    }

    [SerializeField] private Image waitBg;
    // Start is called before the first frame update
    private const float MinScaleWaitBg = 0.2f;
    private const float MaxScaleWaitBg = 15f;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        settingButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        waitBg.gameObject.SetActive(true);
        levelText.text = "LEVEL " + GameManager.Instance.Level;
    }

    public IEnumerator ScaleUpWaitBg()
    {
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
            yield return null;
        }
    }
    
    public IEnumerator ScaleDownWaitBg()
    {
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
    
    public void OnClickSetting()
    {
        Debug.Log("Click Setting button");
    }

    public void OnClickRestart()
    {
        GameManager.Instance.isReset = true;
        StartCoroutine(ScaleDownWaitBg());
    }
}
