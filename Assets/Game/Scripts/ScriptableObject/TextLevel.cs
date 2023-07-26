using System.Collections.Generic;
using UnityEngine;

namespace Game.ScriptableObjects.Editor
{
    [CreateAssetMenu(fileName = "TextLevel", menuName = "ScriptableObjects/TextLevel", order = 1)]
    public class TextLevel : ScriptableObject
    {
        public List<TextAsset> levelText;
    }
}
