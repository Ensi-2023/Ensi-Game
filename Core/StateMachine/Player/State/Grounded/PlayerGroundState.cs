using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerMovementState
{
    
    //private SlopeData _slopeData;
    public PlayerGroundState(PlayerStateMashine statesMashine) : base(statesMashine)
    {
       // _slopeData = statesMashine.Player.CapsuleColliderUtility.SlopeData;
    }


    public override void EnterState()
    {
        base.EnterState();

    }

    public override void Update()
    {
        base.Update();

        
        AnimatorSmoothing();
       
    }

    [SerializeField] private float smoothTime = 0.1f; // Время сглаживания
    private float currentXVelocity, currentYVelocity;
    
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
      
        Float();
    }


    private float _smoothedMoveX;
    private float _smoothedMoveY;
    private float _velocityX;
    private float _velocityY;
    [SerializeField] private float _smoothTime = 0.1f; 
    
    public virtual void AnimatorSmoothing()
    {

        if (PlayerStatesMashine.ReusableData.MoveDirection.magnitude == 0f)
        {
            StartAnimation(animationData.MovingXParamentHash, 0);
            StartAnimation(animationData.MovingYParamentHash, 0);
            return;
        }


        // Получаем целевые значения из входных данных
        float targetX = PlayerStatesMashine.ReusableData.MoveDirection.x;
        float targetY = PlayerStatesMashine.ReusableData.MoveDirection.y;

        // Применяем сглаживание
        _smoothedMoveX = Mathf.SmoothDamp(_smoothedMoveX, targetX, ref _velocityX, _smoothTime);
        _smoothedMoveY = Mathf.SmoothDamp(_smoothedMoveY, targetY, ref _velocityY, _smoothTime);

        // Передаем сглаженные значения в аниматор
        
        StartAnimation(animationData.MovingXParamentHash, _smoothedMoveX);
        StartAnimation(animationData.MovingYParamentHash, _smoothedMoveY);
        
    }

    protected override void OnGroundExit(Collider collider)
    {
        base.OnGroundExit(collider);

        /*
        if (IsThereGroundUnderneatch())
        {
            PlayerStatesMashine.ReusableData.Grounded = true;
            return;
        }

        Vector3 capsuleColliderCenterInWorldSpace = PlayerStatesMashine.Player.CapsuleColliderUtility.CapsuleColliderData.Collider.bounds.center;
        Ray dawnRay = new Ray(capsuleColliderCenterInWorldSpace - PlayerStatesMashine.Player.CapsuleColliderUtility.CapsuleColliderData.ColliderVerticalExtents, 
            Vector3.down);
        if (!Physics.Raycast(dawnRay, out _,movementData.GroundToFallDistance,movementData.LayerData.GroundLayerMask,QueryTriggerInteraction.Ignore))
        {
            OnFall();
        }
*/
    }

    private bool IsThereGroundUnderneatch()
    {
        /*
        BoxCollider groundCheckCollider =
            PlayerStatesMashine.Player.CapsuleColliderUtility.TriggerColliderData.GroundCheckCollider;
        
        Vector3 groundColliderCenterInWorldSpace =
            groundCheckCollider.bounds.center;

        Collider[] overlaGroundCollider = Physics.OverlapBox(groundColliderCenterInWorldSpace,
            groundCheckCollider.bounds.extents,groundCheckCollider.transform.rotation,movementData.LayerData.GroundLayerMask,QueryTriggerInteraction.Ignore);

        return overlaGroundCollider.Length > 0;
  
*/
        return false;
    }

    protected virtual void OnFall()
    {
        PlayerStatesMashine.ReusableData.Grounded = false;
    }
    
    
    protected virtual void OnMove()
    {
        if (PlayerStatesMashine.ReusableData.ShouldCrouch && PlayerStatesMashine.ReusableData.MoveDirection != Vector2.zero) 
        {
            
            Debug.Log("OLALA");
            PlayerStatesMashine.Change(PlayerStatesMashine.CrouchMove);
            return;
        }
        
        if (PlayerStatesMashine.ReusableData.ShouldCrouch)
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.CrouchState);
            return;
        }
        
        if (PlayerStatesMashine.ReusableData.ShouldSprint &&  (!PlayerStatesMashine.ReusableData.Aiming || !PlayerStatesMashine.ReusableData.Attack))
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.SprintState); // Sprint
            return;
        }

        if (PlayerStatesMashine.ReusableData.ShouldWalk)
        {
            PlayerStatesMashine.Change(PlayerStatesMashine.WalkState); // Walk
            return;
        }
        
        PlayerStatesMashine.Change(PlayerStatesMashine.RunState); // Run
        
    }

    protected override void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        base.OnMoveCanceled(obj);
        PlayerStatesMashine.Change(PlayerStatesMashine.IdleState);
    }

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        PlayerStatesMashine.Player.Input.PlayerActions.Jump.performed+=JumpOnPerformed;
        PlayerStatesMashine.Player.Input.PlayerActions.Crouch.canceled += OnCrouchCanceled;
        PlayerStatesMashine.Player.Input.PlayerActions.Crouch.started += OnCrouchStarted; 
    }

    protected override void OnProneOnStarted(InputAction.CallbackContext obj)
    {
        base.OnProneOnStarted(obj);
        PlayerStatesMashine.Change(PlayerStatesMashine.ProneState);
    }

    protected virtual void OnCrouchCanceled(InputAction.CallbackContext obj)
    {
       // PlayerStatesMashine.ReusableData.ShouldCrouch = false;
    }
    
    protected virtual void OnCrouchStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
      //  if(PlayerStatesMashine.ReusableData.ShouldCrouch) return;
        PlayerStatesMashine.ReusableData.ShouldCrouch = !PlayerStatesMashine.ReusableData.ShouldCrouch ;
        PlayerStatesMashine.Change(PlayerStatesMashine.CrouchState);
        
    }
    
    protected virtual  void JumpOnPerformed(InputAction.CallbackContext obj)
    {
        PlayerStatesMashine.Change(PlayerStatesMashine.JumpState); // Jump
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        PlayerStatesMashine.Player.Input.PlayerActions.Jump.performed-=JumpOnPerformed;
        PlayerStatesMashine.Player.Input.PlayerActions.Crouch.canceled -= OnCrouchCanceled;
        PlayerStatesMashine.Player.Input.PlayerActions.Crouch.started -= OnCrouchStarted; 
    }
}
