using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
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
            float inputEpsilon = 0.0001f;
            foreach (var entity in entities)
            {
                var thirdPersonPlayer = em.GetComponentData<ThirdPersonPlayer>(entity);
                var input = em.GetComponentData<ThirdPersonPlayerInputs>(entity);
                var characterEntity = thirdPersonPlayer.ControlledCharacter;
                var kinematicBody = em.GetComponentData<KinematicCharacterBody>(characterEntity);
                var thirdPersonControl = em.GetComponentData<ThirdPersonCharacterControl>(characterEntity);
                var animatorInstance = em.GetComponentObject<AnimatorModelInstanceData>(characterEntity);
                var combatMode = em.GetComponentData<PlayerCombatMode>(characterEntity);
                
                var animator = animatorInstance.instance.GetComponent<Animator>();
                var isCombatMode = combatMode.isCombatMode;
                var inputLength = math.length(input.MoveInput);

                if (input.JumpPressed.IsSet(tick))
                {

                    if (false == animator.IsInTransition(0))
                    {
                        animator.SetTrigger(AnimatorHash.Jump);
                    }
                }

                //if(inputLength > inputEpsilon)
                {
                    var current = animator.GetFloat(AnimatorHash.Z);
                    var target = inputLength;
                    if (input.walkingPressed)
                        target *= 0.5f;

                    var next = MathUtility.MoveToward(current, target, 5f * SystemAPI.Time.DeltaTime);
                    animator.SetFloat(AnimatorHash.Z, next);

                    //if (isCombatMode)
                    //{
                       

                    //    animator.SetFloat(AnimatorHash.X, input.MoveInput.x);
                    //    animator.SetFloat(AnimatorHash.Z, input.MoveInput.y);

                    //    animator.SetBool(AnimatorHash.Walking, input.walkingPressed);
                    //}
                    //else
                    //{
                    //    animator.SetFloat(AnimatorHash.Z, math.length(input.MoveInput));
                    //    animator.SetBool(AnimatorHash.Walking, input.walkingPressed);
                    //}
                }

                animator.SetBool(AnimatorHash.Ground, kinematicBody.IsGrounded);
            }
        }
    }
}
