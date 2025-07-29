using System;
using UnityEngine;

[Serializable]
public class PlayerFallData
{
   [field: Header("FALL DATA")]
   [field: SerializeField]
   [field: Range(1f, 15f)]
   public float FallSpeedLImit { get; private set; } = 15f;
   
   [field: SerializeField]
   [field: Range(0f, 100f)]
   public float MinimimDistanceToConsoderedHardFall { get; private set; } = 3f;
   
   
   [field: SerializeField]
   [field: Range(0f, 1f)]
   public float FallDelay{ get; private set; } = 0.25f;
}
