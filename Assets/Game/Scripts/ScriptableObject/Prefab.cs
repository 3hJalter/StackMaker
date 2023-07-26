using System.Collections.Generic;
using UnityEngine;

namespace Game.ScriptableObjects.Editor
{
    [CreateAssetMenu(fileName = "Prefab", menuName = "ScriptableObjects/Prefab", order = 1)]
    public class Prefab : ScriptableObject
    {
        public List<GameObject> prefab;
    }
}