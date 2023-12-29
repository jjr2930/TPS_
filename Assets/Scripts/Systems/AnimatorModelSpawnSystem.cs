using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

namespace MyTPS
{
    public partial struct AnimatorModelSpawnSystem : ISystem
    {
        public void OnUpdate(ref SystemState state) 
        {
            var animatorQuery = SystemAPI.QueryBuilder().WithAll<AnimatorModelPrefabData>().Build();
            var entities = animatorQuery.ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                var animatorModelPrefab = state.EntityManager.GetComponentData<AnimatorModelPrefabData>(entity);
                var instance = GameObject.Instantiate(animatorModelPrefab.modelPrefab, animatorModelPrefab.offest, Quaternion.identity);
                state.EntityManager.AddComponentObject(entity, instance.GetComponent<Transform>());
                state.EntityManager.AddComponentObject(entity, instance.GetComponent<Animator>());
                state.EntityManager.AddComponentData(entity, new AnimatorModelInstanceData()
                {
                    instance = instance
                });
                state.EntityManager.RemoveComponent<AnimatorModelPrefabData>(entity);
            }
        }
    }
}
