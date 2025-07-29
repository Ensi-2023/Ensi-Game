using UnityEngine;

public class LertToRegularPositionAndRotation : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _positionLerpTime = 8f;
    [SerializeField, Min(0f)] private float _rotationLerpTime = 8f;
    
    private Transform _characterTransform;
    
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    

    void Awake()
    {
        _characterTransform = transform;
        _originalPosition = _characterTransform.localPosition;
        _originalRotation = _characterTransform.localRotation;
    }

  
    void Update()
    {
        var position = _characterTransform.localPosition;
        var rotation = _characterTransform.localRotation;

        rotation = Quaternion.Lerp(rotation, _originalRotation, Time.deltaTime * _rotationLerpTime);
        position = Vector3.Lerp(position,_originalPosition, Time.deltaTime * _positionLerpTime);
        
        _characterTransform.localPosition = position;
        _characterTransform.localRotation = rotation;

    }
}
