using UnityEngine;

public class PlayerWeaponWalkState : PlayerWeaponGroundedState
{
    public PlayerWeaponWalkState(PlayerHandStateMashine playerHandStateMashine) : base(playerHandStateMashine)
    {
    }

    public override void Update()
    {
        base.Update();
        
        if (stateMashine.ReusableData.ShouldWalk) return;
        OnMove();
    }
}
