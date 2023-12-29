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
    public partial struct AnimatorModelFollowSystem : ISystem
    { 

        public void OnUpdate(ref SystemState state)
        {
            //var animatorQuery = SystemAPI.QueryBuilder().WithAll<LocalToWorld, AnimatorModelInstanceData>().Build();
            //var entities = animatorQuery.ToEntityArray(Allocator.TempJob);
            //var parentPositions = new NativeArray<Vector3>(entities.Count(), Allocator.TempJob);
            //TransformAccessArray animatorTransformAccessors;
            //{
            //    Transform[] animatorTransforms = new Transform[entities.Length];
            //    for (int i = 0; i < entities.Length; i++)
            //    {
            //        var entity = entities[i];
            //        var localToWorld = state.EntityManager.GetComponentData<LocalToWorld>(entity);
            //        var animatorInstance = state.EntityManager.GetComponentObject<AnimatorModelInstanceData>(entity);
            //        animatorTransforms[i] = animatorInstance.instance.transform;
            //        parentPositions[i] = localToWorld.Position;
            //    }

            //    animatorTransformAccessors = new TransformAccessArray(animatorTransforms);
            //    new AnimatorModelFollowJob()
            //    {
            //        parentPositions = parentPositions,
            //    }.Schedule(animatorTransformAccessors);
            //}
            //entities.Dispose();
            //animatorTransformAccessors.Dispose();
            //parentPositions.Dispose();

            var animatorQuery = SystemAPI.QueryBuilder().WithAll<LocalToWorld, AnimatorModelInstanceData>().Build();
            var entities = animatorQuery.ToEntityArray(Allocator.Temp);
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    var entity = entities[i];
                    var localToWorld = state.EntityManager.GetComponentData<LocalToWorld>(entity);
                    var animatorInstance = state.EntityManager.GetComponentObject<AnimatorModelInstanceData>(entity);
                    animatorInstance.instance.transform.SetPositionAndRotation(localToWorld.Position, localToWorld.Rotation);
                }
            }
            entities.Dispose();
        }
    }

    //public struct AnimatorModelFollowJob : IJobParallelForTransform
    //{
    //    [ReadOnly] public NativeArray<Vector3> parentPositions;
        
    //    public void Execute(int index, TransformAccess transform)
    //    {
    //        transform.position = parentPositions[index];
    //    }
    //}
}
