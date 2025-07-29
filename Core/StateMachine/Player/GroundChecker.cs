using UnityEngine;

public class GroundChecker : MonoBehaviour
{  
    private Transform _playerTransform;
    private Vector3 _lastSpherePosition;
    private float _lastSphereRadius;
    private bool _lastGroundedState;
    
    
    private Collider _lastGroundCollider;
    private TypeSound _currentSurfaceType = TypeSound.Gravel;
    
    public void Inizialize(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    public virtual bool IsGroundedCheck(LayerData _layerData)
    {

        IsMoveToGroundFixBounce();
        
        
        _lastSpherePosition = new Vector3(
            _playerTransform.position.x,
            _playerTransform.position.y - _layerData.GroundedOffset,
            _playerTransform.position.z
        );
        
        _lastSphereRadius = _layerData.GroundedRadius;
        
        
            /*
        
        _lastGroundedState = Physics.CheckSphere(
            _lastSpherePosition,
            _lastSphereRadius,
            _layerData.GroundLayerMask,
            QueryTriggerInteraction.Ignore
        );*/
            
            // Заменяем CheckSphere на OverlapSphere
            Collider[] colliders = Physics.OverlapSphere(
                _lastSpherePosition,
                _lastSphereRadius,
                _layerData.GroundLayerMask,
                QueryTriggerInteraction.Ignore
            );
        
            _lastGroundedState = colliders.Length > 0;
            _lastGroundCollider = _lastGroundedState ? colliders[0] : null;
        
            // Определяем тип поверхности
            if (_lastGroundedState)
            {
                UpdateSurfaceType();
            }
        
            return _lastGroundedState;
            
       
      //  return _lastGroundedState;
    }

    private void IsMoveToGroundFixBounce()
    {
         
    }

    private void UpdateSurfaceType()
    {
        if (_lastGroundCollider == null) return;
        
        TypeItem typeItem = _lastGroundCollider.GetComponent<TypeItem>();
        if (typeItem != null && typeItem.TypeSound != _currentSurfaceType)
        {
            _currentSurfaceType = typeItem.TypeSound;
        }
    }

    public TypeSound GetCurrentSurfaceType() => _currentSurfaceType;
    
    // Метод для отрисовки в редакторе
    public void DrawGizmos()
    {
        if (_playerTransform == null) return;
        
        // Сохраняем текущий цвет
        Color originalColor = Gizmos.color;
        
        // Устанавливаем цвет в зависимости от состояния
        Gizmos.color = _lastGroundedState ? Color.green : Color.red;
        
        // Рисуем сферу
        Gizmos.DrawWireSphere(_lastSpherePosition, _lastSphereRadius);
        
        // Рисуем линию до центра
        Gizmos.DrawLine(
            _playerTransform.position, 
            _lastSpherePosition
        );
        
        // Восстанавливаем цвет
        Gizmos.color = originalColor;

    }

    void OnDrawGizmos()
    {
        DrawGizmos();
    }

}

public struct LayerData
{
    public float GroundedOffset;
    public float GroundedRadius;
    public LayerMask GroundLayerMask;
}
