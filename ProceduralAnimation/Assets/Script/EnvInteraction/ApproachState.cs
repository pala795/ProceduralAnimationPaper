using System;
using UnityEngine;

public class ApproachState : EnvInteractionState
{
   float _approachWeight = 0.5f;
   float _elapsedTime = 0f;
   float _lerpDuration = 5f;
   float _rotationSpeed = 500f;
   float _approachRotationWeight = 0.75f;
   float _riseDistanceTreeshold = 0.5f;
   float _approachDuration = 2f;
   
   public ApproachState(EnvInteractionContext context, EnvInteractionStateMachine.EEnvInteractionState estate) : base(
      context, estate)
   {
      EnvInteractionContext Context = context;
   }

   public override void EnterState()
   {
      Debug.Log("Entering state " + StateKey);
      _elapsedTime = 0f;
   }
   public override void ExitState(){}

   public override void UpdateState()
   {
      Quaternion expectedGroundRotation =  Quaternion.LookRotation(-Vector3.up, _context._rootTransform.forward); 
      _elapsedTime += Time.deltaTime;
      
      _context.CurrentIKTargetTransform.rotation = Quaternion.RotateTowards(_context.CurrentIKTargetTransform.rotation, expectedGroundRotation, _rotationSpeed * Time.deltaTime);
      _context.CurrentIKConstraint.weight = Mathf.Lerp(_context.CurrentIKConstraint.weight, _approachWeight, _elapsedTime/_lerpDuration);
      _context.CurrentIKRotationConstraint.weight = Mathf.Lerp(_context.CurrentIKRotationConstraint.weight, _approachRotationWeight, _elapsedTime/_lerpDuration);
      
   }

   public override EnvInteractionStateMachine.EEnvInteractionState GetNextState()
   {
      if (CheckShouldReset())
      {
         return EnvInteractionStateMachine.EEnvInteractionState.Reset; 
      }
      bool isOverStateLifeDuration = _elapsedTime >= _approachDuration;
      if (isOverStateLifeDuration)
      {
         return  EnvInteractionStateMachine.EEnvInteractionState.Reset;
      }
      bool isWithinArmsReach = Vector3.Distance(_context.ClosestPointOnColliderFromShoulder,_context.CurrentShoulderTransform.position) < _riseDistanceTreeshold;
      bool isClosestPointOnColliderReal = _context.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;

      if (isWithinArmsReach && isClosestPointOnColliderReal)
      {
         return EnvInteractionStateMachine.EEnvInteractionState.Rise;
      }
      return StateKey;
   }

   public override void OnTriggerEnter(Collider other)
   {
      StartIkTracking(other);
   }

   public override void OnTriggerStay(Collider other)
   {
      UpdateIkTracking(other);
   }

   public override void OnTriggerExit(Collider other)
   {
      ResetIkTracking(other);
   }
}

