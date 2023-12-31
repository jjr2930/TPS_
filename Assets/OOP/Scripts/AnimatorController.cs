using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace MyTPS
{
    public class AnimatorController : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] float movementEpslon;

        float sqrMovementEpsilon;
        public void Reset()
        {
            animator = GetComponent<Animator>();

        }

        public void Awake()
        {
            CalculateSqrs();
        }

#if UNITY_EDITOR
        public void Update()
        {
            CalculateSqrs();
        }
#endif

        private void CalculateSqrs()
        {
            sqrMovementEpsilon = movementEpslon * movementEpslon;
        }

        public void OnMovement(CallbackContext context)
        {
            var movement = context.ReadValue<Vector2>();            
            animator.SetBool(AnimatorHash.Moving, movement.sqrMagnitude > sqrMovementEpsilon);
        }
    }
}
