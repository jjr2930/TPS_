using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
namespace MyTPS
{
    public struct MovementData : IComponentData
    {
        public float movementSpeed;
        public float rotationSpeed;
        public float friction;
        public float accel;
        public float jumpPower;

        public float2 movingInput;
        public float2 lookingInput;
        public bool shouldJump;
    }
}