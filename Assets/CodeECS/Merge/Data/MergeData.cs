using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Merge.Data
{
    [CreateAssetMenu(menuName = "Data/Merge")]
    public class MergeData : ScriptableObject
    {
        [System.Serializable]
        public class Item
        {
            [TableColumnWidth(60, false)]
            public bool isEnable;
            public EntityData a;
            public EntityData b;
            public EntityData result;

            public bool isValid => isEnable & a & b & result;
        }

        [TableList(ShowIndexLabels = true, ShowPaging = true)]
        public List<Item> items = new List<Item>();
    }
}