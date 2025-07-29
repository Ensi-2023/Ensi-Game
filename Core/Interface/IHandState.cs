using UnityEngine;

public interface IHandState : IState
{

    public void Unequip();
    public void Equip();
    public void MoveState(WeaponStateEnum.MoveState moveState);

}
