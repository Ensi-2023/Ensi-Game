using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    /*****************************************************/
    /*************     STATE  MASHINE        *************/
    /*****************************************************/
    [field: SerializeField] internal PlayerStateMashine StateMashine { get; private set; }
    [field: SerializeField] internal PlayerHandStateMashine HandStateMashine { get; private set; }
    [field: SerializeField] internal PlayerDataScriptableObject Data { get; private set; }
    [field: SerializeField] internal PlayerStepAudioClips StepAudioClips { get; private set; }
    [field: SerializeField] internal PlayerInput Input { get; private set; }
    [field: SerializeField] internal SlopeDetectionUtility SlopeDetector { get; private set; }
    
    
    
    
    /*****************************************************/


    /*****************************************************/
    /*************    PRIVATE  VARIABLE      *************/
    /*****************************************************/

   // [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _cameraRoute;
    [SerializeField] private Transform _handpoint;
    [SerializeField] private Transform _handRoute;
    [SerializeField] private Transform _weaponContainer;
    [SerializeField] private Transform _headRZ;
    [SerializeField] private Transform _headRX;
    [SerializeField] private Transform _weaponRoute;
    [SerializeField] private Transform _headRoute;
    [SerializeField] private Transform _recoilRoute;
    [SerializeField] private Transform _cameraShake;
    [SerializeField] private AnimationData _animatorData;
    [SerializeField] private Animator _animator;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private GameObject pickupButtonUI;
    [SerializeField] private Animator _weaponAnimator;
    [SerializeField] private TransformShakeCore _transformShake;
    
    
    [SerializeField] private AudioSource _footstepAudio;
    
    private PlayerIK _playerIK;
    private CharacterController _characterController;
    private AnimationHashVariable _currentAnimationHash;
    private Vector3 _originalCamPosition;
    private Quaternion _originalCamRotation;
    private Vector3 _originalHeadRZPosition;
    private Vector3 _originalHeadRXPosition;
    private PlayerFootstepSound _footstepSound;
    private Quaternion _originalHeadRZCamRotation;
    private Quaternion _originalHeadRXCamRotation;
    /*****************************************************/
    
    internal PlayerIK PlayerIK => _playerIK;
    internal Camera MainCamera => _camera;
    internal Transform MainCameraRoute => _cameraRoute;
    internal Transform HandPoint => _handpoint;
    internal Transform HandRoute => _handRoute;
    internal Transform HeadRoute => _headRoute;
    internal Transform WeaponRoute => _weaponRoute;
    internal Transform CameraShake => _cameraShake;
    internal Transform HeadRZ => _headRZ;
    internal Transform HeadRX => _headRX;
    
    internal AudioSource FootstepAudio => _footstepAudio;
    
    internal PlayerFootstepSound FootstepSound => _footstepSound;
    internal TransformShakeCore TransformShake => _transformShake;
    internal Transform RecoilRoute => _recoilRoute;
    internal Transform WeaponContainer => _weaponContainer;
    internal Transform Orientation => _orientation;
    internal GameObject PickupButtonUI => pickupButtonUI;
    internal AnimationData AnimatorData => _animatorData;
    internal Animator Animator => _animator;
    internal Animator WeaponAnimator => _weaponAnimator;
    internal GroundChecker GroundChecker => _groundChecker;
    
    internal AnimationHashVariable AnimationHash => _currentAnimationHash;
    internal CharacterController CharacterController => _characterController;
    
    internal Vector3 CameraOriginalPosition => _originalCamPosition;
    
    internal Vector3 OriginalHeadRZPosition => _originalHeadRZPosition;
    internal Vector3 OriginalHeadRXPosition => _originalHeadRXPosition;
    public Quaternion CameraOriginalRotation => _originalCamRotation;
    
    public Quaternion OriginalHeadRXCamRotation => _originalHeadRXCamRotation;
    public Quaternion OriginalHeadRZCamRotation => _originalHeadRZCamRotation;




    public void Awake()
    {
        _camera = Camera.main;
        _playerIK = GetComponent<PlayerIK>();
        _characterController = GetComponent<CharacterController>();

        Input = GetComponent<PlayerInput>();
        _animatorData = GetComponent<AnimationData>();
        _groundChecker = GetComponent<GroundChecker>();
        _groundChecker.Inizialize(this.transform);
        _transformShake = GetComponent<TransformShakeCore>();

        _footstepSound = new PlayerFootstepSound();
        _footstepSound.Initialize(_footstepAudio,_groundChecker,StepAudioClips);
        
        
        StateMashine = new PlayerStateMashine(this);
        HandStateMashine = new PlayerHandStateMashine(this);
        SlopeDetector = new SlopeDetectionUtility(this);
        _currentAnimationHash = GetComponent<AnimationHashVariable>();


        _originalCamPosition = _cameraShake.transform.localPosition;
        _originalCamRotation = _cameraShake.transform.localRotation;
        
        _originalHeadRZPosition = _headRZ.transform.localPosition;
        _originalHeadRXPosition = _headRX.transform.localPosition;
        
        _originalHeadRXCamRotation = _headRX.transform.localRotation;
        _originalHeadRZCamRotation = _headRZ.transform.localRotation;
        
        if (_animator == null)
            throw new NullReferenceException("Animator is NULL");
        
    }

    public string GetCurrentStateName() => StateMashine.CurrentState.GetType().Name;
    public string GetCurrentHandStateName() => HandStateMashine.CurrentState.GetType().Name;

    private void Update()
    {
        StateMashine?.HandleInput();
        StateMashine?.Update();

        HandStateMashine?.HandleInput();
        HandStateMashine?.Update();
    }

    private void OnCollisionEnter(Collision collision)
    {
        StateMashine?.OnCollisionEnter(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        StateMashine?.OnCollisionExit(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        StateMashine?.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        StateMashine?.OnTriggerExit(other);
    }

    public void DestroyObj(GameObject obj)
    {
        Destroy(obj);
    }

    private void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _animatorData.Inizialise();

        StateMashine.Change(StateMashine.IdleState); //idle
        HandStateMashine.Change(HandStateMashine.IdleState);
        
        pickupButtonUI = GameObject.Find("PickupButtonUI");
        pickupButtonUI.SetActive(false);
    }

    private void FixedUpdate()
    {
        StateMashine?.PhysicsUpdate();
        HandStateMashine?.PhysicsUpdate();
    }

    private void LateUpdate()
    {
        StateMashine?.LateUpdate();
        HandStateMashine?.LateUpdate();
    }

}
