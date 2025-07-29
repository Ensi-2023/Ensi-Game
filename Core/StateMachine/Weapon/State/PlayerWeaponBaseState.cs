using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponBaseState : IHandState
{
    protected PlayerHandStateMashine stateMashine { get; }
    protected PlayerCameraData cameraData { get; }

    public PlayerWeaponBaseState(PlayerHandStateMashine playerHandStateMashine)
    {
        stateMashine = playerHandStateMashine;   
        cameraData = stateMashine.Player.Data.CameraData;

    }

    public virtual void EnterState()
    {
        AddInputActionsCallbacks();
    }

    public virtual void ExitState()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void Update()
    {
        GetLook();
        CheckForWeapon();
        
        if(stateMashine.ReusableData.Aiming ) Attack();
        
    }

    public virtual void PhysicsUpdate()
    {
        
    }

    public virtual void LateUpdate()
    {
    
    }

    public virtual void HandleInput()
    {
       
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
      
    }

    public virtual void OnCollisionExit(Collision collision)
    {
        
    }

    public virtual bool IsGroundedCheck()
    {
        return false;
    }

    public virtual void OnTriggerEnter(Collider collider)
    {
        
    }

    public virtual void OnTriggerExit(Collider collider)
    {
        
    }
    
    internal virtual void GetLook()
    {
        if (stateMashine.ReusableData.LookDirection.sqrMagnitude >= 0.01f)
        {
            stateMashine.ReusableData.MouseXY = new Vector2(
                stateMashine.ReusableData.LookDirection.x * Time.fixedDeltaTime * cameraData.SensetivityX,
                stateMashine.ReusableData.LookDirection.y * Time.fixedDeltaTime * cameraData.SensetivityY);
            
            stateMashine.ReusableData.Rotation.y += stateMashine.ReusableData.MouseXY.x;
            stateMashine.ReusableData.Rotation.x -= stateMashine.ReusableData.MouseXY.y;
            stateMashine.ReusableData.Rotation.x =
                Mathf.Clamp(stateMashine.ReusableData.Rotation.x, -89f,
                    70); //Делаем тут ограничеине через инсупектор
            
        }
       
        TransformRotation();
        FOV();
    }

    internal virtual void FOV()
    {
        
        var fov = stateMashine.ReusableData.Aiming?  
            stateMashine.CurrentWeapon != null?stateMashine.ReusableData.DataWeapon.Data.PositionWeaponData.AimFOV:60:60;
        
        stateMashine.Player.MainCamera.fieldOfView = Mathf.Lerp(stateMashine.Player.MainCamera.fieldOfView,
            fov,
            Time.deltaTime * 10f);
    }


    internal virtual void DisFOV()
    {
        stateMashine.Player.MainCamera.fieldOfView = Mathf.Lerp(stateMashine.Player.MainCamera.fieldOfView,
            60,
            Time.deltaTime * 10f);
    }

    

    
    internal virtual void TransformRotation()
    {

        float recoilOffset =  stateMashine.ReusableData.CurrentVerticalRecoilAngle;

        stateMashine.ReusableData.Rotation.x  = Mathf.Clamp(
            stateMashine.ReusableData.Rotation.x + recoilOffset,
            -89f,
            70f);


        float targetRotationY = stateMashine.ReusableData.Rotation.y + stateMashine.ReusableData.CurrentHorizontalRecoilAngle;
        
        // Поворот камеры (локальный поворот)
        stateMashine.Player.MainCameraRoute.transform.localRotation =
            Quaternion.Euler(stateMashine.ReusableData.Rotation.x , 0, 0);

        
        stateMashine.Player.transform.rotation = 
            Quaternion.Euler(0f, targetRotationY, 0f);
        

    }
    
    
    
  
    
    internal virtual void Attack()
    {
        
    }

    internal virtual void Recoil()
    {
        
    }

    internal virtual void Aim()
    {
        
    }
    

    protected virtual void AddInputActionsCallbacks()
    {
        stateMashine.Player.Input.PlayerActions.Drop.performed+= OnDropOnPerformed;
        stateMashine.Player.Input.PlayerActions.Pickup.performed+=OnPickupOnPerformed;
        stateMashine.Player.Input.PlayerActions.Attack.started+=OnAttackOnStarted;
        stateMashine.Player.Input.PlayerActions.Attack.canceled+= OnAttackOnCanceled;
        stateMashine.Player.Input.PlayerActions.Aiming.started+=OnAimingStarted;
        stateMashine.Player.Input.PlayerActions.Aiming.canceled+=OnAimingCanceled;
        stateMashine.Player.Input.PlayerActions.Sprint.started+=OnSprintStarted;
        stateMashine.Player.Input.PlayerActions.Walk.started+=OnWalkStarted;
        stateMashine.Player.Input.PlayerActions.Move.canceled += OnMoveCanceled;
        stateMashine.Player.Input.PlayerActions.SwitchWeaponMode.performed +=OnSwitchWeaponModePerformed;
        
    }

    protected virtual void OnSwitchWeaponModePerformed(InputAction.CallbackContext obj)
    {
        if(stateMashine.CurrentWeapon!=null)
             stateMashine.ReusableData.DataWeapon.CycleFireMode();
    }

    protected virtual void OnMoveCanceled(InputAction.CallbackContext obj)
    {
   
    }

    protected virtual  void OnAttackOnCanceled(InputAction.CallbackContext obj)
    {
      
    }

    protected virtual  void OnAttackOnStarted(InputAction.CallbackContext obj)
    {
       
    }

    private void OnPickupOnPerformed(InputAction.CallbackContext obj)
    {
        if (stateMashine.ReusableData.CurrentItemTarget != null)
        {
            var item = stateMashine.ReusableData.CurrentItemTarget.GetComponent<IItems>();
            if (item != null)
            {
              //  Debug.Log($"Найден предмет: {item.NameItem}");
    
                var weapon = item as IWeapon;
                if (weapon != null)
                {
                  //  Debug.Log($"Это оружие с уроном: {weapon.NameItem}");
                    stateMashine.Equip(stateMashine.ReusableData.CurrentItemTarget);
                }
            }
            else
            {
              //  Debug.LogError("Объект не реализует интерфейс IItem!"); 
            }
        }
    }

    private void OnDropOnPerformed(InputAction.CallbackContext obj)
    {
        if(stateMashine.CurrentWeapon==null) return;
        
        Unequip();
    }

    protected virtual void RemoveInputActionsCallbacks()
    {      
        stateMashine.Player.Input.PlayerActions.Drop.performed-= OnDropOnPerformed;
        stateMashine.Player.Input.PlayerActions.Pickup.performed-=OnPickupOnPerformed;
        stateMashine.Player.Input.PlayerActions.Attack.started-=OnAttackOnStarted;
        stateMashine.Player.Input.PlayerActions.Attack.canceled-= OnAttackOnCanceled;
        stateMashine.Player.Input.PlayerActions.Aiming.started-=OnAimingStarted;
        stateMashine.Player.Input.PlayerActions.Aiming.canceled-=OnAimingCanceled;
        stateMashine.Player.Input.PlayerActions.Sprint.started-=OnSprintStarted;
        stateMashine.Player.Input.PlayerActions.Walk.started-=OnWalkStarted;
        stateMashine.Player.Input.PlayerActions.Move.canceled -= OnMoveCanceled;
        stateMashine.Player.Input.PlayerActions.SwitchWeaponMode.performed -=OnSwitchWeaponModePerformed;
    }

    protected virtual void OnWalkStarted(InputAction.CallbackContext obj)
    {
        
    }

    protected virtual void OnSprintStarted(InputAction.CallbackContext obj)
    {
   
    }

    protected virtual  void OnAimingCanceled(InputAction.CallbackContext obj)
    {
        // stateMashine.ReusableData.Aiming = false;
    }

    protected virtual  void OnAimingStarted(InputAction.CallbackContext obj)
    {
      //  stateMashine.ReusableData.Aiming = true;
        
    }

    private void CheckForWeapon()
    {
        Vector3 viewportPoint = new Vector3(0.5f, 0.5f + cameraData.RayVerticalRayOffset, stateMashine.Player.MainCamera.nearClipPlane);
        Vector3 rayOrigin = stateMashine.Player.MainCamera.ViewportToWorldPoint(viewportPoint);
        Vector3 rayDirection = stateMashine.Player.MainCamera.transform.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, 2f,stateMashine.Player.Data.GroudedData.LayerData.WeaponLayerMask,QueryTriggerInteraction.Ignore); // Увеличил дистанцию для реалистичности

        Color debugColor = Color.red; // Стандартный цвет - нет попадания

        if (hitSomething)
        {
            IItems weapon = hit.collider.GetComponent<IItems>();
            if (weapon != null)
            {
              
                stateMashine.ReusableData.CurrentItemTarget = hit.collider.gameObject;
        
                ShowPickupUI();
                debugColor = Color.green; // Попадание на оружие - ЗЕЛЁНЫЙ
            }
            else
            {
                debugColor = Color.yellow; // Попадание на не-оружие - ЖЁЛТЫЙ
            }
        }

        // Визуализация луча в сцене
        Debug.DrawRay(rayOrigin, rayDirection * 2f, debugColor, 0.1f); // Синхронизировано с дистанцией raycast
                                                                       // // Сброс цели и UI если не попали или попали в не-оружие
        if (!hitSomething || stateMashine.ReusableData.CurrentItemTarget == null)
        {
            stateMashine.ReusableData.CurrentItemTarget = null;
            HidePickupUI();
        }
    }
    
    private void ShowPickupUI()
    {
        if (stateMashine.Player.PickupButtonUI != null)
        {
            stateMashine.Player.PickupButtonUI.SetActive(true); // Активация UI с текстом "F"
        }
    }

    private void HidePickupUI()
    {
        if (stateMashine.Player.PickupButtonUI != null)
        {
            stateMashine.Player.PickupButtonUI.SetActive(false); // Деактивация UI
        }
    }
    
    
    /*Убрать в инвентарь*/
    public virtual void Unequip()
    {
        if (stateMashine.Player.HandPoint == null)
        {
            Debug.LogError("Player has no hand point");
            return;
        }
        
        if(stateMashine.Player.HandPoint.transform.childCount == 0)
            return;

        
        Weapon curWp = stateMashine.CurrentWeapon.GetComponent<Weapon>();
        
        curWp.Shooting-=OnShooting;
        curWp.RecoilClear-=OnClearRecoil;
        curWp.StopAttacking-=OnStopAttacking;
        
        stateMashine.CurrentWeapon.transform.SetParent(null);
        
        stateMashine.CurrentWeapon.GetComponent<WeaponData>().EnableTriggerAllColliders();
        stateMashine.CurrentWeapon.GetComponent<WeaponData>().DisableKinematic();
        stateMashine.CurrentWeapon.GetComponent<WeaponData>().AddForce(stateMashine.Player.transform.forward + Vector3.up * 0.2f);
        
        stateMashine.Clear();
        stateMashine.ReusableData.SetDefaultValues();
        stateMashine.Player.PlayerIK.ClearConstraint();
        
        
        stateMashine.Player.WeaponAnimator.runtimeAnimatorController = null;
    }

    public virtual void OnClearRecoil()
    {
        
    }

    public virtual  void OnShooting()
    {
        
    }

    public virtual void Equip()
    {
        
        if (stateMashine.Player.HandPoint == null)
        {
            Debug.LogError("Player has no hand point");
            return;
        }
        
        if(stateMashine.CurrentWeapon==null)
            return;
        
        if(stateMashine.Player.HandPoint.transform.childCount > 0 )
            return;

        Weapon curWp = stateMashine.CurrentWeapon.GetComponent<Weapon>();


        stateMashine.CurrentWeapon.transform.SetParent(stateMashine.Player.HandPoint);
        stateMashine.CurrentWeapon.transform.localPosition = curWp.Data.PositionWeaponData.PositionInHand;
        stateMashine.CurrentWeapon.transform.localRotation = curWp.Data.PositionWeaponData.RotationInHand;
        
        stateMashine.CurrentWeapon.GetComponent<WeaponData>().DisableTriggerAllColliders();
        stateMashine.CurrentWeapon.GetComponent<WeaponData>().EnableKinematic();
        
        stateMashine.ReusableData.IsWeaponInHand = true;

        stateMashine.Player.PlayerIK.SetConstraint(
            curWp.MainLeftIKTarget,
            curWp.MainLeftIKTargetOffset,
            curWp.MainRightIKTarget,
            curWp.MainRightIKTargetOffset,
            curWp.ShadowLeftIKTarget,
            curWp.ShadowRightIKTarget,
            curWp.ShadowLeftIKTargetOffset,
            curWp.ShadowRightIKTargetOffset,
            true);

        stateMashine.ReusableData.DataWeapon = null;
        stateMashine.ReusableData.DataWeapon = curWp;
        
        curWp.Shooting+=OnShooting;
        curWp.RecoilClear+=OnClearRecoil;
        curWp.StopAttacking+=OnStopAttacking;
        
        stateMashine.Player.WeaponAnimator.runtimeAnimatorController = curWp.WeaponAnimController;
        stateMashine.Player.AnimationHash.Inizialize(stateMashine.Player.WeaponAnimator);
    }

    public virtual void OnStopAttacking()
    {
        stateMashine.ReusableData.Attack = false;
    }

    public virtual void MoveState(WeaponStateEnum.MoveState moveState)
    {
        
    }
    
    
    protected virtual void CameraTween()
    {
        if (stateMashine.CurrentWeapon != null)
        {
            stateMashine.Player.CameraShake.transform.localRotation = Quaternion.Lerp(
                stateMashine.Player.CameraShake.transform.localRotation, Quaternion.Euler(stateMashine.ReusableData.DataWeapon.Data.PositionWeaponData.ShakeDirect),
                Time.deltaTime * stateMashine.ReusableData.DataWeapon.Data.PositionWeaponData.ShakeStr);
        }
    }

    protected void StartAnimation(int animationHash)
    { 
        if( stateMashine.Player.WeaponAnimator.runtimeAnimatorController!=null)
        stateMashine.Player.WeaponAnimator.SetBool(animationHash, true);
    }
    
    protected void StartAnimation(int animationHash,float value)
    { 
        if( stateMashine.Player.WeaponAnimator.runtimeAnimatorController!=null)
        stateMashine.Player.WeaponAnimator.SetFloat(animationHash, value);
    }
    
    protected void StartAnimation(int animationHash,bool value)
    { 
        if( stateMashine.Player.WeaponAnimator.runtimeAnimatorController!=null)
        stateMashine.Player.WeaponAnimator.SetBool(animationHash, value);
    }
    
    protected void StopAnimation(int animationHash)
    {
        if( stateMashine.Player.WeaponAnimator.runtimeAnimatorController!=null)
        stateMashine.Player.WeaponAnimator.SetBool(animationHash, false);
    }
    
}
