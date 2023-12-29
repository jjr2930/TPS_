using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Physics;
using Unity.Mathematics;

namespace MyTPS
{
    public partial struct MovementSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameConfigData>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var gameConfigs = SystemAPI.GetSingleton<GameConfigData>();

            new MovementJob()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
                gravity = PhysicsStep.Default.Gravity
                
            }.ScheduleParallel();
        }
    }

    public readonly partial struct MovementAspect : IAspect
    {
        public readonly Entity entity;
        public readonly RefRW<PhysicsVelocity> physicsVelocity;
        public readonly RefRW<MovementData> movementData;
        public readonly RefRW<LocalTransform> localTransform;
        public void Move(float deltaTime, float3 gravity)
        {
            var currentSpeed = math.length(physicsVelocity.ValueRW.Linear);
            float3 frictionVelocity = new float3(0f, 0f, 0f);

            //마찰력...
            if (currentSpeed > 0)
            {
                var currentDirection = math.normalize(physicsVelocity.ValueRW.Linear);
                var friction = movementData.ValueRO.friction;
                physicsVelocity.ValueRW.Linear -= currentDirection * friction * deltaTime;
            }

            var movementInput = movementData.ValueRO.movingInput;
            var inputMagnitude = math.length(movementInput);

            float3 velocityByInput = new float3(0f, 0f, 0f);
            
            //moving by input
            if (inputMagnitude > 0.0001f)
            {
                var movementSpeed = movementData.ValueRO.movementSpeed;
                var forward = localTransform.ValueRO.Forward();
                var right = localTransform.ValueRO.Right();

                forward *= movementInput.y;
                right *= movementInput.x;

                var direction = forward + right;
                direction = math.normalize(direction);

                physicsVelocity.ValueRW.Linear = direction * inputMagnitude * movementSpeed;
            }

            //physicsVelocity.ValueRW.Linear += gravity; 
        }
    }

    public partial struct MovementJob : IJobEntity
    {
        public float deltaTime;
        public float3 gravity;
        public void Execute(MovementAspect movementAspect)
        {
            movementAspect.Move(deltaTime,gravity);
        }
    }
}
