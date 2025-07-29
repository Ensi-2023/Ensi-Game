using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponIdleState : PlayerWeaponGroundedState
{
    public PlayerWeaponIdleState(PlayerHandStateMashine playerHandStateMashine) : base(playerHandStateMashine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        
        StartAnimation(stateMashine.Player.AnimationHash.IdleHash);
        
    }

    public override void ExitState()
    {
        base.ExitState();
        StopAnimation(stateMashine.Player.AnimationHash.IdleHash);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    
    
    
    
}
