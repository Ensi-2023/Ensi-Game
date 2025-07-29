using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintState : PlayerMovingState
{
    private PlayerSprintData _data;
    public PlayerSprintState(PlayerStateMashine statesMashine) : base(statesMashine)
    {
        _data = statesMashine.Player.Data.GroudedData.SprintData;
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
        PlayerStatesMashine.Player.HandStateMashine.MoveState(WeaponStateEnum.MoveState.Sprint);
        StartAnimation(animationData.SprintParamentHash);
    }

    public override void ExitState()
    {
        base.ExitState();
        
        StopAnimation(animationData.SprintParamentHash);
    }

    public override void Update()
    {
        base.Update();

        if (PlayerStatesMashine.ReusableData.ShouldSprint) return;

        OnMove();

    }


    protected override void OnAttackStarted(InputAction.CallbackContext obj)
    {
        base.OnAttackStarted(obj);

        if (PlayerStatesMashine.Player.HandStateMashine.CurrentWeapon!=null)
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.RunState);
        }
        
    }


    protected override void OnAimingStarted(InputAction.CallbackContext obj)
    {
        base.OnAimingStarted(obj);
        
        if (PlayerStatesMashine.Player.HandStateMashine.CurrentWeapon!=null)
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.RunState);
        }
    }
}
