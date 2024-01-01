using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MyTPS
{
    public enum CameraMode
    {
        Normal,
        Aim
    }

    public struct CustomTPSCamera : IComponentData
    {
        //none mdoe
        public float elevation;
        public float polar;
        public float distance;
        public float rotateSpeed;
        //x : radius, y : elevation, z : polar
        public float3 aimOffset;
        public CameraMode mode;
        public float elevationMin;
        public float elevationMax;
        public Entity cameraEntity;
        public float2 zoomMinMax;

        //input values
        public float2 lookingInput;
        public float zoomValue;
        public bool aimPressed;
    }
}
