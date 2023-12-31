using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace MyTPS
{
    public class PlayerCombatModeAuthoring : MonoBehaviour
    {
        public bool isCombatMode;

        public class Baker : Baker<PlayerCombatModeAuthoring>
        {
            public override void Bake(PlayerCombatModeAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new PlayerCombatMode()
                {
                    isCombatMode = authoring.isCombatMode
                });
            }
        }
    }
}
