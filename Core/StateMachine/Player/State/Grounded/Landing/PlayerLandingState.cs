using UnityEngine;

public class PlayerLandingState : PlayerGroundState
{
    private PlayerLandingData _data;
    
    public PlayerLandingState(PlayerStateMashine statesMashine) : base(statesMashine)
    {      _data = statesMashine.Player.Data.GroudedData.LandingData;
    }

    public override void EnterState()
    {
        base.EnterState();
        PlayerStatesMashine.Player.HandStateMashine.MoveState(WeaponStateEnum.MoveState.Landing);
        PlayerStatesMashine.Player.FootstepSound.PlayLandingSound();
        PlayerStatesMashine.Player.TransformShake.ShakeRotateCamera(PlayerStatesMashine.Player.CameraShake,_data.CameraShake);
    }

    public override void ExitState()
    {
        base.ExitState();
    }
    
    public override void Update()
    {
        base.Update();
        
        if (PlayerStatesMashine.ReusableData.MoveDirection == Vector2.zero)
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.IdleState);
            return;
        }
        
        OnMove();
        
    }
}
