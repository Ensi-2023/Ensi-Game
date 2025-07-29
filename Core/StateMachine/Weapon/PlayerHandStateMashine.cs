using UnityEngine;

public class PlayerHandStateMashine : HandState
{
    internal PlayerController Player { get; }
    internal PlayerReusableData ReusableData { get; }

    internal PlayerWeaponIdleState IdleState { get; }
    internal PlayerWeaponCrouchState CrouchState { get; }
    internal PlayerWeaponRunState   RunState { get; }
    internal PlayerWeaponWalkState   WalkState { get; }
    
    internal PlayerWeaponSprintState SprintState { get; }
    internal PlayerWeaponProneState ProneState { get; }
    internal PlayerWeaponFallingState FallingState { get; }
    internal PlayerWeaponJumpState JumpState { get; }
    internal PlayerWeaponLandingState LandingState { get; }
    
    public PlayerHandStateMashine(PlayerController _player)
    {
        Player = _player;
        ReusableData = _player.StateMashine.ReusableData;
        CrouchState = new PlayerWeaponCrouchState(this);
        IdleState = new PlayerWeaponIdleState(this);
        RunState = new PlayerWeaponRunState(this);
        WalkState = new PlayerWeaponWalkState(this);
        SprintState = new PlayerWeaponSprintState(this);
        ProneState = new PlayerWeaponProneState(this);
        FallingState = new PlayerWeaponFallingState(this);
        JumpState = new PlayerWeaponJumpState(this);
        LandingState = new PlayerWeaponLandingState(this);
    }
}
