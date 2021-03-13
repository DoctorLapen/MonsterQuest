using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MonsterQuest
{
    public class LeaderboardView : MonoBehaviour, ILeaderboardView
    {
        public event Action UpdateBoard;

        [SerializeField]
        private GameObject _prefab;

        [SerializeField]
        private RectTransform _content;

        

        
        private List<GameObject> _items = new List<GameObject>();
        public void Clear()
        {
            int lenght = _items.Count;
            for (int i = 0; i < lenght; i++)
            {
                GameObject item = _items[0];
                _items.RemoveAt(0);
                Destroy(item);
            }
        }
        public void AddItem(LeaderboardItem boardItem)
        {
            GameObject item = Instantiate(_prefab, _content);
            item.GetComponent<LeaderboardItemView>().ShowTitles(boardItem);
            _items.Add( item );
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void UpdateLeaderboard()
        {
            UpdateBoard?.Invoke();
        }
    }
}