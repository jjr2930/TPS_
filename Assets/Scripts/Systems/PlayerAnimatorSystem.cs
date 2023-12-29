using System.Collections;
using System.Collections.Generic;
using Unity.CharacterController;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MyTPS
{
    public partial struct PlayerAnimatorSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var entityQuery = SystemAPI.QueryBuilder()
                .WithAll<ThirdPersonPlayerInputs, ThirdPersonPlayer>()
                .Build();

            var em = state.EntityManager;
            var entities = entityQuery.ToEntityArray(Allocator.Temp);

            uint tick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;
            Debug.Log("entities Count : " + entities.Length);
            foreach (var entity in entities)
            {
                var thirdPersonPlayer = em.GetComponentData<ThirdPersonPlayer>(entity);
                var input = em.GetComponentData<ThirdPersonPlayerInputs>(entity);

                var characterEntity = thirdPersonPlayer.ControlledCharacter;
                var thirdPersonControl = em.GetComponentData<ThirdPersonCharacterControl>(characterEntity);
                var animatorInstance = em.GetComponentObject<AnimatorModelInstanceData>(characterEntity);
                var animator = animatorInstance.instance.GetComponent<Animator>();
                if (thirdPersonControl.Jump)
                {
                    animator.SetTrigger("Jump");
                }
                animator.SetFloat("X", input.MoveInput.x);
                animator.SetFloat("Z", input.MoveInput.y);
                animator.SetFloat("Speed", math.length(thirdPersonControl.MoveVector));


                //var animatorInstance = em.GetComponentObject<AnimatorModelInstanceData>(entity);
                //var animator = animatorInstance.instance.GetComponent<Animator>();
                //var kinematicBody = em.GetComponentData<KinematicCharacterBody>(entity);

                //if(input.JumpPressed.IsSet(tick))
                //{
                //    animator.SetTrigger("Jump");
                //}

                //Debug.Log("WOW"); 
                //animator.SetFloat("X", input.MoveInput.x);
                //animator.SetFloat("Z", input.MoveInput.y);
                //animator.SetFloat("Speed", math.length(kinematicBody.RelativeVelocity));
            }
        }
    }
}
