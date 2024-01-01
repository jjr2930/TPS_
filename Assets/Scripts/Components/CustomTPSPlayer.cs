using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public struct CustomTPSPlayer : IComponentData
    {
        public Entity cameraEntity;
        public Entity characterEntity;
    }
}
