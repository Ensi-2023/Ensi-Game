using System;
using UnityEngine;

[Serializable]
public class PlayerRunData 
{
    [field: SerializeField][field: Range(0, 10f)] public float Speed { get; private set; }
    [field: SerializeField][field: Range(0, 10f)] public float AimingSpeed { get; private set; }
    [field: SerializeField][field: Range(0, 10f)] public float WeaponInHandSpeed { get; private set; }

    [field: SerializeField] [field: Range(0, 10f)] public float StepInterval { get; private set; } = 2f;
    [field: SerializeField]
    [field: Range(0, 11f)]
    public float StepVolume { get; set; } = 0.2f;
    
    
    [field:Header("Camera Shake")]
    [field: SerializeField][field: Range(0, 1f)] public float SmoothingWobbleFactor  { get; private set; } = 0.003f;
    [field: SerializeField][field: Range(0, 1f)] public float SmoothingWobblePositionFactor  { get; private set; } = 0.05f;
    [field: SerializeField][field: Range(0, 10f)] public float SmoothingWobblePositionSpeed  { get; private set; } = 5f;

    
}
