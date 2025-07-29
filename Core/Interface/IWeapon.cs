using UnityEngine;

public interface IWeapon : IItems
{
     public GunSettings Data { get; }
     public void Attack(bool fire);

     public void Fire();
     string GetCurrentModeName();
     void CycleFireMode();
}
