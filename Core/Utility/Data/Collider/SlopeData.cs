using System;
using UnityEngine;

[Serializable]
public class SlopeData
{
    [field: SerializeField] [field: Range(0f, 1f)] public float SlopeHeightPercentage { get; private set; } = 0.25f;
    
}



