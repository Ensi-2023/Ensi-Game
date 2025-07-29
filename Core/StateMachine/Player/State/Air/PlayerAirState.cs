using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerAirState : PlayerMovementState
{
    public PlayerAirState(PlayerStateMashine statesMashine) : base(statesMashine)
    {
    }

    public override void EnterState()
    {
        if (PlayerStatesMashine.ReusableData.CooldownJumpStart == false)
        {
            PlayerStatesMashine.ReusableData.DirectionJump = PlayerStatesMashine.ReusableData.Velocity;
        }
        
        base.EnterState();
        
        
        PlayerStatesMashine.ReusableData.SoundStepVolume = 0.6f;
    }

    

    
    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        
        PlayerStatesMashine.Player.Input.PlayerActions.Crouch.canceled += OnCrouchCanceled;
        PlayerStatesMashine.Player.Input.PlayerActions.Crouch.started += OnCrouchStarted; 
    }

    private void OnCrouchStarted(InputAction.CallbackContext obj)
    {
        PlayerStatesMashine.ReusableData.ShouldCrouch = true;
    }

    private void OnCrouchCanceled(InputAction.CallbackContext obj)
    {
        PlayerStatesMashine.ReusableData.ShouldCrouch = false;
    }


    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        
        PlayerStatesMashine.Player.Input.PlayerActions.Crouch.canceled -= OnCrouchCanceled;
        PlayerStatesMashine.Player.Input.PlayerActions.Crouch.started -= OnCrouchStarted; 
    }

    protected virtual void OnMove()
    {
        
        if (PlayerStatesMashine.ReusableData.ShouldCrouch)
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.CrouchState); //crouch
            return;
        }
        
        if (PlayerStatesMashine.ReusableData.ShouldSprint)
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.SprintState); //sprint
            return;
        }

        if (PlayerStatesMashine.ReusableData.ShouldWalk)
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.WalkState); //walk
            return;
        }
        
        PlayerStatesMashine.Change(PlayerStatesMashine.RunState); //run

    }

    protected override void OnGroundEnter(Collider collider)
    {
        Debug.Log("CONTAAAAAAAAAAAAAAAACT");
        
        
        PlayerStatesMashine.ReusableData.Grounded = true;
        PlayerStatesMashine.Change(PlayerStatesMashine.IdleState);
    }
}
