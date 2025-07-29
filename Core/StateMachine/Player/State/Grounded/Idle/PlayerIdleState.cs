using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMashine statesMashine) : base(statesMashine)
    {
    }

    public override void EnterState()
    {
        PlayerStatesMashine.ReusableData.SetCurrentSpeed(0f);
        base.EnterState();
        PlayerStatesMashine.ReusableData.ShouldIdle = true;
        
        PlayerStatesMashine.Player.HandStateMashine.MoveState(WeaponStateEnum.MoveState.Idle);

        PlayerStatesMashine.ReusableData.SmoothingWobbleFactor = 0f;
        PlayerStatesMashine.ReusableData.SmoothingWobblePositionFactor = 0f;
        PlayerStatesMashine.ReusableData.SmoothingWobblePositionSpeed= 0f;
        
        PlayerStatesMashine.Player.FootstepSound.UpdateStepInterval(0);
        
        StartAnimation(animationData.IdleParamentHash);
        
    }



    public override void ExitState()
    {
        base.ExitState();
        PlayerStatesMashine.ReusableData.ShouldIdle = false;
        StopAnimation(animationData.IdleParamentHash);
    }


    public override void Update()
    {
        base.Update();
        if(PlayerStatesMashine.ReusableData.MoveDirection==Vector2.zero)
            return;
        
        OnMove();
    }
}
