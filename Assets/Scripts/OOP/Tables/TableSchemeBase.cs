using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    [Serializable]
    public abstract class TableSchemeBase<KeyT, BlobT>
        where BlobT : unmanaged
    {
        public KeyT key;

        public abstract BlobAssetReference<BlobT> BuildBlobAsset();
    }
}
