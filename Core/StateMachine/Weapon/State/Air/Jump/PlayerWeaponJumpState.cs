using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponJumpState : PlayerWeaponAirState
{
    private PositionWeaponData data;
    public PlayerWeaponJumpState(PlayerHandStateMashine playerHandStateMashine) : base(playerHandStateMashine)
    {
        
    }
    
    public override void EnterState()
    {
        base.EnterState();
        if(stateMashine.ReusableData.DataWeapon ==null) return;
        data = stateMashine.ReusableData.DataWeapon.Data.PositionWeaponData;
    }
    

    public override void Update()
    {
        base.Update();
        if(stateMashine.ReusableData.DataWeapon ==null) return;
        

        
        /*
        stateMashine.ReusableData.airPitchOffset = Mathf.Lerp(
            stateMashine.ReusableData.airPitchOffset,
            stateMashine.ReusableData._targetAirPitch,
            Time.deltaTime * data.jumpPitchSpeed
        );
        */
    }

   
}
