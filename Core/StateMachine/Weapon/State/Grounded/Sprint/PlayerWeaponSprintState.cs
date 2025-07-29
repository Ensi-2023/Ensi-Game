using UnityEngine;

public class PlayerWeaponSprintState : PlayerWeaponGroundedState
{
    private float _sprintoffsetTime;
    private PositionWeaponData data;
    public PlayerWeaponSprintState(PlayerHandStateMashine playerHandStateMashine) : base(playerHandStateMashine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        if(stateMashine.ReusableData.DataWeapon ==null) return;
        
        data = stateMashine.ReusableData.DataWeapon.Data.PositionWeaponData;
    
        _sprintoffsetTime = 0f;
        
        StartAnimation(stateMashine.Player.AnimationHash.SprintHash);
    }

    public override void Update()
    {
        base.Update();  
    }
    
    public override void ExitState()
    {
        base.ExitState();
        StopAnimation(stateMashine.Player.AnimationHash.SprintHash);
    }

    private void OnSprinting()
    {
      
    }
}
