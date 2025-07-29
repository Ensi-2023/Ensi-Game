using System;
using UnityEngine;

[Serializable]
public class PlayerGroundedData 
{
    [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
    [field:SerializeField] public PlayerSlopeData SlopeData { get; private set; }
    [field: SerializeField] public PlayerRunData RunData { get; private set; }
    [field: SerializeField] public PlayerWalkData WalkData { get; private set; }
    [field: SerializeField] public PlayerSprintData SprintData { get; private set; }
    
    [field: SerializeField] public PlayerCrouchData CrouchData { get; private set; }
    [field: SerializeField] public PlayreProneData ProneData { get; private set; }

    [field:SerializeField] public PlayerLandingData LandingData { get; private set; }
   

    [field: SerializeField][field: Range(0, 10f)] public float Drag { get; private set; } = 10f;
    [field: SerializeField][field: Range(60, 110f)] public float Fov { get; private set; } = 60f;
    
    [field:Header("Slope Handling")]
    [field: SerializeField][field: Range(0, 90f)] public float MaxSlopeAngle { get; private set; } = 45f; // Максимальный угол для ходьбы
    [field: SerializeField][field: Range(0, 25f)] public float SlideSpeed { get; private set; } = 5f; 
    
    
    [field: SerializeField][field: Range(-25f, 25f)] public float Gravity { get; private set; } = -9.81f;
   
    [field: SerializeField][field: Range(0f, 5f)] public float GroundToFallDistance { get; private set; } = 1f;


    [field: SerializeField] public AnimationCurve SlopeSpeedAngle { get; private set; }
    
    [field: Header("Slope Detection")]
    [field: SerializeField] public float SlopeRayAngle { get; private set; }= 35f;
    [field: SerializeField] public float SlopeRayLength { get; private set; }= 1.5f;
    
    
    
    
    
    

}
