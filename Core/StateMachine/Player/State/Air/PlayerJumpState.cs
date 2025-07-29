using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    private PlayerJumpData jumpData;
    public PlayerJumpState(PlayerStateMashine statesMashine) : base(statesMashine)
    {
        jumpData = PlayerStatesMashine.Player.Data.AirData.JumpData;
    }

    public override void EnterState()
    {

        PlayerStatesMashine.ReusableData.Grounded = false;
        
        base.EnterState();
        
        PlayerStatesMashine.Player.FootstepSound.PlayJumpSound();
        
        StartAnimation(animationData.JumpParamentHash);
        PlayerStatesMashine.ReusableData.Jumping = true;
        PlayerStatesMashine.Player.HandStateMashine.MoveState(WeaponStateEnum.MoveState.Jump);
        
        PlayerStatesMashine.Player.TransformShake.ShakeRotateCamera(PlayerStatesMashine.Player.CameraShake,jumpData.CameraShake);
        
        OnJump();
        
    }

    public override void ExitState()
    {
        base.ExitState();
        PlayerStatesMashine.ReusableData.Jumping = false;
        StopAnimation(animationData.JumpParamentHash);
    }

    public override void Update()
    {
        base.Update();
        
        
        if (PlayerStatesMashine.ReusableData.VerticalVelocity < 0)
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.FallState); //fall
        }
    }
    
    internal override void PlayerMove()
    {
      
    }
    
    internal override void ApplyPintoGround()
    {
        
    }

    private void OnJump()
    {
        PlayerStatesMashine.ReusableData.VerticalVelocity = jumpData.JumpForce;
    }
}
