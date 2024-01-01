using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MyTPS
{
    public struct CustomTPSCharacterController : IComponentData
    {
        //input infomation
        public float2 lookingInput;
        public float2 movingInput;
        public bool walkingPressed;
        public bool aimPressed;

        //speedInfomation
        public float runningMaxSpeed;
        public float walkingMaxSpeed;
        public float rotationSpeed;
        public float accel;
        public float friction;

        public float3 lastVelocity;
        public CameraMode aimMode;
    }
}
