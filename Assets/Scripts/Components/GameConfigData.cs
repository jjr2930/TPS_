using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public struct GameConfigData : IComponentData
    {
        public bool inverseLookVertical;
        public bool inverseLookHozitonal;
        public float mouseSensitive;
        public float movementInputThreshold;
        public float lookInputThreshold;
    }
}
