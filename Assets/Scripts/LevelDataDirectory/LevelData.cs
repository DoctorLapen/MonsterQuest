using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class LevelData :ScriptableObject, ILeveldData
    {
        [SerializeField]
        private int _turnsForLevel;
        [SerializeField]
        private int _oneLevelCost;
        public List<CellList> serializableField = new List<CellList>();
        public Cell[,] Field { get; private set; }
        public int TurnsForLevel => _turnsForLevel;
        public int OneElementCost => _oneLevelCost;

        private void OnEnable()
        {
            int columns = serializableField.Count;
            int rows = serializableField[0].list.Count;
            Field = new Cell[columns, rows];
            for (int column = 0; column < columns ; column++)
            {
                for (int row = 0; row < rows; row++)
                {
                    Cell cell;
                    if (serializableField[column].list[row].isEmpty)
                    {
                         cell = new Cell(serializableField[column].list[row].IsExist);
                    }
                    else
                    {
                        cell = new Cell(serializableField[column].list[row].IsExist,serializableField[column].list[row].element);
                    }

                    
                    Field[column, row] = cell;
                }
            }
        }
    }
}