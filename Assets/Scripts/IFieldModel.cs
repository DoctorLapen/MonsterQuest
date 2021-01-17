using System;

namespace MonsterQuest
{
    public interface IFieldModel
    {
        event Action<CellChangedArgs> CellChanged;
        void InitializeField();
    }
}