using System.Collections.Generic;
using UnityEngine;

namespace Game.ScriptableObjects.Editor
{
    [CreateAssetMenu(fileName = "ScreenUI", menuName = "ScriptableObjects/ScreenUI", order = 1)]
    public class ScreenUI : ScriptableObject
    {
        public List<BaseScreen> screenList;
    }
}
