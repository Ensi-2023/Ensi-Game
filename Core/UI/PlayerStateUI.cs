using TMPro;
using UnityEngine;

public class PlayerStateUI : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private TMP_Text _stateText;

    [SerializeField] private float _updateInterval = 0.1f;

    private float _timer;
    private string _lastState = "";

    private float accum = 0;
    private int frames = 0;
    private float timeLeft;
    private float updateInterval = 0.5f;



    private float ViewFps;

    private void Start()
    {
        if (_stateText == null)
            _stateText = GetComponent<TMP_Text>();


        timeLeft = updateInterval;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _updateInterval)
        {
            _timer = 0;
            UpdateStateText();
        }


        FPS();
    }

    private void FPS()
    {
        float frameTime = Time.unscaledDeltaTime;
        accum += 1.0f / frameTime;
        ++frames;

        timeLeft -= Time.unscaledDeltaTime;

        if (timeLeft <= 0.0f)
        {
            float fps = accum / frames;
            ViewFps = Mathf.RoundToInt(fps);

            timeLeft = updateInterval;
            accum = 0;
            frames = 0;
        }
    }


    private string RedOrGreen(string text, bool ChangeColor)
    {
        if (ChangeColor)
        {
            text = $"<color=#FF0000><b>{text}</b></color>";
        }
        else
        {
            text = $"<color=#65CF6C><b>{text}</b></color>";
        }

        return text;
    }

    private void UpdateStateText()
    {
        if (_player == null || _stateText == null) return;
        var speed = Mathf.Round(_player.CharacterController.velocity.y);
        string stateName = _player.GetCurrentStateName();
        string weaponStateName = _player.GetCurrentHandStateName();
        bool isGound = _player.StateMashine.ReusableData.Grounded;

        bool isRun = !_player.StateMashine.ReusableData.ShouldSprint && !_player.StateMashine.ReusableData.ShouldWalk
                                                                     && !_player.StateMashine.ReusableData.ShouldCrouch
                                                                     && !_player.StateMashine.ReusableData.ShouldProne
                                                                     && !_player.StateMashine.ReusableData.ShouldIdle
                                                                     && !_player.StateMashine.ReusableData.Falling;
        // Обновляем только при изменении состояния
        //if (stateName != _lastState) AngleNormal stateMashine.ReusableData.CurrentRecoilAngle  stateMashine.ReusableData.HorizontalRecoilProgress
        {
            _stateText.text =
                $"FPS: {RedOrGreen(ViewFps.ToString(), ViewFps < 60 ? true : false)}\n" +
                $"State: {RedOrGreen(stateName, false)}\n" +
                $"Weapon State: {RedOrGreen(weaponStateName, false)}\n" +
                $"IsGround: {RedOrGreen(isGound.ToString(), !isGound)}\n" +
                $"YSpeed: {RedOrGreen(speed.ToString(), (speed >= -2f && speed <= 0.0) ? false : true)}\n" +
                $"Input.Move: {RedOrGreen(_player.StateMashine.ReusableData.MoveDirection.ToString(), _player.StateMashine.ReusableData.MoveDirection == Vector2.zero ? true : false)}\n" +
                $"Input.ShouldIdle: {RedOrGreen(_player.StateMashine.ReusableData.ShouldIdle.ToString(), !_player.StateMashine.ReusableData.ShouldIdle)}\n" +
                $"Input.ShouldWalk: {RedOrGreen(_player.StateMashine.ReusableData.ShouldWalk.ToString(), !_player.StateMashine.ReusableData.ShouldWalk)}\n" +
                $"Input.ShouldRun: {RedOrGreen(isRun.ToString(), !isRun)}\n" +
                $"Input.ShouldSprint: {RedOrGreen(_player.StateMashine.ReusableData.ShouldSprint.ToString(), !_player.StateMashine.ReusableData.ShouldSprint)}\n" +
                $"Input.ShouldCrouch: {RedOrGreen(_player.StateMashine.ReusableData.ShouldCrouch.ToString(), !_player.StateMashine.ReusableData.ShouldCrouch)}\n" +
                $"Input.ShouldJump: {RedOrGreen(_player.StateMashine.ReusableData.Jumping.ToString(), !_player.StateMashine.ReusableData.Jumping)}\n" +
                $"Input.ShouldProne: {RedOrGreen(_player.StateMashine.ReusableData.ShouldProne.ToString(), !_player.StateMashine.ReusableData.ShouldProne)}\n" +
                $"Input.ShouldFall: {RedOrGreen(_player.StateMashine.ReusableData.Falling.ToString(), !_player.StateMashine.ReusableData.Falling)}\n" +
                $"Input.Aiming: {RedOrGreen(_player.StateMashine.ReusableData.Aiming.ToString(), !_player.StateMashine.ReusableData.Aiming)}\n" +
                $"ShouldFallTime: {RedOrGreen(_player.StateMashine.ReusableData.TimerFalling.ToString("0.00"), _player.StateMashine.ReusableData.TimerFalling == 0 ? false : true)}\n" +
                $"LastFallDistance: {RedOrGreen(_player.StateMashine.ReusableData.LastFallDistance.ToString("0.00"), _player.StateMashine.ReusableData.LastFallDistance == 0 ? false : true)}\n" +
                $"AngleNormal: {RedOrGreen(_player.StateMashine.ReusableData.AngleNormal.ToString("0.00"), _player.StateMashine.ReusableData.AngleNormal == 0 ? false : true)}\n" +
                $"BaseSpeed: {RedOrGreen(_player.StateMashine.ReusableData.currentMoveSpeed.ToString("0.00"), _player.StateMashine.ReusableData.currentMoveSpeed == 0 ? false : true)}\n" +
                $"ModifySpeed: {RedOrGreen(_player.StateMashine.ReusableData.currentModifyMoveSpeed.ToString("0.00"), _player.StateMashine.ReusableData.currentModifyMoveSpeed == 0 ? false : true)}\n" +
                $"CurrentSpeed: {RedOrGreen((_player.StateMashine.ReusableData.currentMoveSpeed * _player.StateMashine.ReusableData.currentModifyMoveSpeed).ToString("0.00"), (_player.StateMashine.ReusableData.currentMoveSpeed * _player.StateMashine.ReusableData.currentModifyMoveSpeed) == 0 ? true : false)}\n" +
                $"CurrentCrouchSize: {RedOrGreen(_player.StateMashine.ReusableData.CurrentCrouchSize.ToString("0.00"), _player.StateMashine.ReusableData.CurrentCrouchSize == 0 ? false : true)}\n" +
                "\n" +
                $"RayFrontSlopeHit: {RedOrGreen(_player.StateMashine.ReusableData.FrontSlopeHit.ToString(), !_player.StateMashine.ReusableData.FrontSlopeHit)}\n" +
                $"RayBackSlopeHit: {RedOrGreen(_player.StateMashine.ReusableData.BackSlopeHit.ToString(), !_player.StateMashine.ReusableData.BackSlopeHit)}\n" +
                $"RayLeftSlopeHit: {RedOrGreen(_player.StateMashine.ReusableData.LeftSlopeHit.ToString(), !_player.StateMashine.ReusableData.LeftSlopeHit)}\n" +
                $"RayRightSlopeHit: {RedOrGreen(_player.StateMashine.ReusableData.RightSlopeHit.ToString(), !_player.StateMashine.ReusableData.RightSlopeHit)}\n" +
                $"WeaponHandMoveState: {_player.HandStateMashine.ReusableData.HandMoveState.ToString()}\n" +
                $"WeaponInHand: {RedOrGreen(_player.HandStateMashine.ReusableData.IsWeaponInHand.ToString(), !_player.HandStateMashine.ReusableData.IsWeaponInHand)}\n" +
                $"Items: {RedOrGreen(_player.HandStateMashine.CurrentWeapon == null ? "" : _player.HandStateMashine.CurrentWeapon.ToString(), _player.HandStateMashine.CurrentWeapon == null ? false : true)}\n" +
                $"Attack: {RedOrGreen(_player.HandStateMashine.ReusableData.Attack.ToString(), !_player.HandStateMashine.ReusableData.Attack)}\n" +
                $"Fire: {RedOrGreen(_player.HandStateMashine.ReusableData.Fire.ToString(), !_player.HandStateMashine.ReusableData.Fire)}\n" +
                $"DataWeapon: {RedOrGreen(_player.HandStateMashine.ReusableData.DataWeapon == null ? "" : _player.HandStateMashine.ReusableData.DataWeapon.ToString(), _player.HandStateMashine.ReusableData.DataWeapon == null ? false : true)}\n" +
                /* $"CurrentVerticalRecoilAngle: {RedOrGreen(_player.HandStateMashine.ReusableData.CurrentVerticalRecoilAngle.ToString("0.00"), _player.HandStateMashine.ReusableData.CurrentVerticalRecoilAngle == 0 ? false : true)}\n" +
                 $"CurrentHorizontalRecoilAngle: {RedOrGreen(_player.HandStateMashine.ReusableData.CurrentHorizontalRecoilAngle.ToString("0.00"), _player.HandStateMashine.ReusableData.CurrentHorizontalRecoilAngle == 0 ? false : true)}\n" +
                 $"HorizontalRecoilProgress: {RedOrGreen(_player.HandStateMashine.ReusableData.HorizontalRecoilProgress.ToString("0.00"), _player.HandStateMashine.ReusableData.HorizontalRecoilProgress == 0 ? false : true)}\n" +
                 $"VerticalRecoilProgress: {RedOrGreen(_player.HandStateMashine.ReusableData.VerticalRecoilProgress.ToString("0.00"), _player.HandStateMashine.ReusableData.VerticalRecoilProgress == 0 ? false : true)}\n" +
                 $"RecoilAngle: {RedOrGreen(_player.HandStateMashine.ReusableData.RecoilAngle.ToString("0.00"), _player.HandStateMashine.ReusableData.RecoilAngle == 0 ? false : true)}\n" +
                 $"HorizontalSpread: {RedOrGreen(_player.HandStateMashine.ReusableData.HorizontalSpread.ToString("0.00"), _player.HandStateMashine.ReusableData.HorizontalSpread == 0 ? false : true)}\n" +
                 $"VerticalSpread: {RedOrGreen(_player.HandStateMashine.ReusableData.VerticalSpread.ToString("0.00"), _player.HandStateMashine.ReusableData.VerticalSpread == 0 ? false : true)}\n" +
                 $"TimeSprayProgress: {RedOrGreen(_player.HandStateMashine.ReusableData.TimeSprayProgress.ToString("0.00"), _player.HandStateMashine.ReusableData.TimeSprayProgress == 0 ? false : true)}\n" +
                 $"RecoilAngleRotation: {RedOrGreen(_player.HandStateMashine.ReusableData.RecoilAngleRotation.ToString("0.00"), _player.HandStateMashine.ReusableData.RecoilAngleRotation == Quaternion.identity ? false : true)}\n" +
                 $"AccumulatedRecoil: {RedOrGreen(_player.HandStateMashine.ReusableData.AccumulatedRecoil.ToString("0.00"), _player.HandStateMashine.ReusableData.AccumulatedRecoil == 0 ? false : true)}\n" +
                 $"VerticalrecoilProg: {RedOrGreen(_player.HandStateMashine.ReusableData.VerticalrecoilProg.ToString("0.00"), _player.HandStateMashine.ReusableData.VerticalrecoilProg == 0 ? false : true)}\n" +
                 $"BreathingTimer: {RedOrGreen(_player.HandStateMashine.ReusableData.BreathingTimer.ToString("0.00"), _player.HandStateMashine.ReusableData.BreathingTimer == 0 ? false : true)}\n" +
                 $"LastShotTime: {RedOrGreen(_player.HandStateMashine.ReusableData.LastShotTime.ToString("0.00"), _player.HandStateMashine.ReusableData.LastShotTime == 0 ? false : true)}\n" +
                 $"SmoothingWobblePositionFactor: {RedOrGreen(_player.HandStateMashine.ReusableData.SmoothingWobblePositionFactor.ToString("0.00"), _player.HandStateMashine.ReusableData.SmoothingWobblePositionFactor == 0 ? false : true)}\n" +
                 $"SmoothingWobbleFactor: {RedOrGreen(_player.HandStateMashine.ReusableData.SmoothingWobbleFactor.ToString("0.00"), _player.HandStateMashine.ReusableData.SmoothingWobbleFactor == 0 ? false : true)}\n" +
                 $"SmoothingWooblePositionSpeed: {RedOrGreen(_player.HandStateMashine.ReusableData.SmoothingWobblePositionSpeed.ToString("0.00"), _player.HandStateMashine.ReusableData.SmoothingWobblePositionSpeed == 0 ? false : true)}\n" +
                 $"IsSprayApplied: {RedOrGreen(_player.StateMashine.ReusableData.IsSprayApplied.ToString(), !_player.StateMashine.ReusableData.IsSprayApplied)}\n" +*/
                $"CURRENT FIRE MODE: {RedOrGreen(_player.HandStateMashine.ReusableData.DataWeapon == null ? "" : _player.StateMashine.ReusableData.DataWeapon.GetCurrentModeName(), true)}\n" +
                $"CURRENT FIRE MODE: {RedOrGreen(_player.HandStateMashine.Player.GroundChecker.GetCurrentSurfaceType() == null ? "" : _player.HandStateMashine.Player.GroundChecker.GetCurrentSurfaceType().ToString(), true)}\n";
            _lastState = stateName;
        }
    }
}  

