using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
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

        //aim options
        public float aimDistance;
        public float aimYMultiplier;
        public float aimXMultiplier;
    }
}
