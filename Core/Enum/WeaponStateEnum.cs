using UnityEngine;

public class WeaponStateEnum 
{
    public enum MoveState
    {
        Idle,Run,Sprint,Jump,Crouch,Falling,Prone,Walk,Landing
    }
    
    public enum FireMode
    {
        Auto,
        Single,
        Burst
    }
}
