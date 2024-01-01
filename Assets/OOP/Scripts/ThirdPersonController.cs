using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.InputSystem.InputAction;

namespace MyTPS
{
    public enum AimMode
    {
        None,
        Aim,
        Zoom1,
        Zoom2,
        Zoom3
    }
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("it not set, will be set when you start")]
        [SerializeField] Camera mainCamera;
        [SerializeField] CharacterController characterController;
        [SerializeField] Transform lookTarget;

        [Header("Movement")]
        [SerializeField] float accel = 10f;
        [SerializeField] float speedLimit = 20f;
        [SerializeField] float friction = 5f;
        [SerializeField] float movingEpslion = 0.0001f;

        [Header("Rotation")]
        [SerializeField] float rotationSpeed = 90f;
        [SerializeField] float thetaMin = 1f;
        [SerializeField] float thetaMax = 89f;

        [Header("Camera")]
        [SerializeField] float noneDistance;
        [SerializeField] Vector3 noneOffet = new Vector3(0f, 5f, -5f);
        [SerializeField] float aimDistance;
        [SerializeField] Vector3 aimOffset = new Vector3(1f, 1f, -1f);
        [SerializeField] float theta = 0f;
        [SerializeField] float phi = 0f;

        [Header("Input")]
        [SerializeField] PlayerInput playerInput;
        [SerializeField] Vector2 movingInput;
        [SerializeField] Vector2 lookingInput;
        [SerializeField] float movingThreshold;
        [SerializeField] float lookingThreshold;

        [Header("ETC")]
        [SerializeField] AimMode aimMode;

        float sqrMovingThreshold;
        float sqrLookingThreshold;
        float sqrMovingEpsilon;
        float sqrSpeedLimit;

        public void Reset()
        {
            characterController = GetComponent<CharacterController>();
        }

        public void Awake()
        {
            if (!mainCamera)
            {
                mainCamera = Camera.main;
            }

            if (!lookTarget)
            {
                lookTarget = this.transform;
            }

            CalculateSqrs();
        }

        private void Update()
        {
#if UNITY_EDITOR
            CalculateSqrs();
#endif
            UpdateCharacterController();
            UpdateCamera();
        }

        private void CalculateSqrs()
        {
            sqrMovingThreshold = movingThreshold * movingThreshold;
            sqrLookingThreshold = lookingThreshold * lookingThreshold;
            sqrMovingEpsilon = sqrMovingEpsilon * sqrMovingEpsilon;
            sqrSpeedLimit = speedLimit * speedLimit;    
        }

        private void UpdateCharacterController()
        {
            float deltaTime = Time.deltaTime;
            Vector3 nextVelocity = Vector3.zero;
            Vector3 currentVertical = characterController.velocity;
            currentVertical.x = 0;
            currentVertical.z = 0;

            if(movingInput.sqrMagnitude > sqrMovingEpsilon)
            {
                nextVelocity = transform.forward * movingInput.y + transform.right * movingInput.x;
                if (nextVelocity.sqrMagnitude > sqrSpeedLimit)
                    nextVelocity = nextVelocity.normalized * speedLimit;
            }

            nextVelocity += currentVertical + Physics.gravity * deltaTime;

            characterController.Move(nextVelocity * deltaTime);
        }

        private void UpdateCamera()
        {
            var relativeOffset = Vector3.zero;

            var nextCameraPosition = Vector3.zero;

            var selectedOffset = Vector3.zero;
            var lookTargetPosition = Vector3.zero;
            switch (aimMode)
            {
                case AimMode.None:
                    {
                        selectedOffset = noneOffet;

                        lookTargetPosition = lookTarget.position;

                        phi += lookingInput.x ;
                        phi = (phi > 360f) ? 0f : phi;

                        theta += lookingInput.y;
                        theta = Mathf.Clamp(theta, thetaMin, thetaMax);

                        Vector3 spherePosition = MathUtility.GetSphericalCoordinatesPosition(noneDistance, theta * Mathf.Deg2Rad, phi * Mathf.Deg2Rad);

                        nextCameraPosition = transform.position + spherePosition;
                    }
                    break;
                case AimMode.Aim:
                    {
                        selectedOffset = aimOffset;

                        lookTargetPosition = lookTarget.position + lookTarget.forward * 1000f;

                        nextCameraPosition = transform.position;
                    }
                    break;
                case AimMode.Zoom1:
                    break;
                case AimMode.Zoom2:
                    break;
                case AimMode.Zoom3:
                    break;
                default:
                    break;
            }
 


            relativeOffset += transform.right * selectedOffset.x;
            relativeOffset += transform.up * selectedOffset.y;
            relativeOffset += transform.forward * selectedOffset.z;

            nextCameraPosition += relativeOffset;
            mainCamera.transform.position = nextCameraPosition;

            mainCamera.transform.LookAt(lookTargetPosition);
        }

        public void OnMovement(CallbackContext context)
        {
            movingInput = context.ReadValue<Vector2>();
        }

        public void OnAim(CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Disabled:
                    break;
                case InputActionPhase.Waiting:
                    break;
                case InputActionPhase.Started:
                    if(aimMode == AimMode.None )
                    {
                        aimMode = AimMode.Aim;
                    }
                    break;
                case InputActionPhase.Performed:
                    break;
                case InputActionPhase.Canceled:
                    if (aimMode != AimMode.None)
                        aimMode = AimMode.None;
                    break;
                default:
                    break;
            }
        }

        public void OnLook(CallbackContext context)
        {
            lookingInput = context.ReadValue<Vector2>();    
        }
    }
}
