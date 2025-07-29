using UnityEngine;

public abstract class PlayerState
{

       internal IState CurrentState => _currentState;
    
       private IState _currentState;
       public void Change(IState newState)
       {
           if(_currentState == newState) return;
           
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
       
}
