using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MyTPS
{
    public class AnimatorModelPrefabData : IComponentData
    {
        public GameObject modelPrefab;
        public Vector3 offest;
    }
}
