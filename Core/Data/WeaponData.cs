using System;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void DisableTriggerAllColliders()
    {
        // Получаем ВСЕ коллайдеры (включая дочерние объекты)
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            col.isTrigger = true; // Отключаем каждый коллайдер
        }

     
    }
    
    
    public void EnableTriggerAllColliders()
    {
        // Получаем ВСЕ коллайдеры (включая дочерние объекты)
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            col.isTrigger = false; 
        }
        
    }


    public void EnableKinematic()
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    public void DisableKinematic()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }
    
    public void AddForce(Vector3 throwDirection)
    {
        if (rb != null)
        {
            rb.AddForce(throwDirection * 0.5f, ForceMode.Impulse);
        }
    }
}
