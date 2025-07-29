
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

using static WeaponStateEnum;

public class Weapon : MonoBehaviour,IWeapon
{
    [SerializeField] private RuntimeAnimatorController  _weaponAnimController;
    [field: SerializeField] public GunSettings Data { get; private set; }
 
    public int Index => indexgun;
    public string NameItem => namegun; 
    
    private bool _fire = false;
    
    
    
    [Header("Main")]
    [SerializeField] private string namegun;
    [SerializeField] private int indexgun;

    [Header("Fire point")] [SerializeField]
    private Transform FirePoint;

    [SerializeField] private AudioSource FireSound;
    [SerializeField] private AudioSource SwitchModeSound;
    
    [Header("IK")]
    [SerializeField] private Transform _mainRightIKTarget;
    [SerializeField] private Transform _mainLeftIKTarget;
   
    [SerializeField] private Transform _mainRightIKTargetOffset;
    [SerializeField] private Transform _mainLeftIKTargetOffset;
    
    [SerializeField] private Transform _shadowRightIKTarget;
    [SerializeField] private Transform _shadowLeftIKTarget;
   
    [SerializeField] private Transform _shadowRightIKTargetOffset;
    [SerializeField] private Transform _shadowLeftIKTargetOffset;
    
    internal Transform MainRightIKTarget => _mainRightIKTarget;
    internal Transform MainLeftIKTarget => _mainLeftIKTarget;
    internal Transform MainRightIKTargetOffset => _mainRightIKTargetOffset;
    internal Transform MainLeftIKTargetOffset => _mainLeftIKTargetOffset;
    internal Transform ShadowRightIKTarget => _shadowRightIKTarget;
    internal Transform ShadowLeftIKTarget => _shadowLeftIKTarget;
    internal RuntimeAnimatorController WeaponAnimController => _weaponAnimController;
    internal Transform ShadowRightIKTargetOffset => _shadowRightIKTargetOffset;
    internal Transform ShadowLeftIKTargetOffset => _shadowLeftIKTargetOffset;
    
    internal UnityAction Shooting;
    internal UnityAction RecoilClear;
    internal UnityAction StopAttacking;

    private float nextFireTime;
    public float bulletSpeed = 100f;

    private FireMode currentMode;
    private List<FireMode> _cachedModes;
    private bool _modesDirty = true;
    
    // Новые поля для управления режимами
    private bool _previousClickState;
    private bool _needSingleShot;
    private int _burstCounter;
    private bool _isBursting;
    
    public void Start()
    {
        _fire = false;
        currentMode = Data.ShootWeaponData.CurrentMode;
        _cachedModes = GetAvailableModes();
    }

    private List<FireMode> GetAvailableModes()
    {
        if (!_modesDirty && _cachedModes != null) 
            return _cachedModes;
    
        _cachedModes = new List<FireMode>();
        if (Data.ShootWeaponData.AllowAuto) _cachedModes.Add(FireMode.Auto);
        if (Data.ShootWeaponData.AllowSingle) _cachedModes.Add(FireMode.Single);
        if (Data.ShootWeaponData.AllowBurst) _cachedModes.Add(FireMode.Burst);
    
        _modesDirty = false;
        return _cachedModes;
    }
    
    public void Attack(bool mouseLeftClick)
    {
        
        bool wasPressed = _previousClickState;
        _previousClickState = mouseLeftClick;
        
        switch (currentMode)
        {
            case FireMode.Auto:
                // Для автоматического режима обрабатываем отпускание кнопки
                if (!mouseLeftClick && wasPressed)
                {
                    StopAttacking?.Invoke();
                }
                _fire = mouseLeftClick;
                break;

            case FireMode.Single:
                if (mouseLeftClick && !wasPressed)
                {
                    _needSingleShot = true;
                }
                break;

            case FireMode.Burst:
                if (mouseLeftClick && !wasPressed && !_isBursting)
                {
                    _isBursting = true;
                    _burstCounter = Data.ShootWeaponData.BurstSize;
                    _fire = true;
                }
                break;
        }
    }

    public void Fire()
    {
        
        Vector3 spread = FirePoint.forward;
        GameObject bullet = Instantiate(Data.ShootWeaponData.BulletPrefab, FirePoint.position, Quaternion.LookRotation(spread));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb) rb.linearVelocity = spread * bulletSpeed;
        PlayFireSound();
    }
    
 
    
    /*
    
    public void CycleFireMode()
    {
        if(_fire==true) return;
        
        FireMode nextMode = currentMode;
        
        // Поиск следующего доступного режима
        do
        {
            nextMode = nextMode switch
            {
                FireMode.Auto => FireMode.Single,
                FireMode.Single => FireMode.Burst,
                FireMode.Burst => FireMode.Auto,
                _ => FireMode.Single
            };
            
       
            
        } 
        while (!IsModeAllowed(nextMode));

        if (nextMode != currentMode)
        {
            currentMode = nextMode;
            Debug.Log($"Switched to {nextMode} mode");
            
            PlaySound(Data.SoundWeaponData.FireModeSounds, SwitchModeSound);
        }
    }

    private bool IsModeAllowed(FireMode mode)
    {
        Debug.Log("nextMode: "+mode);
        
        return mode switch
        {
            FireMode.Auto => Data.ShootWeaponData.AllowAuto,
            FireMode.Single => Data.ShootWeaponData.AllowSingle,
            FireMode.Burst => Data.ShootWeaponData.AllowBurst,
            _ => true
        };
    }
    */
    
    
    public void CycleFireMode()
    {
        if (_fire == true) return;

        // Получаем список доступных режимов

        // Если нет доступных режимов, используем Single как fallback
        if (_cachedModes.Count == 0)
        {
            currentMode = FireMode.Single;
            Debug.LogWarning("No fire modes available! Defaulting to Single");
            return;
        }

        // Находим индекс текущего режима в списке доступных
        int currentIndex = _cachedModes.IndexOf(currentMode);
    
        // Если текущий режим недоступен, выбираем первый доступный
        if (currentIndex == -1)
        {
            currentMode = _cachedModes[0];
            Debug.Log($"Switched to {currentMode} mode (fallback)");
            PlaySound(Data.SoundWeaponData.FireModeSounds, SwitchModeSound);
            return;
        }

        // Вычисляем следующий индекс (с зацикливанием)
        int nextIndex = (currentIndex + 1) % _cachedModes.Count;
        FireMode nextMode = _cachedModes[nextIndex];

        // Применяем новый режим
        currentMode = nextMode;
        Debug.Log($"Switched to {nextMode} mode");
    
        PlaySound(Data.SoundWeaponData.FireModeSounds, SwitchModeSound);
    }
    
    
    public string GetCurrentModeName()
    {
        /*
        if (!IsModeAllowed(currentMode))
            AutoCorrectFireMode();
    */
        return currentMode switch
        {
            FireMode.Auto => "AUTO",
            FireMode.Single => "SEMI",
            FireMode.Burst => $"{Data.ShootWeaponData.BurstSize}-RND",
            _ => "UNKNOWN"
        };
    }
    
    
    private void Update()
    {
        bool shouldShoot = false;
        
        // Проверка условий для выстрела
        if (Time.time >= nextFireTime)
        {
            if (_fire || _needSingleShot)
            {
                shouldShoot = true;
            }
        }

        if (shouldShoot)
        {
            Shoot();
            nextFireTime = Time.time + Data.ShootWeaponData.FireRate;
            
            // Обработка одиночного режима
            if (_needSingleShot)
            {
                _needSingleShot = false;
                StopAttacking?.Invoke();
            }
            
            // Обработка очереди
            if (_isBursting)
            {
                _burstCounter--;
                if (_burstCounter <= 0)
                {
                    _isBursting = false;
                    _fire = false; // Прекращаем стрельбу
                    StopAttacking?.Invoke();
                }
            }
        }
        else if (_fire) // Сброс отдачи если не стреляем
        {
            RecoilClear?.Invoke();
        }
        
        // Для автоматического режима: если кнопка отпущена, но флаг еще активен
        if (Data.ShootWeaponData.CurrentMode == FireMode.Auto && 
            !_previousClickState && 
            _fire)
        {
            _fire = false;
            StopAttacking?.Invoke();
        }
        
    }

    // Существующие методы
    private void Shoot() => Shooting?.Invoke();
    
    
    private void PlaySound(SoundCollection soundCollection, AudioSource source)
    {
        if (soundCollection.Clips == null || soundCollection.Clips.Length == 0) 
            return;
    
        if (source == null)
            source = FireSound;
    
        AudioClip clip = soundCollection.Clips[Random.Range(0, soundCollection.Clips.Length)];
    
        source.pitch = 1f + Random.Range(-soundCollection.PitchVariation, soundCollection.PitchVariation);
        source.PlayOneShot(clip, soundCollection.Volume);
    }

// Использование:
    private void PlayFireSound()
    {
        PlaySound(Data.SoundWeaponData.FireSounds, FireSound);
    }
    
    /*
    private void PlayEmptyFireSound()
    {
        if (!_hasAmmo && Data.ShootWeaponData.EmptyFireSounds.Clips.Length > 0)
        {
            PlaySound(Data.ShootWeaponData.EmptyFireSounds, _fireAudioSource);
        }
        else if (Data.ShootWeaponData.FireSounds.Clips.Length > 0)
        {
            // Проигрываем обычный звук, но с пониженной громкостью
            SoundCollection suppressed = Data.ShootWeaponData.FireSounds;
            suppressed.Volume *= 0.3f;
            PlaySound(suppressed, _fireAudioSource);
        }
    }*/
    
    
    
}
