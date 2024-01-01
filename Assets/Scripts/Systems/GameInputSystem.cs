using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
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

            foreach (var tpsCharacter in SystemAPI.Query<RefRW<CustomTPSCharacterController>>())
            {
                tpsCharacter.ValueRW.lookingInput = lookingInput;
                tpsCharacter.ValueRW.movingInput = movingInput;
                tpsCharacter.ValueRW.walkingPressed = walkingPressed;
                tpsCharacter.ValueRW.aimPressed = aimPressed;
            }
        }
    }
}
