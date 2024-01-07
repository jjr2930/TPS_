using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public class MovementAuthoring : MonoBehaviour
    {
        public float movementSpeed = 10f;
        [Tooltip("degree")]
        public float rotationSpeed = 90f;
        public float accel = 5f;
        public float friction = -5f;
        public float jumpPower = 10f;
    }

    public class MovementBaker : Baker<MovementAuthoring>
    {
        public override void Bake(MovementAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MovementData()
            {
                movementSpeed = authoring.movementSpeed,
                rotationSpeed = authoring.rotationSpeed,
                accel  = authoring.accel,
                friction = authoring.friction,
                jumpPower = authoring.jumpPower,
            });
        }
    }
}