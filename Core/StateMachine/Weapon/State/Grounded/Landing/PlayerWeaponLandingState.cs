using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerWeaponLandingState : PlayerWeaponState
{
    private float _landingTime;
    private PositionWeaponData data;
    public PlayerWeaponLandingState(PlayerHandStateMashine playerHandStateMashine) : base(playerHandStateMashine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        
        if(stateMashine.ReusableData.DataWeapon ==null) return;
        data = stateMashine.ReusableData.DataWeapon.Data.PositionWeaponData;
   
        _landingTime = 0f;
    }

    public override void Update()
    {
        base.Update();
  
    }

   

}
