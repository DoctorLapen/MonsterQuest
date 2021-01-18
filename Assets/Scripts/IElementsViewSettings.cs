using System.Collections.Generic;
using UnityEngine.UI;

namespace MonsterQuest
{
    public interface IElementsViewSettings
    {
        Dictionary<Element, Image> ElementsImages { get; }
    }
}