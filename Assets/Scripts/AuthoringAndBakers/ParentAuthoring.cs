using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace MyTPS
{
    public class ParentAuthoring : MonoBehaviour
    {
        public bool isDynamic = false;
        public class Baker : Baker<ParentAuthoring>
        {
            public override void Bake(ParentAuthoring authoring)
            {
                var usage = authoring.isDynamic ? TransformUsageFlags.Dynamic : TransformUsageFlags.None;
                var entity = GetEntity(usage);
                AddComponent<Parent>(entity);
            }
        }
    }
}
