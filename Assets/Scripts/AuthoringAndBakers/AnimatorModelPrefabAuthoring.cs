using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public class AnimatorModelPrefabAuthoring : MonoBehaviour
    {
        public GameObject playerModelPrefab;
        public Vector3 offset;
        public class Baker : Baker<AnimatorModelPrefabAuthoring>
        {
            public override void Bake(AnimatorModelPrefabAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new AnimatorModelPrefabData()
                {
                    modelPrefab = authoring.playerModelPrefab,
                    offest = authoring.offset
                });
            }
        }
    }
}
