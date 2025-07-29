using System;
using UnityEngine;

[Serializable]
public class PlayreProneData
{
    [field: SerializeField] [field:Range(0f,6f)] public float ProneSpeed { get; private set; } = 2f;
    [field: SerializeField] [field:Range(0f,2f)] public float ProneSize { get; private set; } = 1.19f;
    [field: SerializeField] [field:Range(-1f,1f)]public float GroundedOffset { get; private set; } = -0.28f;
    [field: SerializeField] [field:Range(0f,2f)] public float ProneScale { get; private set; } = 0.35f;
    [field: SerializeField] [field:Range(0f,1f)] public float ProneLocalPosition { get; private set; } = 0.55f;
}
