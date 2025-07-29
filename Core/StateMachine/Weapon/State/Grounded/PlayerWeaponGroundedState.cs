using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponGroundedState : PlayerWeaponState
{
    public PlayerWeaponGroundedState(PlayerHandStateMashine playerHandStateMashine) : base(playerHandStateMashine)
    {
    }

    internal virtual void OnMove()
    {
       
    }

    protected override void OnSprintStarted(InputAction.CallbackContext obj)
    {
   
        
    }

    protected override void OnWalkStarted(InputAction.CallbackContext obj)
    {
 
    }
    

    protected override void OnMoveCanceled(InputAction.CallbackContext obj)
    {

        
    }
}
