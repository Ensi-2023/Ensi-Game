using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouchState : PlayerMovingState
{
    private PlayerCrouchData _data;
    public PlayerCrouchState(PlayerStateMashine statesMashine) : base(statesMashine)
    {
        _data = statesMashine.Player.Data.GroudedData.CrouchData;
    }
    
    public override void EnterState()
    {
        PlayerStatesMashine.ReusableData.SetCurrentSpeed(0);
        base.EnterState();
        
        
        PlayerStatesMashine.ReusableData.SmoothingWobbleFactor = 0f;
        PlayerStatesMashine.ReusableData.SmoothingWobblePositionFactor = 0f;
        PlayerStatesMashine.ReusableData.SmoothingWobblePositionSpeed= 0f;
        PlayerStatesMashine.Player.FootstepSound.UpdateStepInterval(0);
        
        PlayerStatesMashine.Player.HandStateMashine.MoveState(WeaponStateEnum.MoveState.Crouch);
        StartAnimation(animationData.CrouchParamentHash);
    }

    public override void Update()
    {
        base.Update();

        if (PlayerStatesMashine.ReusableData.MoveDirection != Vector2.zero)
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.CrouchMove);
            return;
        }


        if (PlayerStatesMashine.ReusableData.ShouldCrouch) return;
        OnMove();
    }



    public override void ExitState()
    {
        base.ExitState();
        StopAnimation(animationData.CrouchParamentHash);
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

