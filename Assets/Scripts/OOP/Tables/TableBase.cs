using System;
using System.Collections.Generic;

using UnityEngine;

namespace MyTPS
{
    public abstract class TableBase<KeyT, TableSchemeT, BlobT> : ScriptableObject
        where TableSchemeT : TableSchemeBase<KeyT,BlobT>, new()
        where BlobT : unmanaged
    {
        public List<TableSchemeT> data = new List<TableSchemeT>();

        Dictionary<KeyT, TableSchemeT> dataDict = null;

        public TableSchemeT this[KeyT key]
        {
            get
            {
                if (dataDict == null)
                {
                    BuildDictionary();
                }

                TableSchemeT found = default(TableSchemeT);
                if (dataDict.TryGetValue(key, out found))
                {
                    return found;
                }
                else
                {
                    throw new KeyNotFoundException($"{name} => Key : {key}");
                }
            }
        }
        public void AddOne()
        {
            data.Add(new TableSchemeT() 
                    {
                        key = GetNextKey()
                    });
        }

        public TableSchemeT GetByIndex(int index)
        {
            if( index < 0 || index >= data.Count)
            {
                throw new IndexOutOfRangeException($"out of range, count : {data.Count}, index : {index}");
            }

            return data[index];
        }

        public void BuildDictionary()
        {
            dataDict = new Dictionary<KeyT, TableSchemeT>();
            for (int i = 0; i < data.Count; ++i)
            {
                dataDict.Add(data[i].key, data[i]);
            }
        }

        protected abstract KeyT GetNextKey();
    }
}
