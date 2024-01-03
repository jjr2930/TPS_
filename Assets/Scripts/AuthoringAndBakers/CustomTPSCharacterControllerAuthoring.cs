using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.CharacterController;
namespace MyTPS
{
    public class CustomTPSCharacterControllerAuthoring : MonoBehaviour
    {
        public AuthoringKinematicCharacterProperties kinematicProperty = AuthoringKinematicCharacterProperties.GetDefault();
        public float rotateSpeed = 90f;
        public float walkingSpeed = 10f;
        public float sprintSpeed = 20f;

        public class Baker : Baker<CustomTPSCharacterControllerAuthoring>
        {
            public override void Bake(CustomTPSCharacterControllerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                KinematicCharacterUtilities.BakeCharacter(this, authoring, authoring.kinematicProperty);

                AddComponent(entity, new CustomTPSCharacterController()
                {
                    rotationSpeed = authoring.rotateSpeed
                });
            }
        }
    }
}
