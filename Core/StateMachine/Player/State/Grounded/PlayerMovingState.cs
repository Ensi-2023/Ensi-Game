using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovingState : PlayerGroundState
{
    public PlayerMovingState(PlayerStateMashine statesMashine) : base(statesMashine)
    {
    }


    public override void Update()
    {
        base.Update();
    }

    protected override void OnSprintStarted(InputAction.CallbackContext obj)
    {
        base.OnSprintStarted(obj);
        if((!PlayerStatesMashine.ReusableData.Aiming && !PlayerStatesMashine.ReusableData.Attack))
              PlayerStatesMashine.Change(PlayerStatesMashine.SprintState); // Sprint
        
    }

    protected override void OnAimingCanceled(InputAction.CallbackContext obj)
    {
        base.OnAimingCanceled(obj);
        
        if(PlayerStatesMashine.ReusableData.ShouldSprint && !PlayerStatesMashine.ReusableData.Attack)
            PlayerStatesMashine.Change(PlayerStatesMashine.SprintState); 
    }

    protected override void OnAttackCanceled(InputAction.CallbackContext obj)
    {
        base.OnAttackCanceled(obj);
        
        if(PlayerStatesMashine.ReusableData.ShouldSprint&& !PlayerStatesMashine.ReusableData.Aiming)
            PlayerStatesMashine.Change(PlayerStatesMashine.SprintState); 
    }

    protected override void OnWalkStarted(InputAction.CallbackContext obj)
    {
        base.OnWalkStarted(obj);
        PlayerStatesMashine.Change(PlayerStatesMashine.WalkState); // Walk
    }

    protected override void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        base.OnMoveCanceled(obj);
    }
}
