using System;
using UnityEngine;

[Serializable]
public class CapsuleColliderUtility 
{
    public CapsuleColliderData CapsuleColliderData { get; private set; }
    [field:SerializeField] public DefaultColliderData DefaultColliderData { get; private set; }
    [field:SerializeField] public SlopeData SlopeData { get; private set; }
     
    public void Initialize(GameObject capsuleColliderObject)
    {
        if (CapsuleColliderData != null)
        {
            return;
        }
        CapsuleColliderData = new CapsuleColliderData();
        CapsuleColliderData.Initialize(capsuleColliderObject);
      
    }

    public void CalculateCapsuleColliderDimensions()
    {
        SetCapsuleColliderRadius(DefaultColliderData.Radius);
        SetCapsuleColliderHeight(DefaultColliderData.Height * (1f - SlopeData.SlopeHeightPercentage));

        RecalculateCapsuleColliderCanter();

        var halfColliderHeight = CapsuleColliderData.Collider.height / 2f;
        
        if (halfColliderHeight < CapsuleColliderData.Collider.radius)
        {
            SetCapsuleColliderRadius(halfColliderHeight);
        }
        
        CapsuleColliderData.UpdateColliderData();

    }
    
    public void CalculateCapsuleColliderDimensions(float value = 0f)
    {
        SetCapsuleColliderRadius(DefaultColliderData.Radius);
        SetCapsuleColliderHeight(DefaultColliderData.Height * (1f - value));

        RecalculateCapsuleColliderCanter();

        var halfColliderHeight = CapsuleColliderData.Collider.height / 2f;
        
        if (halfColliderHeight < CapsuleColliderData.Collider.radius)
        {
            SetCapsuleColliderRadius(halfColliderHeight);
        }
        
        CapsuleColliderData.UpdateColliderData();
    }
    

    private void RecalculateCapsuleColliderCanter()
    {
        float colliderHeight = DefaultColliderData.Height - CapsuleColliderData.Collider.height;
        Vector3 newCapsuleCenter = new(0f, DefaultColliderData.CenterY + (colliderHeight / 2f), 0f);
        
        CapsuleColliderData.Collider.center = newCapsuleCenter;
    }

    private void SetCapsuleColliderRadius(float radius)
    {
        CapsuleColliderData.Collider.radius = radius;
    }
    
    private void SetCapsuleColliderHeight(float height)
    {
        CapsuleColliderData.Collider.height = height;
    }
    
}
