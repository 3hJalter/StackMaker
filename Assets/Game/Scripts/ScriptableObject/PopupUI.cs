using System.Collections.Generic;
using UnityEngine;

namespace Game.ScriptableObjects.Editor
{
    [CreateAssetMenu(fileName = "PopupUI", menuName = "ScriptableObjects/PopupUI", order = 1)]
    public class PopupUI : ScriptableObject
    {
        public List<BasePopup> popupList;
    }
}
