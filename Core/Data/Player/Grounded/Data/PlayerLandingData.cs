using System;
using UnityEngine;

[Serializable]
public class PlayerLandingData
{
    [field: SerializeField] private PlayerCameraShake cameraShake;
    public PlayerCameraShake CameraShake => cameraShake;
}
