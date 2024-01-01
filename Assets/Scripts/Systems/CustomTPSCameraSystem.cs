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

namespace MyTPS
{
    [UpdateAfter(typeof(GameInputSystem))]
    public partial class CustomTPSCameraSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var cameraQuery = SystemAPI.QueryBuilder()
                .WithAll<CustomTPSCamera, CustomTPSCameraTarget>().Build();

            var cameraEntities = cameraQuery.ToEntityArray(Allocator.Temp);
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var cameraEntity in cameraEntities)
            {
                var tpsCamera = SystemAPI.GetComponentRW<CustomTPSCamera>(cameraEntity);
                var tpsCameraTarget = SystemAPI.GetComponentRO<CustomTPSCameraTarget>(cameraEntity);
                var targetEntity = tpsCameraTarget.ValueRO.target;
                var targetLocalToWorld = SystemAPI.GetComponentRW<LocalToWorld>(targetEntity);
                var targetWorldPosition = targetLocalToWorld.ValueRO.Position;
                var lookingInput = tpsCamera.ValueRO.lookingInput;
                var rotateSpeed = tpsCamera.ValueRO.rotateSpeed;
                var elevationMin = tpsCamera.ValueRO.elevationMin;
                var elevationMax = tpsCamera.ValueRO.elevationMax;
                tpsCamera.ValueRW.mode = tpsCamera.ValueRO.aimPressed ? CameraMode.Aim : CameraMode.Normal;
                //calculate transform
                switch (tpsCamera.ValueRO.mode)
                {
                    case CameraMode.Normal:
                        {
                            //calculate input..
                            tpsCamera.ValueRW.distance  += tpsCamera.ValueRO.zoomValue;
                            tpsCamera.ValueRW.polar     += lookingInput.x * rotateSpeed * deltaTime;
                            tpsCamera.ValueRW.elevation += -lookingInput.y * rotateSpeed * deltaTime;

                            tpsCamera.ValueRW.elevation = math.clamp(tpsCamera.ValueRW.elevation, elevationMin, elevationMax);
                            tpsCamera.ValueRW.polar     = (tpsCamera.ValueRW.polar % 360f);

                            var radius          = tpsCamera.ValueRO.distance;
                            var elevation       = math.radians(tpsCamera.ValueRO.elevation);
                            var polar           = math.radians(tpsCamera.ValueRO.polar);
                            var spherePosition  = MathUtility.GetSphericalCoordinatesPosition(radius, elevation, polar);
                            var nextPosition    = targetWorldPosition + spherePosition;

                            Camera.main.transform.position = nextPosition;
                            Camera.main.transform.LookAt(targetWorldPosition, Vector3.up);
                        }
                        break;
                    case CameraMode.Aim:
                        {
                            var targetForward = targetLocalToWorld.ValueRO.Forward;
                            var targetRight = targetLocalToWorld.ValueRO.Right;
                            var targetUp = targetLocalToWorld.ValueRO.Up;

                            var aimOffset = tpsCamera.ValueRO.aimOffset;
                            var relatedAimCameraPosition = aimOffset.x * targetRight + aimOffset.y * targetUp + aimOffset.z * targetForward;
                            var lookingPosition = targetWorldPosition + targetForward * 1000f;
                            Camera.main.transform.position = targetWorldPosition + relatedAimCameraPosition;
                            Camera.main.transform.LookAt(lookingPosition, Vector3.up);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
