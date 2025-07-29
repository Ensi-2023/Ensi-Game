using UnityEngine;

public class PlayerWeaponFallingState : PlayerWeaponAirState
{
    private float _fallDuration;
    private PositionWeaponData data;
    public PlayerWeaponFallingState(PlayerHandStateMashine playerHandStateMashine) : base(playerHandStateMashine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        if(stateMashine.ReusableData.DataWeapon ==null) return;
        data = stateMashine.ReusableData.DataWeapon.Data.PositionWeaponData; 
        _fallDuration = 0f;
    }

    public override void Update()
    {
        base.Update();
        /*
        if(stateMashine.ReusableData.DataWeapon ==null) return;
        
        _fallDuration += Time.deltaTime;
        
        float pitchProgress = Mathf.Clamp01(_fallDuration * 0.5f);
        Vector3 targetEuler = data.FallPitchAngle.eulerAngles;
        Vector3 correctedTarget = new Vector3(
            targetEuler.x > 180f ? targetEuler.x - 360f : targetEuler.x,
            targetEuler.y > 180f ? targetEuler.y - 360f : targetEuler.y,
            targetEuler.z > 180f ? targetEuler.z - 360f : targetEuler.z
        );
        // Рассчитываем текущие целевые углы через Lerp
        Vector3 currentTargetAngles = new Vector3(
            Mathf.Lerp(0f, correctedTarget.x, pitchProgress),
            Mathf.Lerp(0f, correctedTarget.y, pitchProgress),
            Mathf.Lerp(0f, correctedTarget.z, pitchProgress)
        );
        
        Quaternion targetPitchQuat = Quaternion.Euler(currentTargetAngles);

// Интерполируем между текущим и целевым кватернионами
        stateMashine.ReusableData.airPitchOffsetQuatr = Quaternion.Lerp(
            stateMashine.ReusableData.airPitchOffsetQuatr,
            targetPitchQuat,
            Time.deltaTime * data.FallingPitchSpeed
        );*/
        
    }
}
