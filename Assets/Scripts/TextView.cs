using System;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterQuest
{
    public class TextView : MonoBehaviour, ITextView
    {
        [SerializeField]
        private string _startText;

        [SerializeField]
        private Text _text;

        

        private void Awake()
        {
            _text.text = _startText;
        }

        public void UpdateText(string newText)
        {
            _text.text = newText;
        }
    }
}