using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.CharacterController;
using Unity.Physics.Systems;
using MyTPS;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
[BurstCompile]
public partial struct BasicPlayerVariableStepControlSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<BasicPlayer, BasicPlayerInputs>().Build());
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (playerInputs, player) in SystemAPI.Query<BasicPlayerInputs, BasicPlayer>().WithAll<Simulate>())
        {
            if (SystemAPI.HasComponent<CustomTPSCameraTarget>(player.ControlledCamera))
            {
                var camerTarget = SystemAPI.GetComponent<CustomTPSCameraTarget>(player.ControlledCamera);
                camerTarget.target = player.ControlledCharacter;
                
                SystemAPI.SetComponent(player.ControlledCamera, camerTarget);
            }
        }
    }
}

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderFirst = true)]
[BurstCompile]
public partial struct BasicFixedStepPlayerControlSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<FixedTickSystem.Singleton>();
        state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<BasicPlayer, BasicPlayerInputs>().Build());
    }

    public void OnUpdate(ref SystemState state)
    {
        var gameConfigs = SystemAPI.GetSingleton<GameConfigData>();

        uint tick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;
        
        foreach (var (playerInputs, player) in SystemAPI.Query<RefRW<BasicPlayerInputs>, BasicPlayer>().WithAll<Simulate>())
        {
            if (SystemAPI.HasComponent<BasicCharacterControl>(player.ControlledCharacter))
            {
                BasicCharacterControl characterControl = SystemAPI.GetComponent<BasicCharacterControl>(player.ControlledCharacter);
                var characterRotation = SystemAPI.GetComponent<LocalTransform>(player.ControlledCharacter).Rotation;
                float3 characterUp = MathUtilities.GetUpFromRotation(characterRotation);

                // Get camera rotation, since our movement is relative to it.
                quaternion cameraRotation = Camera.main.transform.rotation;
                if (SystemAPI.HasComponent<LocalToWorld>(player.ControlledCamera))
                {
                    // Camera rotation is calculated rather than gotten from transform, because this allows us to 
                    // reduce the size of the camera ghost state in a netcode prediction context.
                    // If not using netcode prediction, we could simply get rotation from transform here instead.

                    characterControl.cameraMode = playerInputs.ValueRO.aimPressed ? CameraMode.Aim : CameraMode.Normal;
                    switch (characterControl.cameraMode)
                    {
                        case CameraMode.Normal:
                            {
                                float3 cameraForwardOnUpPlane = math.normalizesafe(MathUtilities.ProjectOnPlane(MathUtilities.GetForwardFromRotation(cameraRotation), characterUp));
                                float3 cameraRight = MathUtilities.GetRightFromRotation(cameraRotation);

                                // Move
                                characterControl.MoveVector = (playerInputs.ValueRW.MoveInput.y * cameraForwardOnUpPlane) + (playerInputs.ValueRW.MoveInput.x * cameraRight);
                                characterControl.MoveVector = MathUtilities.ClampToMaxLength(characterControl.MoveVector, 1f);
                            }
                            break;

                        case CameraMode.Aim:
                            {
                                float3 characterForwardOnUpPlane = math.normalizesafe(MathUtilities.ProjectOnPlane(MathUtilities.GetForwardFromRotation(characterRotation), characterUp));
                                float3 characterRight = MathUtilities.GetRightFromRotation(characterRotation);

                                characterControl.MoveVector = (playerInputs.ValueRW.MoveInput.y * characterForwardOnUpPlane) + (playerInputs.ValueRW.MoveInput.x * characterRight);
                                characterControl.MoveVector = MathUtilities.ClampToMaxLength(characterControl.MoveVector, 1f);
                                characterControl.lookingInput = playerInputs.ValueRO.CameraLookInput;

                                float3 cameraForwardOnUpPlane = math.normalizesafe(MathUtilities.ProjectOnPlane(MathUtilities.GetForwardFromRotation(cameraRotation), characterUp));
                                characterControl.cameraPlanarForward = cameraForwardOnUpPlane;
                            }
                            break;

                        default:
                            break;
                    }
                }

                // Jump
                // We detect a jump event if the jump counter has changed since the last fixed update.
                // This is part of a strategy for proper handling of button press events that are consumed during the fixed update group
                characterControl.Jump = playerInputs.ValueRW.JumpPressed.IsSet(tick);

                SystemAPI.SetComponent(player.ControlledCharacter, characterControl);
            }
        }
    }
}