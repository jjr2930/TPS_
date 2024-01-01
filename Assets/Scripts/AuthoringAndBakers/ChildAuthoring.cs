using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace MyTPS
{
    public class ChildAuthoring : MonoBehaviour
    {
        public bool isDynamic = false;
        public class Baker :Baker<ChildAuthoring>
        {
            public override void Bake(ChildAuthoring authoring)
            {
                var usage = (authoring.isDynamic) ? TransformUsageFlags.Dynamic : TransformUsageFlags.None;
                var entity = GetEntity(usage);
                AddBuffer<Child>(entity);
            }
        }
    }
}
