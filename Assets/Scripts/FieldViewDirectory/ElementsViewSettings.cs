using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterQuest
{
    [CreateAssetMenu(fileName = "ElementsViewSettings", menuName = "MonsterQuest/ElementsViewSettings", order = 2)]
    public class ElementsViewSettings : ScriptableObject, IElementsViewSettings
    {
        [SerializeField]
        private List<ElementInfo> _elements;
        

        public Dictionary<Element, Image> ElementsImages => _elementsImages;
        
        private Dictionary<Element, Image> _elementsImages;

        private void OnEnable()
        {
            _elementsImages = new Dictionary<Element, Image>();
            foreach (var element in _elements)
            {
                _elementsImages.Add(element.type,element.sprite);
            }
        }

        [Serializable]
        private class ElementInfo
        {
            public Image sprite;
            public Element type;
        }
    }
}