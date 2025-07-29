using System;
using UnityEngine;

[Serializable]
public class SlopeDetectionData 
{
    [field: Header("Slope Detection")]
    [field: SerializeField]
    [field: Range(0f, 90f)]
    public float SlopeRayAngle { get; private set; } = 35f;

    [field: SerializeField]
    [field: Range(0f, 10f)]
    public float SlopeRayLength { get; private set; } = 1.5f;
   
}
