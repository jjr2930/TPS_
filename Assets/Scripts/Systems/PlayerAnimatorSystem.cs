using Unity.CharacterController;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace MyTPS
{
    public partial struct PlayerAnimatorSystem : ISystem 
    { 
        public void OnUpdate(ref SystemState state)
        {
            var entityQuery = SystemAPI.QueryBuilder()
                .WithAll<BasicPlayerInputs, BasicPlayer>()
                .Build();

            var entities = entityQuery.ToEntityArray(Allocator.Temp);
            var deltaTime = SystemAPI.Time.DeltaTime;
            uint tick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;
            foreach (var entity in entities)
            {
                var basicPlayer = SystemAPI.GetComponent<BasicPlayer>(entity);
                var basicInput = SystemAPI.GetComponent<BasicPlayerInputs>(entity);

                var controlledCharacter = basicPlayer.ControlledCharacter;

                Animator animator = null;
                
                if(state.EntityManager.HasComponent<AnimatorModelInstanceData>(controlledCharacter))
                {
                    var animatorModelInstance = state.EntityManager.GetComponentObject<AnimatorModelInstanceData>(controlledCharacter);
                    Assert.IsNotNull(animatorModelInstance.instance);

                    animator = state.EntityManager.GetComponentObject<Animator>(controlledCharacter);
                }

                Assert.IsNotNull(animator);


                #region is Ground
                bool isGround = true;
                if (SystemAPI.HasComponent<KinematicCharacterBody>(controlledCharacter))
                {
                    var kinematicBody = SystemAPI.GetComponent<KinematicCharacterBody>(controlledCharacter);
                    isGround = kinematicBody.IsGrounded;
                }

                animator.SetBool(AnimatorHash.Ground, isGround); 
                #endregion


                #region Running and walking
                var x = animator.GetFloat(AnimatorHash.X);
                var z = animator.GetFloat(AnimatorHash.Z);

                float xLerpSpeed = 1f;
                float zLerpSpeed = 1f;

                if (state.EntityManager.HasComponent<AnimatorOptions>(controlledCharacter))
                {
                    var animatorOptions = state.EntityManager.GetComponentData<AnimatorOptions>(controlledCharacter);
                    xLerpSpeed = animatorOptions.xLerpSpeed;
                    zLerpSpeed = animatorOptions.zLerpSpeed;
                }

                if (basicInput.aimPressed)
                {
                    var xTarget = basicInput.MoveInput.x;
                    var zTarget = basicInput.MoveInput.y;

                    x = MathUtility.MoveToward(x, xTarget, xLerpSpeed * deltaTime);
                    z = MathUtility.MoveToward(z, zTarget, zLerpSpeed * deltaTime);
                }
                else
                {
                    var target = math.length(basicInput.MoveInput);
                    x = MathUtility.MoveToward(x, 0f, 10f * deltaTime);
                    z = MathUtility.MoveToward(z, target, zLerpSpeed * deltaTime);
                }

                animator.SetFloat(AnimatorHash.X, x);
                animator.SetFloat(AnimatorHash.Z, z);
                #endregion

                #region Jump!
                if(basicInput.JumpPressed.IsSet(tick) && false == animator.IsInTransition(0))
                {
                    Debug.Log("Jump pressed");
                    animator.SetTrigger(AnimatorHash.Jump);
                }

                #endregion
            }
            //uint tick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;
            //float inputEpsilon = 0.0001f;
            //foreach (var entity in entities)
            //{
            //    var thirdPersonPlayer = em.GetComponentData<ThirdPersonPlayer>(entity);
            //    var input = em.GetComponentData<ThirdPersonPlayerInputs>(entity);
            //    var characterEntity = thirdPersonPlayer.ControlledCharacter;
            //    var kinematicBody = em.GetComponentData<KinematicCharacterBody>(characterEntity);
            //    var thirdPersonControl = em.GetComponentData<ThirdPersonCharacterControl>(characterEntity);
            //    var animatorInstance = em.GetComponentObject<AnimatorModelInstanceData>(characterEntity);
            //    var combatMode = em.GetComponentData<PlayerCombatMode>(characterEntity);

            //    var animator = animatorInstance.instance.GetComponent<Animator>();
            //    var isCombatMode = combatMode.isCombatMode;
            //    var inputLength = math.length(input.MoveInput);

            //    if (input.JumpPressed.IsSet(tick))
            //    {

            //        if (false == animator.IsInTransition(0))
            //        {
            //            animator.SetTrigger(AnimatorHash.Jump);
            //        }
            //    }

            //    //if(inputLength > inputEpsilon)
            //    {
            //        var current = animator.GetFloat(AnimatorHash.Z);
            //        var target = inputLength;
            //        if (input.walkingPressed)
            //            target *= 0.5f;

            //        var next = MathUtility.MoveToward(current, target, 5f * SystemAPI.Time.DeltaTime);
            //        animator.SetFloat(AnimatorHash.Z, next);

            //        //if (isCombatMode)
            //        //{


            //        //    animator.SetFloat(AnimatorHash.X, input.MoveInput.x);
            //        //    animator.SetFloat(AnimatorHash.Z, input.MoveInput.y);

            //        //    animator.SetBool(AnimatorHash.Walking, input.walkingPressed);
            //        //}
            //        //else
            //        //{
            //        //    animator.SetFloat(AnimatorHash.Z, math.length(input.MoveInput));
            //        //    animator.SetBool(AnimatorHash.Walking, input.walkingPressed);
            //        //}
            //    }

            //    animator.SetBool(AnimatorHash.Ground, kinematicBody.IsGrounded);
            //}
        }
    }
}
