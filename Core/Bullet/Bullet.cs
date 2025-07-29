using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    public float initialSpeed = 100f;
    public float damage = 25f;
    public float lifeTime = 5f;
    public LayerMask collisionMask;
    public LayerMask weaponCollisionMask;

    [Header("Impact")]
    public GameObject impactEffect;
    public GameObject decalPrefab;
    public float decalLifetime = 30f;
    public float decalOffset = 0.025f;

    private Rigidbody rb;
    private bool hasHit;
    
    
    
    
    void Start()
    {
     //  
     rb = GetComponent<Rigidbody>();
     rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
     rb.interpolation = RigidbodyInterpolation.Interpolate;
       
     Destroy(gameObject, lifeTime);
    }
    
    
    void FixedUpdate()
    {
        if (!hasHit)
        {
            // Автоматическая ориентация пули по траектории
          //  transform.forward = rb.linearVelocity.normalized;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;
    

        
        // Проверка по слоям
        if (((1 << collision.gameObject.layer) & collisionMask) != 0)
        {
            ProcessImpact(collision);
            

        }
    
        Destroy(gameObject);
    }
    
    void ProcessImpact(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
    
        // 1. Эффект попадания (вспышка, искры)
        if (impactEffect)
        {
            GameObject effect = Instantiate(impactEffect, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(effect, 2f);
        }
    
        // 2. Декаль (след от пули)
        if (decalPrefab)
        {
            CreateBulletDecal(contact);
        }
        
    

    }

    
    void CreateBulletDecal(ContactPoint contact)
    {
        // Создаем декаль с небольшим смещением от поверхности
        Vector3 decalPosition = contact.point + contact.normal * decalOffset;
        GameObject decal = Instantiate(decalPrefab, decalPosition, Quaternion.LookRotation(-contact.normal));
    
        // Привязываем к объекту
        decal.transform.SetParent(contact.otherCollider.transform);
    
        // Случайный поворот для натуральности
        decal.transform.Rotate(Vector3.forward, Random.Range(0, 360f), Space.Self);
    
        // Автоматическое удаление
        Destroy(decal, decalLifetime);
    }
    
}
