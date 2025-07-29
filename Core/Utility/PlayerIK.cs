using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerIK : MonoBehaviour
{
      [Header("MAIN MODEL")]
      [SerializeField] private TwoBoneIKConstraint _mainLeftConstraint;
      [SerializeField] private TwoBoneIKConstraint _mainRightConstraint;
      [SerializeField] private Rig _mainRigBody;
      [SerializeField] private  RigBuilder _mainRigBuilder;
      
      internal TwoBoneIKConstraint LeftConstraint => _mainLeftConstraint;
      internal TwoBoneIKConstraint RightConstraint => _mainRightConstraint;
      internal Rig RigBody => _mainRigBody;
      internal RigBuilder RigBuilder => _mainRigBuilder;
      
      [Header("Only Shadow Model")]
      [SerializeField] private TwoBoneIKConstraint _shadowLeftConstraint;
      [SerializeField] private TwoBoneIKConstraint _shadowRightConstraint;
      [SerializeField] private Rig _shadowRigBody;
      [SerializeField] private  RigBuilder _shadowRigBuilder;
      
      internal TwoBoneIKConstraint ShadowLeftConstraint => _shadowLeftConstraint;
      internal TwoBoneIKConstraint hadowRightConstraint => _shadowRightConstraint;
      internal Rig hadowRigBody => _shadowRigBody;
      internal RigBuilder hadowRigBuilder => _shadowRigBuilder;

      internal void BuildRig()
      {
          _shadowRigBuilder.Build();
          _mainRigBuilder.Build();
      }
      
      internal void SetConstraint(
          Transform mainLeftConstraint, 
          Transform mainLeftConstraintOffset,
          Transform mainRightConstraint, 
          Transform mainRightConstraintOffset, 
          Transform shadowLeftConstraint,
          Transform shadowRightConstraint,
          Transform shadowLeftOffset,
          Transform shadowRightOffset,
          
          bool isRebuild = false)
      {

          _mainLeftConstraint.weight = 1f;
          _mainLeftConstraint.data.target = mainLeftConstraint;
          _mainLeftConstraint.data.hint = mainLeftConstraintOffset;

          _mainRightConstraint.weight = 1f;
          _mainRightConstraint.data.target = mainRightConstraint;
          _mainRightConstraint.data.hint = mainRightConstraintOffset;
          
          
          _shadowLeftConstraint.weight = 1f;
          _shadowLeftConstraint.data.target = shadowLeftConstraint;
          _shadowLeftConstraint.data.hint = shadowLeftOffset;

          _shadowRightConstraint.weight = 1f;
          _shadowRightConstraint.data.target = shadowRightConstraint;
          _shadowRightConstraint.data.hint = shadowRightOffset;
          
          if (isRebuild)
              BuildRig();

      }


      internal void ClearConstraint()
      {
          _shadowLeftConstraint.weight = 0f;
          _shadowLeftConstraint.data.target = null;
          _shadowLeftConstraint.data.hint = null;

          _shadowRightConstraint.weight = 0f;
          _shadowRightConstraint.data.target = null;
          _shadowRightConstraint.data.hint = null;
          
          _mainLeftConstraint.weight = 0f;
          _mainLeftConstraint.data.target = null;
          _mainLeftConstraint.data.hint = null;

          _mainRightConstraint.weight = 0f;
          _mainRightConstraint.data.target = null;
          _mainRightConstraint.data.hint = null;
          
      }

}
