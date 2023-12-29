using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public class AnimatorModelInstanceData : IComponentData, IDisposable
    {
        public GameObject instance;

        void IDisposable.Dispose()
        {
            UnityEngine.Object.DestroyImmediate(instance);
        }
    }
}
