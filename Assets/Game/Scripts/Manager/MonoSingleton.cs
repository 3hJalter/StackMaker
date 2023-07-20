using UnityEngine;
 public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _mInstance;
    private static bool _shuttingDown;
 
    public static T Instance
    {
        get
        {
            if (_mInstance == null && !_shuttingDown && Application.isPlaying)
            {
                _mInstance = FindObjectOfType(typeof(T)) as T;
 
                if (_mInstance == null)
                {
                    Debug.LogWarning("No instance of " + typeof(T) + ", a temporary one is created.");
 
                    _mInstance = new GameObject("Temp Instance of " + typeof(T), typeof(T)).GetComponent<T>();
                }
            }
 
            return _mInstance;
        }
    }
 
    protected virtual void Awake()
    {
        if (_mInstance == null)
            _mInstance = this as T;
        else if (_mInstance != this)
        {
            Debug.LogError("Another instance of " + GetType() + " is already exist! Destroying self...");
            DestroyImmediate(gameObject);
        }
    }
 
    protected virtual void OnDestroy()
    {
        if (this == _mInstance)
            _mInstance = null;
    }
 
    private void OnApplicationQuit()
    {
        _mInstance = null;
        _shuttingDown = true;
    }
}