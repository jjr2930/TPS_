using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public class CustomTPSCharacterControllerAuthoring : MonoBehaviour
    {
        public float rotateSpeed = 90f;
        public class Baker : Baker<CustomTPSCharacterControllerAuthoring>
        {
            public override void Bake(CustomTPSCharacterControllerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CustomTPSCharacterController()
                {
                    rotationSpeed = authoring.rotateSpeed
                });
            }
        }
    }
}
