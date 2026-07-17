using UnityEngine;

public class ResetState: EnvInteractionState
{
    float _elapsedTime = 0f;
    float _resetTime = 2f;
    float _lerpDuration = 10f;
    float _rotationSpeed = 500f;
    
    public ResetState(EnvInteractionContext context, EnvInteractionStateMachine.EEnvInteractionState estate) : base(context , estate)
    {
        EnvInteractionContext _context = context;    
    }

    public override void EnterState()
    {
        Debug.Log("ResetState Enter");
        _elapsedTime = 0;
        _context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        _context.CurrentInterceptingCollider = null;
    }
    public override void ExitState(){}

    public override void UpdateState()
    {
        Debug.Log("ResetState Update");        
        _context.InteractionPointYOffset = Mathf.Lerp(_context.InteractionPointYOffset, _context.ColliderCenterY, _elapsedTime / _lerpDuration);
        
        _context.CurrentIKConstraint.weight = Mathf.Lerp(_context.CurrentIKConstraint.weight, 0, _elapsedTime/_lerpDuration);
        _context.CurrentIKRotationConstraint.weight = Mathf.Lerp(_context.CurrentIKRotationConstraint.weight, 0, _elapsedTime/_lerpDuration);
        
        _context.CurrentIKTargetTransform.localPosition = Vector3.Lerp(_context.CurrentIKTargetTransform.localPosition, _context.CurrentOriginalTargetPosition, _elapsedTime / _lerpDuration);
        _context.CurrentIKTargetTransform.rotation = Quaternion.RotateTowards(_context.CurrentIKTargetTransform.rotation, _context.CurrentOriginalTargetRotation, _rotationSpeed * Time.deltaTime);

        _elapsedTime += Time.deltaTime;
    }
    //restart the whole state machine
    public override EnvInteractionStateMachine.EEnvInteractionState GetNextState()
    {
        
        bool isMoving = _context.Rigidbody.linearVelocity != Vector3.zero;
        if (_elapsedTime > _resetTime)
        {
            return EnvInteractionStateMachine.EEnvInteractionState.Search;
        }

        return StateKey;
    }
    public override void OnTriggerEnter(Collider other){}
    public override void OnTriggerStay(Collider other){}
    public override void OnTriggerExit(Collider other){}
}
