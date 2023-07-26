using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PreAllocation {
    public GameObject gameObject;
    public int count;
    public bool expandable;
}
 
public class ObjectPool : MonoSingleton<ObjectPool>
{
    public List<PreAllocation> preAllocations;
 
    [SerializeField] private List<GameObject> pooledObjects;
 
    protected override void Awake()
    {
        base.Awake();
        pooledObjects = new List<GameObject>();
 
        foreach (var item in preAllocations) {
            for (var i = 0; i < item.count; ++i) 
                pooledObjects.Add(CreateObject(item.gameObject));
        }
    }
 
    public GameObject Spawn(string objectTag) {
        foreach (var t in 
                 pooledObjects.Where(t => !t.activeSelf && t.CompareTag(objectTag)))
        {
            t.SetActive(true);
            return t;
        }
 
        foreach (var obj in from t in preAllocations 
                 where t.gameObject.CompareTag(objectTag) where t.expandable select CreateObject(t.gameObject))
        {
            pooledObjects.Add(obj);
            obj.SetActive(true);
            return obj;
        }
        return null;
    }

    private GameObject CreateObject(GameObject item) {
        var gobject = Instantiate(item, transform);
        gobject.SetActive(false);
        return gobject;
    }
}