using UnityEngine;

public abstract class HandState 
{
    private IHandState _currentState;
    public IHandState CurrentState => _currentState;

    private GameObject _currentWeapon;
    public GameObject CurrentWeapon => _currentWeapon;
    
    
    public void Change(IHandState newState)
    {
        _currentState?.ExitState();
        _currentState = newState;
        _currentState?.EnterState();
    }
    
    public void Update()
    {
        _currentState?.Update();
    }
       
    public void PhysicsUpdate()
    {
        _currentState?.PhysicsUpdate();
    }
   
    public void LateUpdate()
    {
        _currentState?.LateUpdate();
    }
   
    public void HandleInput()
    { 
        _currentState?.HandleInput();    
    }
   
    public void OnCollisionEnter(Collision collision)
    {
        _currentState?.OnCollisionEnter(collision);
    }
   
    public void OnCollisionExit(Collision collision)
    {
        _currentState?.OnCollisionExit(collision);
    }
    
    public void OnTriggerEnter(Collider collider)
    {
        _currentState?.OnTriggerEnter(collider);
    }

    public void OnTriggerExit(Collider collider)
    {
        _currentState?.OnTriggerExit(collider);
    }

    public void Equip(GameObject weapon)
    {
        //_currentState?.Unequip();

        if (_currentWeapon != null)
        {
            return;
        }

        _currentWeapon = weapon;
        _currentState?.Equip();
    }

    public void MoveState(WeaponStateEnum.MoveState moveState)
    {
        _currentState?.MoveState(moveState);
    }

    public void Clear()
    {
        _currentWeapon = null;
    }

}
