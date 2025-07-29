using UnityEngine;

public class AnimationHashVariable:MonoBehaviour
{

    [SerializeField] private string _walkingVariable = "IsWalk";
    [SerializeField] private string _idleVariable = "IsIdle";
    [SerializeField] private string _sprintVariable = "IsSprint";
    [SerializeField] private string _aimingVariable = "IsAim";

    private int _walkingHash;
    private int _idleHash;
    private int _sprintHash;
    private int _aimingHash;
    
    public int WalkingHash => _walkingHash;
    public int IdleHash => _idleHash;
    public int SprintHash => _sprintHash;
    public int AimingHash => _aimingHash;
    
    public void Inizialize(Animator animator)
    {
        _walkingHash = Animator.StringToHash(_walkingVariable);
        _idleHash = Animator.StringToHash(_idleVariable);
        _sprintHash = Animator.StringToHash(_sprintVariable);
        _aimingHash = Animator.StringToHash(_aimingVariable);
    }
}
