using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouchMove : PlayerMovingState
{
    private PlayerCrouchData _data;
    public PlayerCrouchMove(PlayerStateMashine statesMashine) : base(statesMashine)
    {
        _data = statesMashine.Player.Data.GroudedData.CrouchData;
    }


    public override void EnterState()
    {
        PlayerStatesMashine.ReusableData.SetCurrentSpeed(_data.MoveSpeed);
        
        base.EnterState();

        PlayerStatesMashine.ReusableData.SoundStepVolume = _data.StepVolume;
        PlayerStatesMashine.ReusableData.SmoothingWobbleFactor = _data.SmoothingWobbleFactor;
        PlayerStatesMashine.ReusableData.SmoothingWobblePositionFactor = _data.SmoothingWobblePositionFactor;
        PlayerStatesMashine.ReusableData.SmoothingWobblePositionSpeed= _data.SmoothingWobblePositionSpeed;
        PlayerStatesMashine.Player.FootstepSound.UpdateStepInterval(_data.StepInterval);
        PlayerStatesMashine.Player.HandStateMashine.MoveState(WeaponStateEnum.MoveState.Crouch);
        StartAnimation(animationData.CrouchParamentHash);
        
    }

    public override void Update()
    {
        base.Update();
        
        if (PlayerStatesMashine.ReusableData.ShouldCrouch) return;
        OnMove();
    }
    
    protected override void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        base.OnMoveCanceled(obj);
        
        if (PlayerStatesMashine.ReusableData.ShouldCrouch)
        { 
            PlayerStatesMashine.Change(PlayerStatesMashine.CrouchState);
        }
    }
    
    
    protected override void OnSprintStarted(InputAction.CallbackContext obj)
    {
        
    }

    protected override void JumpOnPerformed(InputAction.CallbackContext obj)
    {
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //CrouchChecker();
    }
    
}
