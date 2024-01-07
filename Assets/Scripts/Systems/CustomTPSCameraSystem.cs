using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.CharacterController;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.CodeGeneratedJobForEach;
using Unity.Mathematics;
using Unity.Physics.Authoring;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

namespace MyTPS
{
    //[UpdateInGroup(typeof(SimulationSystemGroup))]
    //[UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    //[UpdateAfter(typeof(BasicPlayerVariableStepControlSystem))]
    //[UpdateAfter(typeof(BasicCharacterVariableUpdateSystem))]
    //[UpdateBefore(typeof(TransformSystemGroup))]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    [UpdateAfter(typeof(PlayerAnimatorModelFollowSystem))]

    public partial class CustomTPSCameraSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();


            var cameraQuery = SystemAPI.QueryBuilder()
                .WithAll<CustomTPSCamera, CustomTPSCameraTarget>().Build();
            
            base.RequireForUpdate(cameraQuery);
            base.RequireForUpdate<GameConfigData>();
        }
        protected override void OnUpdate()
        {
            var cameraQuery = SystemAPI.QueryBuilder()
                .WithAll<CustomTPSCamera, CustomTPSCameraTarget>().Build();

            var cameraEntities = cameraQuery.ToEntityArray(Allocator.Temp);
            var deltaTime = SystemAPI.Time.DeltaTime;
            var configs = SystemAPI.GetSingleton<GameConfigData>();

            foreach (var cameraEntity in cameraEntities)
            {
                var tpsCamera = SystemAPI.GetComponentRW<CustomTPSCamera>(cameraEntity);
                var tpsCameraLocal = SystemAPI.GetComponentRW<LocalTransform>(cameraEntity);
                var tpsCameraWorld = SystemAPI.GetComponentRW<LocalToWorld>(cameraEntity);
                var tpsCameraTarget = SystemAPI.GetComponentRO<CustomTPSCameraTarget>(cameraEntity);
                var targetEntity = tpsCameraTarget.ValueRO.target;
                var targetLocalToWorld = SystemAPI.GetComponentRW<LocalToWorld>(targetEntity);
                var targetWorldPosition = targetLocalToWorld.ValueRO.Position;
                var lookingInput = tpsCamera.ValueRO.lookingInput;
                var rotateSpeed = tpsCamera.ValueRO.rotateSpeed;
                var elevationMin = tpsCamera.ValueRO.elevationMin;
                var elevationMax = tpsCamera.ValueRO.elevationMax;
                var normalOffset = tpsCamera.ValueRO.normalOffset;

                tpsCamera.ValueRW.mode = tpsCamera.ValueRO.aimPressed ? CameraMode.Aim : CameraMode.Normal;

                tpsCamera.ValueRW.distance += tpsCamera.ValueRO.zoomValue;
                tpsCamera.ValueRW.polar += lookingInput.x * rotateSpeed * deltaTime;
                tpsCamera.ValueRW.elevation += -lookingInput.y * rotateSpeed * deltaTime;

                tpsCamera.ValueRW.elevation = math.clamp(tpsCamera.ValueRW.elevation, elevationMin, elevationMax);
                tpsCamera.ValueRW.polar = (tpsCamera.ValueRW.polar % 360f);

                //var radius = tpsCamera.ValueRO.distance;
                var radius = tpsCamera.ValueRO.mode == CameraMode.Aim ? 3f : 5f;
                var elevation = math.radians(tpsCamera.ValueRO.elevation);
                var polar = math.radians(tpsCamera.ValueRO.polar);
                var spherePosition = MathUtility.GetSphericalCoordinatesPosition(radius, elevation, polar);
                var cameraPlanarRotation = MathUtilities.CreateRotationWithUpPriority(math.up(), Camera.main.transform.forward);
                var relativeOffset = float3.zero;
                var nextPosition = targetWorldPosition + spherePosition;

                var cameraPlanarForward = MathUtilities.GetForwardFromRotation(cameraPlanarRotation);
                var cameraPlanarRight = MathUtilities.GetRightFromRotation(cameraPlanarRotation);
                var cameraPlanarUp = MathUtilities.GetUpFromRotation(cameraPlanarRotation);

                relativeOffset += cameraPlanarForward * normalOffset.z;
                relativeOffset += cameraPlanarRight * normalOffset.x;
                relativeOffset += cameraPlanarUp * normalOffset.y;

                var lookPosition = targetWorldPosition + relativeOffset;
                var camToLook = lookPosition - tpsCameraWorld.ValueRO.Position;
                var lookRotation = quaternion.LookRotationSafe(camToLook, math.up());

                Camera.main.transform.position = nextPosition;
                Camera.main.transform.LookAt(targetWorldPosition + relativeOffset, Vector3.up);
                
                var nextRotation = TransformHelpers.LookAtRotation(tpsCameraWorld.ValueRO.Position, lookPosition, math.up());
                tpsCameraLocal.ValueRW = LocalTransform.FromPositionRotation(nextPosition, nextRotation);
            }
        }
    }
}
