using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerWeaponState : PlayerWeaponBaseState
{

    public PlayerWeaponState(PlayerHandStateMashine playerHandStateMashine) : base(playerHandStateMashine)
    {

    }

    public override void MoveState(WeaponStateEnum.MoveState moveState)
    {
        base.MoveState(moveState);

        switch (moveState)
        {
            case WeaponStateEnum.MoveState.Jump:
                stateMashine.Change(stateMashine.JumpState);
                stateMashine.ReusableData.HandMoveState = moveState;
                break;

            case WeaponStateEnum.MoveState.Falling:
                stateMashine.Change(stateMashine.FallingState);
                stateMashine.ReusableData.HandMoveState = moveState;
                break;


            case WeaponStateEnum.MoveState.Landing:
                stateMashine.Change(stateMashine.LandingState);
                stateMashine.ReusableData.HandMoveState = moveState;
                break;



            case WeaponStateEnum.MoveState.Idle:
                stateMashine.Change(stateMashine.IdleState);
                stateMashine.ReusableData.HandMoveState = moveState;
                break;


            case WeaponStateEnum.MoveState.Walk:
                stateMashine.Change(stateMashine.WalkState);
                stateMashine.ReusableData.HandMoveState = moveState;
                break;


            case WeaponStateEnum.MoveState.Sprint:
                stateMashine.Change(stateMashine.SprintState);
                stateMashine.ReusableData.HandMoveState = moveState;
                break;



            case WeaponStateEnum.MoveState.Run:
                stateMashine.Change(stateMashine.RunState);
                stateMashine.ReusableData.HandMoveState = moveState;
                break;
        }
    }

    internal override void Aim()
    {
        base.Aim();

        
        StartAnimation(stateMashine.Player.AnimationHash.AimingHash,stateMashine.ReusableData.Aiming);
        
        var data = stateMashine.ReusableData.DataWeapon.Data.PositionWeaponData;

        Vector3 currentPosition = stateMashine.ReusableData.Aiming
            ? data.PositionInHand + data.AimPositionOffset
            : data.PositionInHand;

        stateMashine.ReusableData.BasePosition = Vector3.Lerp(
            stateMashine.ReusableData.BasePosition,
            currentPosition,
            Time.deltaTime * (data.SwaySmoothing * data.AimSmoothingMultiplier)
        );

        Vector3 currentCamPosition = stateMashine.ReusableData.Aiming
            ? data.AimCameraPosition
            : data.StartCameraPosition;

        stateMashine.ReusableData.CameraBasePosition = Vector3.Lerp(
            stateMashine.ReusableData.CameraBasePosition,
            currentCamPosition,
            Time.deltaTime * (data.SwaySmoothing * data.AimSmoothingMultiplier)
        );

    }

    protected virtual void CalculateSway()
    {
        var data = stateMashine.ReusableData.DataWeapon.Data.PositionWeaponData;

        Vector2 lookdelta = stateMashine.ReusableData.LookDirection;
        bool isAiming = stateMashine.ReusableData.Aiming;

        // Используем параметры из WeaponData
        float verticalMultiplier = isAiming ? data.VerticalAimMultiplier : 1f;
        float smoothingMultiplier = isAiming ? data.AimSmoothingMultiplier : 1f;

        // Рассчет позиции
        stateMashine.ReusableData.targetSwayPosition = new Vector3(
            Mathf.Clamp(-lookdelta.x * data.SwayAmount, -data.MaxSwayAmount, data.MaxSwayAmount),
            Mathf.Clamp(-lookdelta.y * data.SwayAmount * verticalMultiplier, -data.MaxSwayAmount, data.MaxSwayAmount),
            0
        );

        // Рассчет вращения
        stateMashine.ReusableData.targetSwayRotation = new Vector3(
            lookdelta.y * data.SwayAmount * 2f * verticalMultiplier,
            -lookdelta.x * data.SwayAmount * 2f,
            -lookdelta.x * data.SwayAmount * 1.5f
        );

        // Сглаживание с множителем
        float positionSmoothing = data.SwaySmoothing * smoothingMultiplier;

        stateMashine.ReusableData.swayPositionOffset = Vector3.Lerp(
            stateMashine.ReusableData.swayPositionOffset,
            stateMashine.ReusableData.targetSwayPosition,
            Time.deltaTime * positionSmoothing
        );

        stateMashine.ReusableData.swayRotationOffset = Vector3.Lerp(
            stateMashine.ReusableData.swayRotationOffset,
            stateMashine.ReusableData.targetSwayRotation,
            Time.deltaTime * positionSmoothing
        );


        ApplySway();
    }

    protected virtual void ApplySway()
    {
        if (stateMashine.CurrentWeapon == null) return;

        var data = stateMashine.ReusableData.DataWeapon.Data.PositionWeaponData;


        Quaternion baseRotation = stateMashine.ReusableData.Aiming
            ? Quaternion.Euler(data.AimRotationOffset)
            : data.RotationInHand;

        // Комбинируем все вращения:
        Quaternion totalRotation = baseRotation *
                                   Quaternion.Euler(stateMashine.ReusableData.swayRotationOffset)
            ;

        stateMashine.CurrentWeapon.transform.localPosition =
            stateMashine.ReusableData.BasePosition + stateMashine.ReusableData.swayPositionOffset;
        stateMashine.CurrentWeapon.transform.localRotation = totalRotation;
        stateMashine.Player.MainCamera.transform.localPosition = stateMashine.ReusableData.CameraBasePosition;
    }
    

    internal override void Attack()
    {
        base.Attack();

        if (stateMashine.ReusableData.IsWeaponInHand == false) return;
        stateMashine.ReusableData.DataWeapon.Attack(stateMashine.ReusableData.Attack);
        
    }

    protected virtual void UnAttack()
    {
        if (stateMashine.ReusableData.Attack == false)
        {
            var data = stateMashine.ReusableData.DataWeapon.Data.RecoilWeaponData;
            
            stateMashine.ReusableData.IsSprayApplied = false;
            
            stateMashine.ReusableData.VerticalrecoilProg = Mathf.Lerp(stateMashine.ReusableData.VerticalrecoilProg, 0,
                data.positionSmoothTime * Time.deltaTime);
            
            stateMashine.ReusableData.VerticalSpread = Mathf.Lerp(stateMashine.ReusableData.VerticalSpread, 0,
                data.positionSmoothTime * Time.deltaTime);
            

            stateMashine.ReusableData.SprayRotationOffset = Quaternion.Lerp(stateMashine.ReusableData.SprayRotationOffset, Quaternion.identity,
                data.positionSmoothTime * Time.deltaTime);
            
            stateMashine.ReusableData.HorizontalSpread = Mathf.Lerp(stateMashine.ReusableData.HorizontalSpread, 0,
                data.positionSmoothTime * Time.deltaTime);
            

            if (stateMashine.ReusableData.VerticalrecoilProg >data.MaxVerticalProcessToReturn);
            {
                stateMashine.ReusableData.ShouldReturn = true;
            }
        }
    }

    private void UpdateRotateWeaponContainer()
{
    var data = stateMashine.ReusableData.DataWeapon.Data.RecoilWeaponData;
    float time = Time.deltaTime * data.positionSmoothTime;

    if (stateMashine.ReusableData.RecoilAngle == 0)
    {
        stateMashine.ReusableData.ShouldReturn = false;
    }

    // Плавное уменьшение угла отдачи только если прошло время задержки
    if (stateMashine.ReusableData.ShouldReturn)
    {
        stateMashine.ReusableData.RecoilAngle = Mathf.MoveTowards(
            stateMashine.ReusableData.RecoilAngle,
            0f,
            data.returnSmoothSpeed * Time.deltaTime
        );
    }

    Quaternion targetRotation = Quaternion.Euler(stateMashine.ReusableData.RecoilAngle, 0, 0);
    Quaternion newRotation = Quaternion.Lerp(
        stateMashine.ReusableData.RecoilAngleRotation,
        targetRotation,
        time
    );

    // Вычисляем текущий угол
    float currentAngle = stateMashine.ReusableData.VerticalrecoilProg>data.MaxVerticalProcessToReturn? Mathf.Round(stateMashine.ReusableData.RecoilAngleRotation.x * 100f) / 100f:0f;
    
    // Выбираем кривую на основе состояния
    AnimationCurve recoilCurve;
    
    if (stateMashine.ReusableData.Attack)
    {
        // Во время стрельбы - кривая подъема
        recoilCurve = data.CameraRecoilCurveUp;
    }
    else
    {
        // После задержки - кривая возврата
        recoilCurve = data.CameraRecoilCurveDown;
    }
    
    float verticalRecoil = recoilCurve.Evaluate(-currentAngle);

    stateMashine.ReusableData.CurrentVerticalRecoilAngle = Mathf.Lerp(
        stateMashine.ReusableData.CurrentVerticalRecoilAngle,
        -verticalRecoil,
        time
    );
    
    stateMashine.ReusableData.RecoilAngleRotation = newRotation;
    stateMashine.Player.WeaponContainer.transform.localRotation = 
        stateMashine.ReusableData.Aiming ? Quaternion.identity : newRotation;
}
    
    public virtual void UpdateSpray()
    {
        /*
        stateMashine.Player.RecoilRoute.transform.localRotation = Quaternion.Lerp(
            stateMashine.Player.RecoilRoute.transform.localRotation,
            stateMashine.ReusableData.IsSprayApplied
                ? stateMashine.ReusableData.SprayRotationOffset
                : Quaternion
                    .identity,
            Time.deltaTime * stateMashine.ReusableData.DataWeapon.Data.RecoilWeaponData.positionRecoilSmoothTime);
            */
        
        
       
        stateMashine.Player.RecoilRoute.transform.localRotation = Quaternion.Lerp(
            stateMashine.Player.RecoilRoute.transform.localRotation,
            stateMashine.ReusableData.SprayRotationOffset,
            Time.deltaTime * stateMashine.ReusableData.DataWeapon.Data.RecoilWeaponData.positionRecoilSmoothTime);
    }

    public virtual void UpdateWeaponKick()
    {
        var recoilData = stateMashine.ReusableData.DataWeapon.Data.RecoilWeaponData;


        if (stateMashine.ReusableData.IsKickApplied)
        {
            // Плавное движение к целевому смещению
            stateMashine.ReusableData.CurrentKickOffset = Vector3.Lerp(
                stateMashine.ReusableData.CurrentKickOffset,
                stateMashine.ReusableData.TargetKickOffset,
                Time.deltaTime * recoilData.KickSpeed
            );


            stateMashine.ReusableData.CurrentKickRotateOffset = Quaternion.Lerp( 
                stateMashine.ReusableData.CurrentKickRotateOffset,
                stateMashine.ReusableData.TargetKickRotation,    Time.deltaTime * recoilData.KickSpeed);

        }
        else
        {
            // Плавное возвращение в исходное положение
            stateMashine.ReusableData.CurrentKickOffset = Vector3.Lerp(
                stateMashine.ReusableData.CurrentKickOffset,
                Vector3.zero,
                Time.deltaTime * recoilData.ReturnKickSpeed
            );
            
            stateMashine.ReusableData.CurrentKickRotateOffset  = Quaternion.Lerp(   stateMashine.ReusableData.CurrentKickRotateOffset,Quaternion.identity,Time.deltaTime * recoilData.ReturnKickSpeed);
        }


        stateMashine.Player.RecoilRoute.transform.localPosition = Vector3.Lerp(
            stateMashine.Player.RecoilRoute.transform.localPosition, stateMashine.ReusableData.CurrentKickOffset,
            Time.deltaTime * recoilData.ReturnKickSpeed);

        
        stateMashine.Player.RecoilRoute.transform.localRotation = Quaternion.Lerp(stateMashine.Player.RecoilRoute.transform.localRotation,
            stateMashine.ReusableData.CurrentKickRotateOffset ,Time.deltaTime * recoilData.ReturnKickSpeed );
        
        if (Vector3.Distance(stateMashine.ReusableData.CurrentKickOffset,
                stateMashine.ReusableData.TargetKickOffset) < 0.01f)
        {
            stateMashine.ReusableData.IsKickApplied = false;
        }
    }

    public virtual void Spray()
    {
        stateMashine.ReusableData.IsSprayApplied = true;

        var weaponData = stateMashine.ReusableData.DataWeapon.Data.RecoilWeaponData;
        var time = Time.deltaTime * weaponData.positionSmoothTime;

        // Вычисляем случайное отклонение (вертикаль и горизонталь)
        stateMashine.ReusableData.VerticalSpread = Random.Range(-weaponData.SprayAngleY, weaponData.SprayAngleY);
        stateMashine.ReusableData.HorizontalSpread = Random.Range(-weaponData.SprayAngleX, weaponData.SprayAngleX);
        // Создаем кватернион отклонения

        stateMashine.ReusableData.SprayRotationOffset = Quaternion.Euler(stateMashine.ReusableData.VerticalSpread,
            stateMashine.ReusableData.HorizontalSpread, 0f);
    }

    public override void OnShooting()
    {
        base.OnShooting();

        var date = stateMashine.ReusableData.DataWeapon.Data.RecoilWeaponData;
        
        stateMashine.ReusableData.RecoilAngle =
            date.RecoilAngle;
        
        
        stateMashine.ReusableData.DataWeapon.Fire();
        ApplyWeaponKick();
        Spray();
        CameraTween();
        
        
        if (stateMashine.ReusableData.VerticalrecoilProg >= date.MaxVerticalProcess)
        {
            stateMashine.ReusableData.VerticalrecoilProg =date.MaxVerticalProcess;
            return;
        }
        stateMashine.ReusableData.VerticalrecoilProg +=date.VerticalProcessSum;

    }

    private void ApplyWeaponKick()
    {
        var kickData = stateMashine.ReusableData.DataWeapon.Data.RecoilWeaponData;

        // Устанавливаем целевое смещение
        stateMashine.ReusableData.TargetKickOffset = new Vector3(
            Random.Range(kickData.KickHorizontalDirection.x, kickData.KickHorizontalDirection.y),
            Random.Range(kickData.KickVerticalDirection.x, kickData.KickVerticalDirection.y),
            -kickData.KickAmount
        );

        stateMashine.ReusableData.TargetKickRotation = Quaternion.Euler(kickData.KickRotationDirection.x,kickData.KickRotationDirection.y,kickData.KickRotationDirection.z);
        stateMashine.ReusableData.IsKickApplied = true;
    }
    

    public override void Update()
    {
        base.Update();
        
        if(stateMashine.ReusableData.IsWeaponInHand==false) return;
        Attack();
        UnAttack();
        UpdateWeaponKick();
        UpdateSpray();
        Aim();
        UpdateRotateWeaponContainer();
        CalculateSway();

    

    }
    
    

  
}
