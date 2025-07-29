using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProneState : PlayerMovingState
{
    private PlayreProneData _data; 
    
    public PlayerProneState(PlayerStateMashine statesMashine) : base(statesMashine)
    {
        _data = statesMashine.Player.Data.GroudedData.ProneData;
    }

    public override void EnterState()
    {
        base.EnterState();
        
        PlayerStatesMashine.ReusableData.ShouldProne = true;
        PlayerStatesMashine.ReusableData.currentMoveSpeed = _data.ProneSpeed;
        
        PlayerStatesMashine.Player.HandStateMashine.MoveState(WeaponStateEnum.MoveState.Prone);
        
        StartAnimation(animationData.ProneParamentHash);
    }

    
     protected override void CrouchChecker()
    {
        /*
        // Специальная обработка масштаба и позиции для лежания
        var time = Time.deltaTime / .25f;
        
        // Целевые значения для лежащего состояния
        float targetCrouchSize = movementData.ProneData.ProneSize;
        float targetGroundedOffset = movementData.ProneData.GroundedOffset;
        float targetScaleY = _data.ProneScale;
        float targetLocalPositionY = _data.ProneLocalPosition;

        // Плавная интерполяция параметров
        PlayerStatesMashine.ReusableData.CurrentCrouchSize =
            Mathf.Lerp(PlayerStatesMashine.ReusableData.CurrentCrouchSize, 
                targetCrouchSize,
                time);
                
        PlayerStatesMashine.ReusableData.GroundedOffset = Mathf.Lerp(
            PlayerStatesMashine.ReusableData.GroundedOffset,
            targetGroundedOffset,
            time
        );

        PlayerStatesMashine.Player.CapsuleColliderUtility.CalculateCapsuleColliderDimensions(
            PlayerStatesMashine.ReusableData.CurrentCrouchSize
        );

        // Обработка визуальной модели
        PlayerStatesMashine.ReusableData.CurrentScaleY = Mathf.SmoothDamp(
            PlayerStatesMashine.ReusableData.CurrentScaleY, 
            targetScaleY, 
            ref PlayerStatesMashine.ReusableData.VelocityScale, 
            time
        );
        
        PlayerStatesMashine.ReusableData.CurrentLocalPositionY = Mathf.SmoothDamp(
            PlayerStatesMashine.ReusableData.CurrentLocalPositionY, 
            targetLocalPositionY, 
            ref PlayerStatesMashine.ReusableData.VelocityLocalPosition, 
            time
        );
        
        Vector3 newScale = PlayerStatesMashine.Player.MeshRenderer.localScale;
        newScale.y = PlayerStatesMashine.ReusableData.CurrentScaleY;
        
        Vector3 newPosition = PlayerStatesMashine.Player.MeshRenderer.localPosition;
        newPosition.y = PlayerStatesMashine.ReusableData.CurrentLocalPositionY;
        
        PlayerStatesMashine.Player.MeshRenderer.localPosition = newPosition;
        PlayerStatesMashine.Player.MeshRenderer.localScale = newScale;
        */
    }

    
    protected override void OnProneOnStarted(InputAction.CallbackContext obj)
    {
       base.OnProneOnStarted(obj);
       PlayerStatesMashine.Change(PlayerStatesMashine.CrouchState);
    }
    
    protected override void OnMove()
    {
        // Блокировка автоматического перехода в другие состояния
        if (PlayerStatesMashine.ReusableData.MoveDirection == Vector2.zero) return;
    }
    
    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        
        // Переопределение обработчиков
        PlayerStatesMashine.Player.Input.PlayerActions.Crouch.started += OnCrouchStarted;
        
   
    }

    protected override void JumpOnPerformed(InputAction.CallbackContext obj)
    {
        PlayerStatesMashine.Change(PlayerStatesMashine.CrouchState);
    }

    protected override void OnSprintStarted(InputAction.CallbackContext obj)
    {
        
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        
        PlayerStatesMashine.Player.Input.PlayerActions.Crouch.started -= OnCrouchStarted;
 
    }

    protected override void OnCrouchStarted(InputAction.CallbackContext context)
    {
        // Выход из лежания в приседание
        PlayerStatesMashine.Change(PlayerStatesMashine.CrouchState);
    }
    
    public override void Update()
    {
        base.Update();
        
        //OnMove();
    }

    protected override void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        
    }

    public override void ExitState()
    {
        base.ExitState();
        PlayerStatesMashine.ReusableData.ShouldProne = false;
        StopAnimation(animationData.ProneParamentHash);
    }
    
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    
    
    
    
}
