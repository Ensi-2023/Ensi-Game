using UnityEngine;

public class FootIKFix : MonoBehaviour {
  
    public Transform leftFootTarget;
    public Transform rightFootTarget;

    [Range(0, 1)] public float footDistanceToGround = 0.5f;
    [Range(0, 1)] public float raycastDistance = 1.5f;
    [SerializeField] private LayerMask groundLayer;

    private Animator _animator;
    private Vector3 _leftFootPos;
    private Quaternion _leftFootRot;
    private Vector3 _rightFootPos;
    private Quaternion _rightFootRot;

    private bool _leftFootHit;
    private bool _rightFootHit;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (_animator == null)
            return;

        // Получаем текущие позиции и ориентацию стоп от анимации
        _leftFootPos = _animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        _leftFootRot = _animator.GetIKRotation(AvatarIKGoal.LeftFoot);

        _rightFootPos = _animator.GetIKPosition(AvatarIKGoal.RightFoot);
        _rightFootRot = _animator.GetIKRotation(AvatarIKGoal.RightFoot);

        // Проверяем поверхность под левой ногой
        _leftFootHit = Physics.Raycast(_leftFootPos + Vector3.up * 0.5f, Vector3.down, out RaycastHit leftHit,
            raycastDistance, groundLayer);
        if (_leftFootHit)
        {
            _leftFootPos = leftHit.point + leftHit.normal * footDistanceToGround;
            Quaternion fromTo = Quaternion.FromToRotation(Vector3.up, leftHit.normal);
            _leftFootRot = fromTo * _leftFootRot;
        }

        // Проверяем поверхность под правой ногой
        _rightFootHit = Physics.Raycast(_rightFootPos + Vector3.up * 0.5f, Vector3.down, out RaycastHit rightHit,
            raycastDistance, groundLayer);
        if (_rightFootHit)
        {
            _rightFootPos = rightHit.point + rightHit.normal * footDistanceToGround;
            Quaternion fromTo = Quaternion.FromToRotation(Vector3.up, rightHit.normal);
            _rightFootRot = fromTo * _rightFootRot;
        }

        // Устанавливаем новые позиции и вращение стоп
        _animator.SetIKPosition(AvatarIKGoal.LeftFoot, _leftFootPos);
        _animator.SetIKRotation(AvatarIKGoal.LeftFoot, _leftFootRot);

        _animator.SetIKPosition(AvatarIKGoal.RightFoot, _rightFootPos);
        _animator.SetIKRotation(AvatarIKGoal.RightFoot, _rightFootRot);

        // Вес влияния IK (можно сделать динамическим)
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _leftFootHit ? 1f : 0f);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _leftFootHit ? 1f : 0f);

        _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, _rightFootHit ? 1f : 0f);
        _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, _rightFootHit ? 1f : 0f);
    }
}