using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public interface IFieldModel
    {
        event Action<CellChangedArgs> CellChanged;
        void InitializeField();
        bool IsElementInField(Vector2Int elementCoordinates);
        void ReplaceElements(Vector2Int coordinatesA,Vector2Int coordinatesB);
        event Action<ElementsReplacedArgs> ElementsReplaced;
        HashSet<Vector2Int> FindMatchedElements(Vector2Int coordinateA,Vector2Int coordinateB);
        void DeleteElements(IEnumerable<Vector2Int> elementsCoordinates);
        
        event Action<ElementsMoveDownArgs> ElementsMovedDown;
        void ShiftElements();
        HashSet<Vector2Int> FindMatchedElements();
    }
}