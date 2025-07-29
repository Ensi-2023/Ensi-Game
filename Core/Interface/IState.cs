using UnityEngine;

public interface IState 
{
    public void EnterState();
    public void ExitState();
    public void Update();
    public void PhysicsUpdate();
    public void LateUpdate();
    public void HandleInput();
    public void OnCollisionEnter(Collision collision);
    public void OnCollisionExit(Collision collision);
    public bool IsGroundedCheck();

    public void OnTriggerEnter(Collider collider);
    public void OnTriggerExit(Collider collider);
}