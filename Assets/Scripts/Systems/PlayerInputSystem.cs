using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Animations;

namespace MyTPS
{
    public partial class PlayerInputSystem : SystemBase
    {
        MyTPSInput myInput;

        protected override void OnUpdate()
        {
            //if (null == myInput)
            //{
            //    myInput = new MyTPSInput();
            //    myInput.Enable();
            //    myInput.HumanAction.Enable();
            //}

            //var gameConfigs = SystemAPI.GetSingleton<GameConfigData>();
            //var invertLookHorizontal = gameConfigs.inverseLookHozitonal ? 1f : -1f;
            //var invertLookVertical = gameConfigs.inverseLookVertical ? 1f : -1f;
            //var movementInputThreshold = gameConfigs.movementInputThreshold;
            //var lookInputThreshold = gameConfigs.lookInputThreshold;

            //var movingInput = myInput.HumanAction.Movement.ReadValue<Vector2>();
            //var lookInput = myInput.HumanAction.Look.ReadValue<Vector2>();
            //var jumpValue = myInput.HumanAction.Jump.ReadValue<bool>();

            //foreach(var (localTransform, localToWorld, movementData)
            //    in SystemAPI.Query<RefRW<LocalTransform>, RefRW< LocalToWorld>, RefRW<MovementData>>())
            //{
            //    /*
            //    //if (math.abs(movementInput.x) > movementInputThreshold)
            //    //{
            //    //    if (movementInput.x < 0f)
            //    //        movementInput.x += movementInputThreshold;
            //    //    else
            //    //        movementInput.x -= movementInputThreshold;

            //    //    Debug.Log("GO!");
            //    //    movementData.ValueRW.movingInput.x = movementInput.x;
            //    //}

            //    //if (math.abs(movementInput.y) > movementInputThreshold)
            //    //{
            //    //    if(movementInput.y < 0f)
            //    //        movementInput.y += movementInputThreshold;
            //    //    else
            //    //        movementInput.y -= movementInputThreshold;

            //    //    Debug.Log("GO!");
            //    //    movementData.ValueRW.movingInput.y = movementInput.y;
            //    //}
            //    */
            //    //movement
            //    movementData.ValueRW.movingInput = movingInput;

            //    //looking
            //    movementData.ValueRW.lookingInput = lookInput;

            //    //jump
            //    if(jumpValue)
            //    {
            //        movementData.ValueRW.shouldJump = true;
            //    }
            //}
        }
    }
}
