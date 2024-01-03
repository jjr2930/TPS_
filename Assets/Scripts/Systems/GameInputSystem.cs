using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MyTPS
{
    public partial class GameInputSystem : SystemBase
    {
        MyTPSInput input;

        protected override void OnUpdate()
        {
            if (null == input)
            {
                input = new MyTPSInput();
                input.Enable();
                input.HumanAction.Enable();
            }

            var lookingInput = input.HumanAction.Look.ReadValue<Vector2>();
            var movingInput = input.HumanAction.Movement.ReadValue<Vector2>();
            var cameraZoom = input.HumanAction.CameraZoom.ReadValue<float>();
            var aimPressed = input.HumanAction.Aim.IsPressed();
            var walkingPressed = input.HumanAction.Walking.IsPressed();

            foreach (var tpsCamera in SystemAPI.Query<RefRW<CustomTPSCamera>>())
            {
                tpsCamera.ValueRW.lookingInput = lookingInput;
                tpsCamera.ValueRW.zoomValue = cameraZoom;
                tpsCamera.ValueRW.aimPressed = aimPressed;
            }

            var humanAction = input.HumanAction;
            uint tick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;
            
            foreach (var (playerInputs, player) in SystemAPI.Query<RefRW<BasicPlayerInputs>, BasicPlayer>())
            {
                playerInputs.ValueRW.MoveInput = Vector2.ClampMagnitude(humanAction.Movement.ReadValue<Vector2>(), 1f);
                playerInputs.ValueRW.CameraLookInput = humanAction.Look.ReadValue<Vector2>();
                playerInputs.ValueRW.CameraZoomInput = humanAction.CameraZoom.ReadValue<float>();

                if (humanAction.Jump.WasPressedThisFrame())
                {
                    playerInputs.ValueRW.JumpPressed.Set(tick);
                }
            }
        }
    }
}
