using System;
using UnityEngine;

[Serializable]
public class PlayerLayerData
{
   [field: Header("Player Layer Grounded Data")]
   [field: SerializeField]
   [field: Range(-20f, 2f)]
   public float GroundedOffset { get; private set; } = 1f;
   
   [field: SerializeField]
   [field: Range(0f, 10f)]
   public float GroundedRadius { get; private set; } = 0.28f;
   
   [field: SerializeField]
   [field: Range(0f, 10f)]
   public float CheckDistance { get; private set; } = 1.28f;
  
   [field: SerializeField]
   public LayerMask GroundLayerMask { get; private set; }

   [field: SerializeField]
   public LayerMask WeaponLayerMask { get; private set; }

   
   public bool ContainsLayer(LayerMask layerMask, int layer)
   {
      return (1 << layer & layerMask) != 0;
   }

   public bool IsGroundLayer(int layer)
   {
      return ContainsLayer(GroundLayerMask, layer);
   }
   
}
