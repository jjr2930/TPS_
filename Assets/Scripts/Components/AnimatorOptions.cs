using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public struct AnimatorOptions : IComponentData
    {
        public float xLerpSpeed;
        public float zLerpSpeed;
    }
}
