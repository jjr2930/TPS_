using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MyTPS
{
    public struct CustomTPSCameraTarget : IComponentData
    {
        public Entity target;
    }
}