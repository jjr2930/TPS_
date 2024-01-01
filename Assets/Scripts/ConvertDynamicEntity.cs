using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public class ConvertDynamicEntity : MonoBehaviour
    {
        public class Baker : Baker<ConvertDynamicEntity>
        {
            public override void Bake(ConvertDynamicEntity authoring)
            {
                GetEntity(TransformUsageFlags.Dynamic);
            }
        }
    }
}
