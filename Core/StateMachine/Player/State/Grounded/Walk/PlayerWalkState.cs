using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerMovingState
{
    private PlayerWalkData _data;
    public PlayerWalkState(PlayerStateMashine statesMashine) : base(statesMashine)
    {
        _data = statesMashine.Player.Data.GroudedData.WalkData;
    }

    public override void EnterState()
    {

        PlayerStatesMashine.ReusableData.SetCurrentSpeed(_data.Speed);
        base.EnterState();
        
        PlayerStatesMashine.Player.FootstepSound.UpdateStepInterval(_data.StepInterval);
        PlayerStatesMashine.ReusableData.SmoothingWobbleFactor = _data.SmoothingWobbleFactor;
        PlayerStatesMashine.ReusableData.SmoothingWobblePositionFactor = _data.SmoothingWobblePositionFactor;
        PlayerStatesMashine.ReusableData.SmoothingWobblePositionSpeed= _data.SmoothingWobblePositionSpeed;
        PlayerStatesMashine.ReusableData.SoundStepVolume = _data.StepVolume;
        PlayerStatesMashine.Player.HandStateMashine.MoveState(WeaponStateEnum.MoveState.Walk);
        StartAnimation(animationData.WalkParamentHash);

        if (PlayerStatesMashine.ReusableData.MoveDirection.magnitude == 0)
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.IdleState);
        }


    }

    public override void ExitState()
    {
        base.ExitState();
        
        StopAnimation(animationData.WalkParamentHash);
    }

    public override void Update()
    {
        base.Update();
        
        if (PlayerStatesMashine.ReusableData.ShouldWalk) return;
        OnMove();
    }


}
