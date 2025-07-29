using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponAirState : PlayerWeaponState
{
    public PlayerWeaponAirState(PlayerHandStateMashine playerHandStateMashine) : base(playerHandStateMashine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        stateMashine.ReusableData.Attack = false;
      

    }

    public override void Update()
    {
        base.Update();
        
        DisFOV();
    }

    
    internal override void Attack()
    {
        
    }

    internal override void Aim()
    {
        var data =  stateMashine.ReusableData.DataWeapon.Data.PositionWeaponData;

        Vector3 currentPosition = data.PositionInHand;
        
        stateMashine.ReusableData.BasePosition = Vector3.Lerp(
            stateMashine.ReusableData.BasePosition, 
            currentPosition, 
            Time.deltaTime * (data.SwaySmoothing * data.AimSmoothingMultiplier)
        );
    }

    internal override void Recoil()
    {
    }


    internal override void FOV()
    {
        
    }

    protected override void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        
    }
}
