using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerMovingState
{
    private PlayerRunData _data;
    public PlayerRunState(PlayerStateMashine statesMashine) : base(statesMashine)
    {
        _data = statesMashine.Player.Data.GroudedData.RunData;
    }
    
    public override void EnterState()
    {

        PlayerStatesMashine.ReusableData.SetCurrentSpeed(_data.Speed);
        base.EnterState();


        PlayerStatesMashine.Player.FootstepSound.UpdateStepInterval(_data.StepInterval);
            
        StartAnimation(animationData.RunParamentHash);
        
        PlayerStatesMashine.ReusableData.SoundStepVolume = _data.StepVolume;
        
        PlayerStatesMashine.ReusableData.SmoothingWobbleFactor = _data.SmoothingWobbleFactor;
        PlayerStatesMashine.ReusableData.SmoothingWobblePositionFactor = _data.SmoothingWobblePositionFactor;
        PlayerStatesMashine.ReusableData.SmoothingWobblePositionSpeed= _data.SmoothingWobblePositionSpeed;
        
        PlayerStatesMashine.Player.HandStateMashine.MoveState(WeaponStateEnum.MoveState.Run);
        PlayerStatesMashine.ReusableData.ShouldRun = true;

        if (PlayerStatesMashine.ReusableData.MoveDirection == Vector2.zero)
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.IdleState);
        }

    }

    public override void ExitState()
    {
        base.ExitState();

        PlayerStatesMashine.ReusableData.ShouldRun = false;
        StopAnimation(animationData.RunParamentHash);
    }

    protected override void OnMoveCanceled(InputAction.CallbackContext obj)
    {
      
        
        base.OnMoveCanceled(obj);
        
        
    }



}
