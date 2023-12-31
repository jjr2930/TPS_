using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.CharacterController;
using MyTPS;
using UnityEditor.Build.Pipeline;
using System.Drawing.Drawing2D;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class ThirdPersonPlayerInputsSystem : SystemBase
{
    MyTPSInput myInput;

    protected override void OnCreate()
    {
        RequireForUpdate<FixedTickSystem.Singleton>();
        RequireForUpdate(SystemAPI.QueryBuilder().WithAll<ThirdPersonPlayer, ThirdPersonPlayerInputs>().Build());

        if (null == myInput)
        {
            myInput = new MyTPSInput();
            myInput.Enable();
            myInput.HumanAction.Enable();
        }
    }

    protected override void OnUpdate()
    {
        uint tick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;
        var gameConfigs = SystemAPI.GetSingleton<GameConfigData>();

        foreach (var (playerInputs, player) in SystemAPI.Query<RefRW<ThirdPersonPlayerInputs>, ThirdPersonPlayer>())
        {
            var lookingInput = myInput.HumanAction.Look.ReadValue<Vector2>();
            lookingInput.x *= gameConfigs.inverseLookHozitonal ? -1f : 1f;
            lookingInput.y *= gameConfigs.inverseLookVertical ? -1f : 1f;
            lookingInput *= gameConfigs.mouseSensitive;

            playerInputs.ValueRW.walkingPressed = myInput.HumanAction.Walking.IsPressed();            
            playerInputs.ValueRW.MoveInput = myInput.HumanAction.Movement.ReadValue<Vector2>();
            playerInputs.ValueRW.CameraLookInput = lookingInput;
            playerInputs.ValueRW.CameraZoomInput = myInput.HumanAction.CameraZoom.ReadValue<float>();

            if (myInput.HumanAction.Jump.IsPressed())
            {
                playerInputs.ValueRW.JumpPressed.Set(tick);
            }

            if (myInput.HumanAction.Aim.IsPressed())
            {
                playerInputs.ValueRW.aimPressed.Set(tick);
            }
        }
    }
}

/// <summary>
/// Apply inputs that need to be read at a variable rate
/// </summary>
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
[BurstCompile]
public partial struct ThirdPersonPlayerVariableStepControlSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<ThirdPersonPlayer, ThirdPersonPlayerInputs>().Build());
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (playerInputs, player) in SystemAPI.Query<ThirdPersonPlayerInputs, ThirdPersonPlayer>().WithAll<Simulate>())
        {
            if (SystemAPI.HasComponent<OrbitCameraControl>(player.ControlledCamera))
            {
                OrbitCameraControl cameraControl = SystemAPI.GetComponent<OrbitCameraControl>(player.ControlledCamera);
                
                cameraControl.FollowedCharacterEntity = player.ControlledCharacter;
                cameraControl.LookDegreesDelta = playerInputs.CameraLookInput;
                cameraControl.ZoomDelta = playerInputs.CameraZoomInput;
                
                SystemAPI.SetComponent(player.ControlledCamera, cameraControl);
            }
        }
    }
}

/// <summary>
/// Apply inputs that need to be read at a fixed rate.
/// It is necessary to handle this as part of the fixed step group, in case your framerate is lower than the fixed step rate.
/// </summary>
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderFirst = true)]
[UpdateAfter(typeof(AnimatorModelSpawnSystem))]
[BurstCompile]
public partial struct ThirdPersonPlayerFixedStepControlSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<FixedTickSystem.Singleton>();
        state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<ThirdPersonPlayer, ThirdPersonPlayerInputs>().Build());
    }
    
    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        uint tick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;
        
        foreach (var (playerInputs, player) in SystemAPI.Query<ThirdPersonPlayerInputs, ThirdPersonPlayer>().WithAll<Simulate>())
        {
            if (SystemAPI.HasComponent<ThirdPersonCharacterControl>(player.ControlledCharacter))
            {
                ThirdPersonCharacterControl characterControl = SystemAPI.GetComponent<ThirdPersonCharacterControl>(player.ControlledCharacter);
                ThirdPersonCharacterComponent characterComponent = SystemAPI.GetComponent<ThirdPersonCharacterComponent>(player.ControlledCharacter);
                if (state.EntityManager.HasComponent<AnimatorModelInstanceData>(player.ControlledCharacter))
                {
                    var animatorInstance = state.EntityManager.GetComponentObject<AnimatorModelInstanceData>(player.ControlledCharacter);
                    var animatorEventListener = animatorInstance.instance.GetComponent<AnimatorEventListener>();    
                    
                    // Jump
                    characterControl.Jump = animatorEventListener.jump;
                    animatorEventListener.jump = false;
                }

                float3 characterUp = MathUtilities.GetUpFromRotation(SystemAPI.GetComponent<LocalTransform>(player.ControlledCharacter).Rotation);
                
                // Get camera rotation, since our movement is relative to it.
                quaternion cameraRotation = quaternion.identity;
                if (SystemAPI.HasComponent<OrbitCamera>(player.ControlledCamera))
                {
                    // Camera rotation is calculated rather than gotten from transform, because this allows us to 
                    // reduce the size of the camera ghost state in a netcode prediction context.
                    // If not using netcode prediction, we could simply get rotation from transform here instead.
                    OrbitCamera orbitCamera = SystemAPI.GetComponent<OrbitCamera>(player.ControlledCamera);
                    cameraRotation = OrbitCameraUtilities.CalculateCameraRotation(characterUp, orbitCamera.PlanarForward, orbitCamera.PitchAngle);
                }
                float3 cameraForwardOnUpPlane = math.normalizesafe(MathUtilities.ProjectOnPlane(MathUtilities.GetForwardFromRotation(cameraRotation), characterUp));
                float3 cameraRight = MathUtilities.GetRightFromRotation(cameraRotation);

                // Move
                characterControl.MoveVector = (playerInputs.MoveInput.y * cameraForwardOnUpPlane) + (playerInputs.MoveInput.x * cameraRight);
                characterControl.MoveVector = MathUtilities.ClampToMaxLength(characterControl.MoveVector, 1f);

                //Debug.Log("walking pressed : " + playerInputs.walkingPressed);
                if(playerInputs.walkingPressed)
                {
                    characterComponent.GroundMaxSpeed = characterComponent.walkingMaxSpeed;
                }
                else
                {
                    characterComponent.GroundMaxSpeed = characterComponent.runningMaxSpeed;
                }

                SystemAPI.SetComponent(player.ControlledCharacter, characterControl);
                SystemAPI.SetComponent(player.ControlledCharacter, characterComponent);
            }
        }
    }
}