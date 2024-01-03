using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.CharacterController;

namespace MyTPS
{
    public partial struct CustomTPSCharacterControllerSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (characterController, localTransform, localToWorld, entity)
                in SystemAPI.Query<RefRW<CustomTPSCharacterController>, RefRW<LocalTransform>, RefRW<LocalToWorld>>().WithEntityAccess())
            {
                var movingInput = characterController.ValueRO.movingInput;
                var lookingInput = characterController.ValueRO.lookingInput;
                var aimPressed = characterController.ValueRO.aimPressed;

                characterController.ValueRW.aimMode = aimPressed ? CameraMode.Aim : CameraMode.Normal;
                var aimMode = characterController.ValueRW.aimMode;
                var deltaTime = SystemAPI.Time.DeltaTime;
                switch (aimMode)
                {
                    case CameraMode.Normal:
                        {

                        }
                        break;

                    case CameraMode.Aim:
                        {
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
