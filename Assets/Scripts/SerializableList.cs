using System;
using System.Collections.Generic;

namespace MonsterQuest
{

    [Serializable]
    public class SerializableList<T>
    {
        public List<T> list = new List<T>();
    }
}