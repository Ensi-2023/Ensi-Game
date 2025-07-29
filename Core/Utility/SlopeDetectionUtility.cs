using UnityEngine;

public class SlopeDetectionUtility 
{
    private readonly PlayerReusableData _reusableData;
    private readonly CharacterController _characterController;
    private readonly LayerMask _groundLayer;
    private readonly Transform _transform;
    private readonly PlayerGroundedData _playerGroundedData;
    public SlopeDetectionUtility(PlayerController player)
    {
        _reusableData = player.StateMashine.ReusableData;
        _characterController = player.CharacterController;
        _groundLayer = player.Data.GroudedData.LayerData.GroundLayerMask;
        _transform = player.gameObject.transform;
        _playerGroundedData = player.Data.GroudedData;
    }
    
    public void DetectSlope()
    {
        Vector3 origin = _characterController.bounds.center;
     
        // Сбрасываем флаги
        _reusableData.FrontSlopeHit = false;
        _reusableData.BackSlopeHit = false;
        _reusableData.LeftSlopeHit = false;
        _reusableData.RightSlopeHit = false;

        // Передний луч (в направлении взгляда)
        CastSlopeRay(origin, _transform.forward, ref _reusableData.FrontSlopeHit);
        // Задний луч (противоположный взгляду)
        CastSlopeRay(origin, -_transform.forward, ref _reusableData.BackSlopeHit);
        // Левый луч
        CastSlopeRay(origin, -_transform.right, ref _reusableData.LeftSlopeHit);
        // Правый луч
        CastSlopeRay(origin, _transform.right, ref _reusableData.RightSlopeHit);
    }

    private void CastSlopeRay(Vector3 origin, Vector3 direction, ref bool hitFlag)
    {
        Vector3 rayDirection = Quaternion.AngleAxis(-_playerGroundedData.SlopeRayAngle, 
            Vector3.Cross(direction, Vector3.up)) * direction;
        
        Debug.DrawRay(origin, rayDirection * _playerGroundedData.SlopeRayLength, Color.blue);

        if (Physics.Raycast(origin, rayDirection, out RaycastHit hit, 
                _playerGroundedData.SlopeRayLength, _groundLayer))
        {
            hitFlag = true;
        }
    }
    
}
