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
        List<Vector2Int> FindHorizontalMatch(Vector2Int coordinateA,Vector2Int coordinateB);
    }
}