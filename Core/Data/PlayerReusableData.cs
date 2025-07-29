using Unity.Mathematics;
using UnityEngine;

public class PlayerReusableData
{
    private PlayerDataScriptableObject _data;

    /*region Reusable*/

    public float AccumulatedRecoil; // Накопленная отдача
    
    internal Vector2 MoveDirection;
    internal Vector2 LookDirection;
    internal float currentMoveSpeed;
    internal float currentModifyMoveSpeed;
   

    internal Vector3 Velocity;
    internal Vector2 MouseXY;
    internal Vector2 Rotation = new Vector2();


    internal bool ShouldSprint;
    internal bool ShouldWalk;
    internal bool ShouldCrouch;
    internal bool ShouldIdle;
    internal bool ShouldRun;
    internal bool Grounded;
    internal bool ShouldProne;
    internal bool Aiming;
    internal bool Attack;
    internal bool Reload;
    internal bool Jumping;
    internal bool Falling;

    internal Vector2 MousePosition;
    internal Vector3 DirectionJump;
    
    internal bool CooldownJumpStart;
    //internal float WaitJumpTime;
    internal bool Reloading;
    //internal float AmountToLift;
    internal float TimerFalling = 0f;
    internal float AngleNormal;
    internal float LastFallDistance;
    internal Vector3 GroundNormal { get; set; }


    public WeaponStateEnum.MoveState HandMoveState { get; set; }
    public bool IsWeaponInHand { get; set; }
    public IWeapon DataWeapon { get; set; }
   
    public float VerticalRecoilProgress { get; set; }
    public float HorizontalRecoilProgress { get; set; }
    public float CurrentVerticalRecoilAngle { get; set; }
    public float CurrentHorizontalRecoilAngle { get; set; }
    public Quaternion CurrentHorizontalRecoilAngleQuatr { get; set; }
    public Vector3 HorizontalVelocity { get; set; }
    public float VerticalVelocity { get; set; } = 0f;
    public float TargetHeight { get; set; }
    public Vector3 СurrentWeaponKickPosition { get; set; }
    public Vector3 CurrentWeaponKickRotation { get; set; }

    public float WalkCycleTime { get; set; } = 0f;
    public Vector3 WalkOffset { get; set; }
    public Vector3 BasePosition { get; set; }

    // Текущие смещения от sway
    internal Vector3 swayPositionOffset;
    internal Vector3 swayRotationOffset;
    
    // Параметры для плавного применения
    internal Vector3 targetSwayPosition;
    internal Vector3 targetSwayRotation;

   

    public float OriginalCenterY { get; set; }
    public bool Fire { get; set; }

    public float RecoilAngle { get; set; }
    public Quaternion RecoilAngleRotation { get; set; } = Quaternion.identity;
    public Quaternion SprayRotationOffset { get; set; }
    public bool IsSprayApplied { get; set; }
    public float VerticalSpread { get; set; }
    public float HorizontalSpread { get; set; }
    public float TimeSprayProgress { get; set; }

    public Vector3 CameraBasePosition = new Vector3(0, 0.13f, 0);

    public float LastShotTime { get; set; } 
    private float СurrentRecoilAngle = 0f;
    
 
  internal bool BackSlopeHit;
  internal bool LeftSlopeHit;
  internal bool RightSlopeHit;
  internal bool FrontSlopeHit;
  
  internal float CurrentCrouchSize;

  /*endregion*/
  
  
  internal GameObject CurrentItemTarget;
  


  public PlayerReusableData(PlayerController _player)
   {
       _data = _player.Data;
   }

 


   internal void SetCurrentSpeed(float speed)
   {
  
       if (speed >= _data.GroudedData.SprintData.Speed)
       {
           speed = _data.GroudedData.SprintData.Speed;
       }
       else
       {
           if (speed <= 0f)
           {
               speed = 0f;
           }
       }
  
       currentMoveSpeed = speed;
   }
   
  
  
  public Vector3 CurrentKickOffset { get; set; } // Текущее смещение от отдачи
  public Vector3 TargetKickOffset { get; set; }  // Целевое смещение от отдачи
  public bool IsKickApplied { get; set; }    
  
  public float VerticalrecoilProg { get; set; }
  public bool ShouldReturn { get; set; }
  public Quaternion TargetKickRotation { get; set; }
  public Quaternion CurrentKickRotateOffset { get; set; }
  public float ParabolaProgress { get; set; }
  public float BreathingTimer { get; set; }
  public float ProgressPosition { get; set; }
  public float SmoothingWobbleFactor { get; set; }
  public float BreathingTimerZ { get; set; }
  public float BreathingTimerX { get; set; }
  public float SmoothingWobblePositionFactor { get; set; }
  public float SmoothingWobblePositionSpeed { get; set; }
  public float BreathingTimerY { get; set; }
  public float SoundStepVolume { get; set; }


  internal void SetDefaultValues()
  {
      IsWeaponInHand = false;
      DataWeapon = null;
      VerticalRecoilProgress = 0f;
      HorizontalRecoilProgress = 0f;
      CurrentVerticalRecoilAngle = 0f;
      HorizontalVelocity = Vector3.zero;
      CurrentHorizontalRecoilAngle = 0f;
      VerticalVelocity = 0f;
      TargetHeight = 0f;
      WalkCycleTime = 0f;
      ShouldReturn = false;
      IsKickApplied = false;
      VerticalrecoilProg = 0f;
      СurrentWeaponKickPosition = Vector3.zero;
      CurrentWeaponKickRotation = Vector3.zero;
      WalkOffset = Vector3.zero;
      BasePosition = Vector3.zero;
      swayPositionOffset = Vector3.zero;
      TargetKickOffset = Vector3.zero;
      swayRotationOffset = Vector3.zero;
      targetSwayPosition = Vector3.zero;
      targetSwayRotation = Vector3.zero;
      CurrentKickOffset = Vector3.zero;
      VerticalSpread = 0f;
      HorizontalSpread = 0f;
      CurrentHorizontalRecoilAngleQuatr = quaternion.identity;
     // RecoilAngleRotation = quaternion.identity;
     //  RecoilAngle = 0f;
      TargetKickRotation = quaternion.identity;
      CurrentKickRotateOffset = quaternion.identity;


  }
  
}
