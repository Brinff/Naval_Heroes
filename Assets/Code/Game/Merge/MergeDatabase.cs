using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Merge.Data
{
    [CreateAssetMenu(menuName = "Data/Database/Merge")]
    public class MergeDatabase : Database
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

        public EntityData GetResult(EntityData a, EntityData b)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (!item.isValid) continue;
                if (item.a == a && item.b == b) return item.result;
            }
            return null;
        }
    }
}