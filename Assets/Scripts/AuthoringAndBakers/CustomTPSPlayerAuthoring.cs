using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public class CustomTPSPlayerAuthoring : MonoBehaviour
    {
        public GameObject tpsCharacterEntity;
        public GameObject tpsCameraEntity;

        public class Baker : Baker<CustomTPSPlayerAuthoring>
        {
            public override void Bake(CustomTPSPlayerAuthoring authoring)
            {
                var tpsPlayer = GetEntity(TransformUsageFlags.None);
                AddComponent(tpsPlayer, new CustomTPSPlayer()
                {
                    cameraEntity = GetEntity(authoring.tpsCameraEntity, TransformUsageFlags.Dynamic),
                    characterEntity = GetEntity(authoring.tpsCharacterEntity, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}
