using UnityEngine;

public class BasePopup : MonoBehaviour
{
    private bool IsInit { get; set; }
    
    public Transform Root => root;
    [SerializeField] private Transform root;
    private void Start()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
        if (IsInit) return;
        IsInit = true;
        root = gameObject.transform;
        root.SetParent(UIManager.Instance.PopupContainer);
    }

    public void CreatePopup(Popup popup)
    {
        var newPopup = Instantiate(gameObject);
        UIManager.Instance.PopupObject.Add(popup, newPopup);
    }
    
    protected virtual void OnEnable()
    {
        OnInit();
    }

    protected virtual void OnDisable()
    {
        Debug.Log("Do something when disable popup");
    }
}
