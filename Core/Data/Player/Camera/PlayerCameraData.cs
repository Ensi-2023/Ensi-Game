using System;
using UnityEngine;

[Serializable]
public class PlayerCameraData 
{
    [field: SerializeField][field: Range(0f, 1000f)] public float SensetivityX { get; private set; } = 400f;
    [field: SerializeField][field: Range(0f, 1000f)] public float SensetivityY { get; private set; } = 400f;
    [field: SerializeField][field: Range(0f, 2f)] public float RotationSmoothness { get; private set; } = 0.2f;
    [field: SerializeField][field: Range(-500f, 50f)] public float RayVerticalRayOffset { get; private set; }
}
