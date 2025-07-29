using System;
using UnityEngine;
[Serializable]
public class PlayerJumpData
{
    [field: SerializeField]
    [field: Range(0f, 10f)]
    public float JumpForce { get; private set; } = 7f;
    
    [field:SerializeField]
    [field:Range(0f, 10f)]
    public float WeaponJumpForce { get; private set; } = 5f;
    
    
    [field: SerializeField] private PlayerCameraShake cameraShake;
    public PlayerCameraShake CameraShake => cameraShake;
}
