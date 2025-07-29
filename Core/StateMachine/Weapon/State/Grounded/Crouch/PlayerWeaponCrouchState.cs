using UnityEngine;

public class PlayerWeaponCrouchState : PlayerWeaponGroundedState
{
    public PlayerWeaponCrouchState(PlayerHandStateMashine playerHandStateMashine) : base(playerHandStateMashine)
    {
    }
    
    public override void EnterState()
    {
        base.EnterState();
        StartAnimation(stateMashine.Player.AnimationHash.WalkingHash);
    }

    public override void ExitState()
    {
        base.ExitState();
        StopAnimation(stateMashine.Player.AnimationHash.WalkingHash);
    }
}
