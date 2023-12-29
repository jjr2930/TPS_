using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
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


        [Header("Rotation")]
        [SerializeField] float rotationSpeed = 90f;
        [SerializeField] float xMin = 1f;
        [SerializeField] float xMax = 89f;

        [Header("Camera")]
        [SerializeField] Vector3 delta = new Vector3(0f,0f,-5f);
        [SerializeField] Vector3 offSet;

        [Header("Input")]
        [SerializeField] Vector2 movingInput;
        [SerializeField] Vector2 lookingInput;
        [SerializeField] float movingThreshold;
        [SerializeField] float lookingThreshold;


        float sqrMovingThreshold;
        float sqrLookingThreshold;

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

            sqrMovingThreshold = movingThreshold * movingThreshold;
            sqrLookingThreshold = lookingThreshold * lookingThreshold;
        }

        private void Update()
        {
#if UNITY_EDITOR
            sqrMovingThreshold = movingThreshold * movingThreshold;
            sqrLookingThreshold = lookingThreshold * lookingThreshold;
#endif

            UpdateCharacterController();
            UpdateCamera();
        }

        private void UpdateCharacterController()
        {
            characterController.Move(Physics.gravity);
        }

        private void UpdateCamera()
        {
            var relativeDelta = Vector3.zero;
            relativeDelta += transform.right * delta.x;
            relativeDelta += transform.up * delta.y;
            relativeDelta += transform.forward * delta.z;

            var relativeOffset = Vector3.zero;
            relativeOffset += transform.right * offSet.x;
            relativeOffset += transform.up * offSet.y;
            relativeOffset += transform.forward * offSet.z;

            var nextCameraPosition = transform.position + relativeDelta + relativeOffset;
            mainCamera.transform.position = nextCameraPosition;
            mainCamera.transform.LookAt(lookTarget);
        }
    }
}
