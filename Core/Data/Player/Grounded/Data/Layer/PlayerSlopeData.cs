using System;
using UnityEngine;

[Serializable]
public class PlayerSlopeData 
{    
    [field: SerializeField] [field: Range(0f, 25f)] public float TargetFloatHeight { get; private set; }= 1.3f; // Желаемая высота парения над землёй
    [field: SerializeField] [field: Range(0f, 1000f)] public float MaxLiftSpeed { get; private set; }= 5f;      // Максимальная скорость подъёма/опускания
    [field: SerializeField] [field: Range(0f, 25f)] public float RaycastDistance { get; private set; }= 5f;
   
}
