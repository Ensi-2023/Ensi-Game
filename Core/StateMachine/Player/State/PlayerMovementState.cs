using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovementState : IState
{

    private const float _threshold = 0.01f;
    private bool wasGroundedLastFrame = true;
    private bool wasFalling = true;
    
    
    protected PlayerStateMashine PlayerStatesMashine { get; }
    protected PlayerGroundedData movementData { get; }
    protected PlayerAirData airData { get; }
    protected PlayerCameraData cameraData { get; }

    
    protected PlayerSlopeData slopeData { get; }
    
    protected AnimationData animationData { get; }
    
    
    
    

    public PlayerMovementState(PlayerStateMashine statesMashine)
    {
        PlayerStatesMashine = statesMashine;
        movementData = PlayerStatesMashine.Player.Data.GroudedData;
        airData = PlayerStatesMashine.Player.Data.AirData;
        cameraData = PlayerStatesMashine.Player.Data.CameraData;

        animationData = PlayerStatesMashine.Player.AnimatorData;
        slopeData = movementData.SlopeData;


        DefaultData();

    }


    public virtual Quaternion GetNewWobbleRotationZ(Quaternion newRotationX)
    {
        if (PlayerStatesMashine.ReusableData.Attack)
        {
            PlayerStatesMashine.ReusableData.BreathingTimerX = 0;
            return Quaternion.Lerp(newRotationX,Quaternion.identity,2f* Time.deltaTime);
        }
        
        PlayerStatesMashine.ReusableData.BreathingTimerX += Time.deltaTime;
        newRotationX.z += Mathf.Sin(PlayerStatesMashine.ReusableData.BreathingTimerX * 0.5f) * PlayerStatesMashine.ReusableData.SmoothingWobbleFactor * 0;
        PlayerStatesMashine.ReusableData.ProgressPosition = newRotationX.x;
        return newRotationX;
    }
    
    
    public virtual Vector3 GetNewWobblePositionY(Vector3 newPositonY)
    {
        if (PlayerStatesMashine.ReusableData.Attack)
        {
            PlayerStatesMashine.ReusableData.BreathingTimerY = 0;
            return Vector3.Lerp(newPositonY,Vector3.zero,2f* Time.deltaTime);
        }
        
        PlayerStatesMashine.ReusableData.BreathingTimerY += Time.deltaTime * PlayerStatesMashine.ReusableData.SmoothingWobblePositionSpeed;
        newPositonY.z += Mathf.Sin(PlayerStatesMashine.ReusableData.BreathingTimerY * 2f) * PlayerStatesMashine.ReusableData.SmoothingWobblePositionFactor;
        PlayerStatesMashine.ReusableData.ProgressPosition = newPositonY.x;
        return newPositonY;
    }
    

    public virtual void CameraMoveShake()
    {
        Quaternion newRotationZ = PlayerStatesMashine.Player.OriginalHeadRXCamRotation;
        Vector3 newPositionY = PlayerStatesMashine.Player.OriginalHeadRXPosition;
        var valueRZ = GetNewWobbleRotationZ(newRotationZ);
        
        var valuePZ = GetNewWobblePositionY(newPositionY);
        
        PlayerStatesMashine.Player.HeadRX.localRotation = Quaternion.Lerp(
            valueRZ, PlayerStatesMashine.Player.OriginalHeadRXCamRotation,
            0.05f * Time.deltaTime);
        
        
        PlayerStatesMashine.Player.HeadRZ.localPosition = Vector3.Lerp(
            PlayerStatesMashine.Player.HeadRZ.localPosition,
            new Vector3(0,valuePZ.z,0),
            5f * Time.deltaTime);
        
    }
    
    void DefaultData()
    {
        PlayerStatesMashine.ReusableData.OriginalCenterY = PlayerStatesMashine.Player.CharacterController.center.y;
    }

    /*****************************************************/
    /***************    MAIN STATE FUNC    ***************/
    /*****************************************************/
    public virtual void EnterState()
    {
        AddInputActionsCallbacks();
    }

    public virtual void ExitState()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadInput();
    }

    public virtual void LateUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
       UpdateCameraHeight();
       CameraMoveShake();
       PlayerMove();
       ApplyGravity();
       ApplyMovement();
       FallChecker();
       CrouchChecker();
       
       PlayerStatesMashine.Player.FootstepSound.HandleFootsteps(PlayerStatesMashine.ReusableData.currentMoveSpeed,PlayerStatesMashine.ReusableData.SoundStepVolume, PlayerStatesMashine.ReusableData.Grounded);
       
    }
    

    protected void StartAnimation(int animationHash)
    { 
        PlayerStatesMashine.Player.Animator.SetBool(animationHash, true);
    }
    
    protected void StartAnimation(int animationHash,float value)
    { 
        PlayerStatesMashine.Player.Animator.SetFloat(animationHash, value);
    }
    
    protected void StopAnimation(int animationHash)
    {
        PlayerStatesMashine.Player.Animator.SetBool(animationHash, false);
    }
    
    private Vector3 targetColliderCenter;
    private float targetColliderHeight;
    protected virtual void CrouchChecker()
    {
        if (PlayerStatesMashine.ReusableData.ShouldCrouch)
        {
            StartCroush();
        }
        else
        {
            TryStandUp();
        }
    }


    protected internal float GetCrouchHeight()
    {
        return PlayerStatesMashine.ReusableData.MoveDirection == Vector2.zero
            ? movementData.CrouchData.CrouchCameraPosition
            : movementData.CrouchData.CrouchCameraPositionOnMove;
    }

    protected private void StartCroush()
    {
        PlayerStatesMashine.ReusableData.TargetHeight = movementData.CrouchData.CrouchSize;
        
     
        Vector3 camPos = PlayerStatesMashine.Player.MainCameraRoute.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, GetCrouchHeight(), 0.3f);
        
        
        PlayerStatesMashine.Player.MainCameraRoute.localPosition = camPos;
        
    }

    protected private void TryStandUp()
    {
        if (!CheckHeadroom())
        {
            PlayerStatesMashine.ReusableData.TargetHeight = movementData.CrouchData.DefaultSize;
            
            
            if (movementData.CrouchData.CameraTransitionSpeed > 0)
            {
                Vector3 camPos = PlayerStatesMashine.Player.MainCameraRoute.localPosition;
                camPos.y = Mathf.Lerp(camPos.y, GetCrouchHeight(), 0.3f);
                PlayerStatesMashine.Player.MainCameraRoute.localPosition = camPos;
            }
            
        }
    }
    
    
    private bool CheckHeadroom()
    {
        Vector3 topPoint = PlayerStatesMashine.Player.CharacterController.transform.position + 
                           Vector3.up * (PlayerStatesMashine.Player.CharacterController.height - PlayerStatesMashine.Player.CharacterController.radius);
    
        float spaceNeeded = movementData.CrouchData.DefaultSize - movementData.CrouchData.CrouchSize;
    
        return Physics.SphereCast(
            topPoint, 
            0.3f, 
            Vector3.up, 
            out _, 
            spaceNeeded, 
            movementData.LayerData.GroundLayerMask
        );
    }

    

    internal virtual void UpdateCrouchHeight()
    {
        CharacterController controller = PlayerStatesMashine.Player.CharacterController;
    
        // Плавное изменение высоты
        if (Mathf.Abs(controller.height - PlayerStatesMashine.ReusableData.TargetHeight) > 0.01f)
        {
            float newHeight = Mathf.Lerp(
                controller.height, 
                PlayerStatesMashine.ReusableData.TargetHeight, 
                movementData.CrouchData.CrouchSpeed * Time.deltaTime
            );
        
            // Рассчитываем смещение для сохранения позиции ног
            float heightDifference = newHeight - controller.height;
            Vector3 newCenter = controller.center;
            newCenter.y = movementData.CrouchData.OriginalCentreCollider- (movementData.CrouchData.DefaultSize- newHeight) / 2;
        
            controller.height = newHeight;
            controller.center = newCenter;
        }
        
        
  
    }
    
    internal virtual void UpdateCameraHeight()
    {
        Transform cameraRoot = PlayerStatesMashine.Player.MainCameraRoute;

        // Вычисления положения камеры
            
            float targetHeight = PlayerStatesMashine.ReusableData.ShouldCrouch
                ? GetCrouchHeight()
                : movementData.CrouchData.StandCameraPosition;
             

            cameraRoot.transform.DOLocalMoveY(targetHeight, movementData.CrouchData.CrouchSpeed);
    }


    private void FallChecker()
    {
        
        // PlayerStatesMashine.ReusableData.Grounded = IsGroundedCheck();
      
        if (PlayerStatesMashine.ReusableData.Grounded)
        {
            // Сброс таймера, если на земле
            PlayerStatesMashine.ReusableData.TimerFalling = 0f;
            wasGroundedLastFrame = true;
        }
        else
        {
            if (wasGroundedLastFrame)
            {
                PlayerStatesMashine.ReusableData.TimerFalling = 0f;
                wasGroundedLastFrame = false;
            }
            else
            {
                PlayerStatesMashine.ReusableData.TimerFalling += Time.deltaTime;

                if (PlayerStatesMashine.ReusableData.TimerFalling >=
                    PlayerStatesMashine.Player.Data.AirData.FallData.FallDelay &&
                    !PlayerStatesMashine.ReusableData.Falling)
                {
                    // Прошло 0.25 секунды без опоры — меняем состояние
                    PlayerStatesMashine.Change(PlayerStatesMashine.FallState);
                    PlayerStatesMashine.ReusableData.TimerFalling =
                        0f; // Можно оставить, можно убрать — зависит от логики

                }
            }

        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {

    }

    public virtual void OnCollisionExit(Collision collision)
    {

    }

    
    public virtual bool IsGroundedCheck()
    {
        return PlayerStatesMashine.Player.GroundChecker.IsGroundedCheck(new LayerData()
        {
            GroundedOffset = movementData.LayerData.GroundedOffset,
            GroundedRadius =  movementData.LayerData.GroundedRadius,
            GroundLayerMask = movementData.LayerData.GroundLayerMask,
        });

    }

    public virtual void OnTriggerEnter(Collider collider)
    {
        if (movementData.LayerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnGroundEnter(collider);
        }
    }

    protected virtual void OnGroundEnter(Collider collider)
    {
       
    }

    public virtual void OnTriggerExit(Collider collider)
    {
        if (movementData.LayerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnGroundExit(collider);
        }
    }

    protected virtual void OnGroundExit(Collider collider)
    {
        
    }

    /*****************************************************/


    /*****************************************************/
    /*************    REUSABLE STATE FUNC    *************/
    /*****************************************************/

    internal virtual void Damping()
    {
        return;
        /*
        if (PlayerStatesMashine.ReusableData.Grounded)
        {
            PlayerStatesMashine.Player.Rigidbody.linearDamping = movementData.Drag;
        }
        else
        {
            PlayerStatesMashine.Player.Rigidbody.linearDamping = 0f;
        }*/
    }

    
    internal virtual void GetLook()
    {

        if (PlayerStatesMashine.ReusableData.LookDirection.sqrMagnitude >= 0.01f)
        {
            PlayerStatesMashine.ReusableData.MouseXY = new Vector2(
                PlayerStatesMashine.ReusableData.LookDirection.x * Time.fixedDeltaTime * cameraData.SensetivityX,
                PlayerStatesMashine.ReusableData.LookDirection.y * Time.fixedDeltaTime * cameraData.SensetivityY);
            PlayerStatesMashine.ReusableData.Rotation.y += PlayerStatesMashine.ReusableData.MouseXY.x;
            PlayerStatesMashine.ReusableData.Rotation.x -= PlayerStatesMashine.ReusableData.MouseXY.y;
            PlayerStatesMashine.ReusableData.Rotation.x =
                Mathf.Clamp(PlayerStatesMashine.ReusableData.Rotation.x, -65f,
                    80); //Делаем тут ограничеине через инсупектор
            TransformRotation();
        }
    }

    internal virtual void TransformRotation()
    {
        // Поворот камеры (локальный поворот)
        PlayerStatesMashine.Player.MainCameraRoute.transform.localRotation =
            Quaternion.Euler(PlayerStatesMashine.ReusableData.Rotation.x, 0, 0);


        // Поворот персонажа (через Rigidbody)
        Quaternion targetRotation = Quaternion.Euler(0f, PlayerStatesMashine.ReusableData.Rotation.y, 0f);
        //PlayerStatesMashine.Player.Rigidbody.MoveRotation(targetRotation);
    }
    
    internal virtual void PlayerMove()
    {
        PlayerStatesMashine.ReusableData.Velocity = PlayerStatesMashine.Player.Orientation.forward * PlayerStatesMashine.ReusableData.MoveDirection.y +
                                                    PlayerStatesMashine.Player.Orientation.right * PlayerStatesMashine.ReusableData.MoveDirection.x;

        PlayerForce();
    }
    
    internal virtual void PlayerForce()
    {

        PlayerStatesMashine.ReusableData.currentModifyMoveSpeed = CalculateSlopeSpeedModifier();
        
        float finalSpeed = PlayerStatesMashine.ReusableData.currentMoveSpeed * PlayerStatesMashine.ReusableData.currentModifyMoveSpeed;
        
        
        PlayerStatesMashine.ReusableData.Velocity = PlayerStatesMashine.ReusableData.Velocity.normalized * finalSpeed;

        PlayerStatesMashine.ReusableData.HorizontalVelocity = new Vector3(PlayerStatesMashine.ReusableData.Velocity.x,
            0, PlayerStatesMashine.ReusableData.Velocity.z);
    
    }


    internal virtual void ApplyMovement()
    {
        UpdateCrouchHeight();
        
        Vector3 motion = PlayerStatesMashine.ReusableData.HorizontalVelocity * Time.deltaTime;
        motion.y = PlayerStatesMashine.ReusableData.VerticalVelocity * Time.deltaTime;
        
        PlayerStatesMashine.Player.CharacterController.Move(motion);
        
    }


    internal virtual void ApplyPintoGround()
    {
        PlayerStatesMashine.ReusableData.VerticalVelocity = -2f;
    }

    internal virtual void ApplyGravity()
    {
        PlayerStatesMashine.ReusableData.Grounded = IsGroundedCheck();
        if (PlayerStatesMashine.ReusableData.Grounded)
        {
            ApplyPintoGround();
        }
        else
        {
            PlayerStatesMashine.ReusableData.VerticalVelocity += movementData.Gravity * Time.deltaTime;
        }
    }

    
    
    // Проверка крутизны склона
    
    
    
    
    private float CalculateSlopeSpeedModifier()
    {
        Vector3 moveDirection = PlayerStatesMashine.ReusableData.MoveDirection.normalized;
        
        if (moveDirection == Vector3.zero) return 1f;

        if (moveDirection.y > 0.7 && PlayerStatesMashine.ReusableData.FrontSlopeHit)
        {
            return movementData.SlopeSpeedAngle.Evaluate(PlayerStatesMashine.ReusableData.AngleNormal);
        }

        if (moveDirection.y < -0.7 && PlayerStatesMashine.ReusableData.FrontSlopeHit)
        {
            return movementData.SlopeSpeedAngle.Evaluate(PlayerStatesMashine.ReusableData.AngleNormal);
        }
        
        if (moveDirection.x > 0.7 && PlayerStatesMashine.ReusableData.RightSlopeHit)
        {
            return movementData.SlopeSpeedAngle.Evaluate(PlayerStatesMashine.ReusableData.AngleNormal);
        }

        if (moveDirection.x < -0.7 && PlayerStatesMashine.ReusableData.RightSlopeHit)
        {
            return 1f;
        }
        
        if (moveDirection.x < -0.7 && PlayerStatesMashine.ReusableData.LeftSlopeHit)
        {
            return movementData.SlopeSpeedAngle.Evaluate(PlayerStatesMashine.ReusableData.AngleNormal);
        }
        
        if (moveDirection.x > 0.7 && PlayerStatesMashine.ReusableData.LeftSlopeHit)
        {
            return 1f;
        }
        
        return 1f;
    }
    
    
    internal virtual void Float()
    {
        
        Vector3 capsuleCenter = PlayerStatesMashine.Player.CharacterController.bounds.center;

      // Луч вниз
        Ray ray = new Ray(capsuleCenter, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 
                slopeData.RaycastDistance, 
                movementData.LayerData.GroundLayerMask,
                QueryTriggerInteraction.Ignore))
        {
            
            Debug.DrawRay(capsuleCenter, Vector3.down * hit.distance, Color.yellow);
            
            PlayerStatesMashine.ReusableData.GroundNormal = hit.normal;
            float groundAngle = Vector3.Angle(hit.normal,-ray.direction);
            PlayerStatesMashine.ReusableData.AngleNormal = groundAngle;
            
            PlayerStatesMashine.Player.SlopeDetector.DetectSlope();
            
        }
        
    }
    
  

    
    internal virtual void SpeedControl()
    {
       /*
        Vector3 flatVel = new Vector3(PlayerStatesMashine.Player.Rigidbody.linearVelocity.x,0f, PlayerStatesMashine.Player.Rigidbody.linearVelocity.z);

        if (flatVel.magnitude > PlayerStatesMashine.ReusableData.currentMoveSpeed)
        {
            Vector3 limited = flatVel.normalized * PlayerStatesMashine.ReusableData.currentMoveSpeed;
            PlayerStatesMashine.Player.Rigidbody.linearVelocity = new Vector3(limited.x, PlayerStatesMashine.Player.Rigidbody.linearVelocity.y, limited.z);
        }
       */
    }
    
    protected virtual void AddInputActionsCallbacks()
    {
        PlayerStatesMashine.Player.Input.PlayerActions.Walk.started += OnWalkStarted;
        PlayerStatesMashine.Player.Input.PlayerActions.Sprint.started += OnSprintStarted;
        PlayerStatesMashine.Player.Input.PlayerActions.Sprint.canceled += OnSprintCanceled;
        PlayerStatesMashine.Player.Input.PlayerActions.Move.canceled += OnMoveCanceled;
        PlayerStatesMashine.Player.Input.PlayerActions.Attack.started += OnAttackStarted;
        PlayerStatesMashine.Player.Input.PlayerActions.Attack.canceled += OnAttackCanceled;
        PlayerStatesMashine.Player.Input.PlayerActions.Prone.performed+= OnProneOnStarted;
        
        PlayerStatesMashine.Player.Input.PlayerActions.Aiming.started+=OnAimingStarted;
        PlayerStatesMashine.Player.Input.PlayerActions.Aiming.canceled+=OnAimingCanceled;

        
    }

    protected virtual void OnProneOnStarted(InputAction.CallbackContext obj)
    {
        PlayerStatesMashine.ReusableData.ShouldProne = !PlayerStatesMashine.ReusableData.ShouldProne;
    }


    protected virtual void OnWalkStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        PlayerStatesMashine.ReusableData.ShouldWalk = !PlayerStatesMashine.ReusableData.ShouldWalk;
    }
    
    protected virtual void OnSprintStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        PlayerStatesMashine.ReusableData.ShouldSprint = true;
    }
    
    protected virtual void OnSprintCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        PlayerStatesMashine.ReusableData.ShouldSprint = false;
    }
    
    protected virtual void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
     
    }
    
    protected virtual void OnAimingCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
       // PlayerStatesMashine.ReusableData.Aiming = false;
    }

    protected virtual void OnAimingStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        PlayerStatesMashine.ReusableData.Aiming = !PlayerStatesMashine.ReusableData.Aiming;  
    } 
    
    protected virtual void OnAttackCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        PlayerStatesMashine.ReusableData.Attack = false;
    }

    protected virtual void OnAttackStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        PlayerStatesMashine.ReusableData.Attack = true;
    }
    
    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerStatesMashine.Player.Input.PlayerActions.Walk.started -= OnWalkStarted;
        PlayerStatesMashine.Player.Input.PlayerActions.Sprint.started -= OnSprintStarted;
        PlayerStatesMashine.Player.Input.PlayerActions.Sprint.canceled -= OnSprintCanceled;
        PlayerStatesMashine.Player.Input.PlayerActions.Move.canceled -= OnMoveCanceled;
        PlayerStatesMashine.Player.Input.PlayerActions.Attack.started -= OnAttackStarted;
        PlayerStatesMashine.Player.Input.PlayerActions.Attack.canceled -= OnAttackCanceled;
        PlayerStatesMashine.Player.Input.PlayerActions.Prone.performed -= OnProneOnStarted;
        PlayerStatesMashine.Player.Input.PlayerActions.Aiming.started-=OnAimingStarted;
        PlayerStatesMashine.Player.Input.PlayerActions.Aiming.canceled-=OnAimingCanceled;

    }
    
    /*****************************************************/    
    
    private void ReadInput()
    {
        PlayerStatesMashine.ReusableData.MoveDirection = PlayerStatesMashine.Player.Input.PlayerActions.Move.ReadValue<Vector2>();
        PlayerStatesMashine.ReusableData.LookDirection = PlayerStatesMashine.Player.Input.PlayerActions.Look.ReadValue<Vector2>(); 
    }

 
    

    
}
