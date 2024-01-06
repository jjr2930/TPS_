using System.Linq;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;
using static UnityEngine.EventSystems.EventTrigger;

namespace MyTPS
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct PlayerAnimatorModelFollowSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var animatorQuery = SystemAPI.QueryBuilder().WithAll<LocalToWorld, AnimatorModelInstanceData, PlayerCombatMode>().Build();
            var entities = animatorQuery.ToEntityArray(Allocator.Temp);
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    var entity = entities[i];
                    var localToWorld = state.EntityManager.GetComponentData<LocalToWorld>(entity);
                    var animatorInstance = state.EntityManager.GetComponentObject<AnimatorModelInstanceData>(entity);
                    var combatMode = state.EntityManager.GetComponentData<PlayerCombatMode>(entity);
                    var isCombatMode = combatMode.isCombatMode;
                    var instanceTransform = animatorInstance.instance.transform;
                    if (isCombatMode)
                    {
                        instanceTransform.position = localToWorld.Position;
                    }
                    else
                    {
                        instanceTransform.SetPositionAndRotation(localToWorld.Position, localToWorld.Rotation);
                    }
                }
            }
            entities.Dispose();
        }
    }
}
