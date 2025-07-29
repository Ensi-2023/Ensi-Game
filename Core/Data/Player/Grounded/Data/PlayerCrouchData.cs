using System;
using UnityEngine;

[Serializable]
public class PlayerCrouchData
{

    [field: SerializeField] [field: Range(0, 10f)] public float StepInterval { get; private set; } = 1f;

    [field: SerializeField]
    [field: Range(0, 11f)]
    public float StepVolume { get; set; } = 0.2f;

    [field: SerializeField]
    [field: Range(0f, 2f)]
    public float CrouchSize { get; private set; } = 0.6f;

    [field: SerializeField]
    [field: Range(0f, 15f)]
    public float CrouchSpeed { get; private set; } = 1.5f;

    [field: SerializeField]
    [field: Range(0f, 5f)]
    public float MoveSpeed { get; private set; } = 1f;
    

    [field: SerializeField]
    [field: Range(0f, 5f)]
    public float DefaultSize { get; private set; } = 1.89f;
    
    
    [field: SerializeField]
    [field: Range(-1f, 1f)]
    public float OriginalCentreCollider { get; private set; } = -0.04f;



    [field: SerializeField] public float StandCameraPosition { get; private set; } = 1.7f;

    [field: SerializeField] public float CrouchCameraPosition { get; private set; } = 0.8f;
    
    [field: SerializeField] public float CrouchCameraPositionOnMove { get; private set; } = 0.1f;
    
    [field: SerializeField]
    [field: Range(-10f, 15f)]
    public float CameraTransitionSpeed { get; private set; } = 1.5f;
    
    
    
    [field:Header("Camera Shake")]
    [field: SerializeField][field: Range(0, 1f)] public float SmoothingWobbleFactor  { get; private set; } = 0.002f;
    [field: SerializeField][field: Range(0, 1f)] public float SmoothingWobblePositionFactor  { get; private set; } = 0.015f;
    [field: SerializeField][field: Range(0, 10f)] public float SmoothingWobblePositionSpeed  { get; private set; } = 2f;
   
}
