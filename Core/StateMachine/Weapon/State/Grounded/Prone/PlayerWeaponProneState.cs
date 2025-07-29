using UnityEngine;

public class PlayerWeaponProneState : PlayerWeaponGroundedState
{
    public PlayerWeaponProneState(PlayerHandStateMashine playerHandStateMashine) : base(playerHandStateMashine)
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
