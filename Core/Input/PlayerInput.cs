using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerControls InputSystem { get; private set; }
    public PlayerControls.PlayerActions PlayerActions { get; private set; }
    public PlayerControls.UIActions PlayerActionsUI { get; private set; }
    
    
    public void Awake()
    {
        InputSystem = new PlayerControls();
        PlayerActions = InputSystem.Player;
        PlayerActionsUI = InputSystem.UI;
        
        
        PlayerActions.Move.Disable();
        
    }


    private void OnEnable()
    {
        InputSystem.Enable();
    }

    private void OnDisable()
    {
        InputSystem.Disable();
    }

    public void DisableActionFor(InputAction action, float seconds)
    {
        StartCoroutine(DisableAction(action, seconds));
    }

    private IEnumerator DisableAction(InputAction action, float seconds)
    {
        action.Disable();
        yield return new WaitForSeconds(seconds);
        action.Enable();
    }
    
}
