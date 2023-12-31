using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct ThirdPersonPlayer : IComponentData
{
    public Entity ControlledCharacter;
    public Entity ControlledCamera;
}

[Serializable]
public struct ThirdPersonPlayerInputs : IComponentData
{
    //character's default locomotion is running
    public bool walkingPressed;
    public float2 MoveInput;
    public float2 CameraLookInput;
    public float CameraZoomInput;
    public FixedInputEvent JumpPressed;
    public FixedInputEvent aimPressed;
}
