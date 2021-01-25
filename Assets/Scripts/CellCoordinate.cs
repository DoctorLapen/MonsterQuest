using UnityEngine;

namespace MonsterQuest
{
    public class CellCoordinate : MonoBehaviour
    {
        public Vector2Int Coordinate { get; private set; }
        private bool _isInitialized = false;

        public void Initialize(Vector2Int coordinate)
        {
            if (!_isInitialized)
            {
                Coordinate = coordinate;
            }

            _isInitialized = true;
        }
    }
}