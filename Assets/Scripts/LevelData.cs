using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class LevelData :ScriptableObject, ILeveldData
    {
        public List<CellList> serializableField = new List<CellList>();
        public Cell[,] Field { get; private set; } 

        private void OnEnable()
        {
            int columns = serializableField.Count;
            Debug.Log(serializableField.Count);
            int rows = serializableField[0].list.Count;
            Field = new Cell[columns, rows];
            for (int column = 0; column < columns ; column++)
            {
                for (int row = 0; row < rows; row++)
                {
                    Field[column, row] = serializableField[column].list[row];
                }
            }
        }
    }
}