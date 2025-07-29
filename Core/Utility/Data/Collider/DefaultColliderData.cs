using System;
using UnityEngine;

[Serializable]
public class DefaultColliderData
{
    [field: SerializeField] public float Height { get; private set; } = 1.89f;
    [field: SerializeField] public float CenterY { get; private set; } = -0.04f;
    [field: SerializeField] public float Radius { get; private set; } = 0.47f;
}
