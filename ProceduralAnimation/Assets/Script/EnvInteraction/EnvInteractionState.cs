using UnityEngine;

public abstract class EnvInteractionState : BaseState<EnvInteractionStateMachine.EEnvInteractionState>
{
    protected EnvInteractionContext _context;
    public EnvInteractionState(EnvInteractionContext context, EnvInteractionStateMachine.EEnvInteractionState stateKey) : base(stateKey)
    {
        _context = context;
    }
    private Vector3 GetClosestPointOnCollider(Collider interceptingCollider, Vector3 positionToCheck)
    {
        return interceptingCollider.ClosestPoint(positionToCheck);
    }
    protected void StartIkTracking(Collider interceptingCollider)
    {
        if (interceptingCollider.gameObject.layer == LayerMask.NameToLayer("Interactable") && _context.CurrentInterceptingCollider == null)
        {
            _context.CurrentInterceptingCollider = interceptingCollider;
            Vector3 closestPointFromRoot = GetClosestPointOnCollider(interceptingCollider, _context._rootTransform.position);
            _context.SetBodySide(closestPointFromRoot);
            
            SetIKTargetPosition();
        }
    }
    protected void UpdateIkTracking(Collider interceptingCollider)
    {
        if (interceptingCollider == _context.CurrentInterceptingCollider)
        {
            SetIKTargetPosition();
        }
    }

    protected void ResetIkTracking(Collider interceptingCollider)
    {
        if (interceptingCollider == _context.CurrentInterceptingCollider)
        {
            _context.CurrentInterceptingCollider = null;
            _context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        }
    }
    //set the position of the target for the IKConstraint
    private void SetIKTargetPosition()
    {
        _context.ClosestPointOnColliderFromShoulder = GetClosestPointOnCollider(_context.CurrentInterceptingCollider, 
            new Vector3(_context.currentShoulderTransform.position.x, _context.CharacterShouldeHeight, _context.currentShoulderTransform.position.z));
        
        // create an offset for the target 
        Vector3 rayDirection = _context.currentShoulderTransform.position - _context.ClosestPointOnColliderFromShoulder;
        Vector3 normalizedDir =  rayDirection.normalized;
        float offsetDistance = 0.05f;
        Vector3 offset =  normalizedDir * offsetDistance;
        
        Vector3 offsetPosition = _context.ClosestPointOnColliderFromShoulder + offset;
        _context.currentIKTargetTransform.position = offsetPosition;
    }
}
