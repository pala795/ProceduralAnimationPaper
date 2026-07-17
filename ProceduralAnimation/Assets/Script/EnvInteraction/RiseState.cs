using UnityEngine;

public class RiseState : EnvInteractionState
{
    float _elapsedTime = 0f;
    float _lerpDuration = 5f;
    float _riseRotationWeight = 1f;
    float _riseWeight = 1f;
    Quaternion _expectedRotation;
    float _maxDistance = 5f;
    float _rotationSpeed = 1000f;
    float _touchDistanceTreshold = 0.5f;
    float _touchTimeTreshold = 1f;
    protected LayerMask InteractableLayerMask = LayerMask.GetMask("Interactable");
    public RiseState (EnvInteractionContext context, EnvInteractionStateMachine.EEnvInteractionState estate) : base(
        context, estate)
    {
        EnvInteractionContext Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("RiseState Enter");
        _elapsedTime = 0f;
    }
    public override void ExitState(){}

    public override void UpdateState()
    {
        Debug.Log("RiseState Update");
        CalculateExpectedRotation();
        _context.InteractionPointYOffset = Mathf.Lerp(_context.InteractionPointYOffset,
            _context.ClosestPointOnColliderFromShoulder.y, _elapsedTime / _lerpDuration);
        _context.CurrentIKConstraint.weight = Mathf.Lerp(_context.CurrentIKConstraint.weight, _riseWeight, _elapsedTime/_lerpDuration);
        _context.CurrentIKRotationConstraint.weight = Mathf.Lerp(_context.CurrentIKRotationConstraint.weight, _riseRotationWeight, _elapsedTime/_lerpDuration);
        _context.CurrentIKTargetTransform.rotation = Quaternion.RotateTowards(_context.CurrentIKTargetTransform.rotation, _expectedRotation, _rotationSpeed * Time.deltaTime);
        _elapsedTime += Time.deltaTime;
    }

    private void CalculateExpectedRotation()
    {
        Vector3 startPos = _context.CurrentShoulderTransform.position;
        Vector3 endPos = _context.ClosestPointOnColliderFromShoulder;
        Vector3 direction = (endPos - startPos).normalized;

        RaycastHit hit;
        if (Physics.Raycast(startPos, direction,out hit, _maxDistance, InteractableLayerMask))
        {
            Vector3 surfaceNormal = hit.normal;
            Vector3 targetForward = - surfaceNormal;
            _expectedRotation = Quaternion.LookRotation(targetForward, Vector3.up);
        }
    }
    public override EnvInteractionStateMachine.EEnvInteractionState GetNextState()
    {
        if (CheckShouldReset())
        {
            return EnvInteractionStateMachine.EEnvInteractionState.Reset; 
        }
        if (Vector3.Distance(_context.CurrentIKTargetTransform.position, _context.ClosestPointOnColliderFromShoulder) <
            _touchDistanceTreshold && _elapsedTime >= _touchTimeTreshold)
        {
            return EnvInteractionStateMachine.EEnvInteractionState.Touch;
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
