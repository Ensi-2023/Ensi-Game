using UnityEngine;

public class PlayerStateMashine : PlayerState
{

    internal PlayerController Player { get; }
    internal PlayerReusableData ReusableData { get; }
    
    /*STATE*/
    internal PlayerIdleState IdleState { get; }
    internal PlayerRunState RunState { get; }
    internal PlayerWalkState WalkState { get; } 
    internal PlayerSprintState SprintState { get; }
    
    internal PlayerJumpState JumpState { get; }
    internal PlayerFallState FallState { get; }
    internal PlayerCrouchState CrouchState { get; }
    
    internal PlayerProneState ProneState { get; }
    internal PlayerLandingState LandingState { get; }
    internal PlayerCrouchMove   CrouchMove { get; }

    
    /*******/

    public PlayerStateMashine(PlayerController _player)
    {
        ReusableData = new PlayerReusableData(_player);
        Player = _player;
        
        IdleState = new PlayerIdleState(this);
        WalkState= new PlayerWalkState(this);
        SprintState= new PlayerSprintState(this);   
        RunState = new PlayerRunState(this);
        
        FallState = new PlayerFallState(this);
        JumpState = new PlayerJumpState(this);
        CrouchState = new PlayerCrouchState(this);
        LandingState = new PlayerLandingState(this);
        ProneState = new PlayerProneState(this);
        CrouchMove = new PlayerCrouchMove(this);
    }

}
