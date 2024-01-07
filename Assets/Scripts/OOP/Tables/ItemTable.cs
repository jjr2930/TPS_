using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{

    public struct ItemTalbeSchemeBlobStruct
    {
        public FixedString32Bytes fixedDisplayName;
        public Entity prefab;
        public Entity icon;
    }

    [Serializable]
    public class ItemTableScheme : TableSchemeBase<int, ItemTalbeSchemeBlobStruct>
    {
        public string displayedName;
        public GameObject prefab;
        public Sprite icon;

        public override BlobAssetReference<ItemTalbeSchemeBlobStruct> BuildBlobAsset()
        {
            throw new NotImplementedException();     
        }
    }

    [CreateAssetMenu(menuName = "Tables/ItemTable")]
    public class ItemTable : TableBase<int, ItemTableScheme, ItemTalbeSchemeBlobStruct>
    {
        protected override int GetNextKey()
        {
            int max = int.MinValue;
            for (int i = 0; i < data.Count; i++)
            {
                if (max < data[i].key)
                {
                    max = data[i].key;
                }
            }

            if (max == int.MinValue)
            {
                max = -1;
            }

            return max + 1;
        }
    }
}
