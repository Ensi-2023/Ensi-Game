
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool RunPressed { get; private set; }
    public bool CrouchPressed { get; private set; }
    
    public bool PronePressed { get; private set; }
    public bool DashPressed { get; private set; }
    public bool JumpPressed { get; private set; }
    
    private PlayerControls _playerControls;
    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        if (_playerControls == null) 
            _playerControls = new PlayerControls();
        
        _playerControls.Player.Enable();   
        _playerControls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _playerControls.Player.Move.canceled += ctx => MoveInput = Vector2.zero;
        _playerControls.Player.Sprint.performed += ctx => RunPressed = ctx.ReadValueAsButton();
        _playerControls.Player.Sprint.canceled += ctx => RunPressed = false;
        _playerControls.Player.Crouch.performed += ctx => CrouchPressed = ctx.ReadValueAsButton();
        _playerControls.Player.Crouch.canceled += ctx => CrouchPressed = false;
        _playerControls.Player.Jump.performed += ctx => JumpPressed = ctx.ReadValueAsButton();
        _playerControls.Player.Jump.canceled += ctx => JumpPressed = false;

        _playerControls.Player.Look.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        _playerControls.Player.Look.canceled += ctx => LookInput = Vector2.zero;
    }
    private void OnDisable()
    {
        _playerControls.Player.Disable();
    }


    private void Update()
    {
       
    }
}
