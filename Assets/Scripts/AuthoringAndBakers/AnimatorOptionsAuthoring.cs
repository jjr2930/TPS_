using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public class AnimatorOptionsAuthoring : MonoBehaviour
    {
        [Range(0.0001f, 20f)]
        public float xLerpSpeed = 10f;
        
        [Range(0.0001f, 20f)]
        public float zLerpSpeed = 10f;

        public class Baker : Baker<AnimatorOptionsAuthoring>
        {
            public override void Bake(AnimatorOptionsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new AnimatorOptions()
                {
                    xLerpSpeed = authoring.xLerpSpeed,
                    zLerpSpeed = authoring.zLerpSpeed,
                });
            }
        }
    }
}
