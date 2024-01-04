using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public class GameConfigAuthoring : MonoBehaviour
    {
        public bool inverseLookHorizontal = false;
        public bool inverseLookVertical = false;
        public float mouseSensitive = 1f;
        public float lookInputThreshold = 1f;
        public float movementInputThreshold = 1f;
        public float aimDistance = 1000f;
        public float aimYMultiplier = 10f;
        public float aimXMultiplier = 10f;
    }

    public class GameConfigBaker : Baker<GameConfigAuthoring>
    {
        public override void Bake(GameConfigAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new GameConfigData()
            {
                inverseLookHozitonal = authoring.inverseLookHorizontal,
                inverseLookVertical = authoring.inverseLookVertical,
                mouseSensitive = authoring.mouseSensitive,
                lookInputThreshold = authoring.lookInputThreshold,
                movementInputThreshold = authoring.movementInputThreshold,
                aimXMultiplier = authoring.aimXMultiplier,
                aimYMultiplier = authoring.aimYMultiplier
            }) ;
        }
    }
}
