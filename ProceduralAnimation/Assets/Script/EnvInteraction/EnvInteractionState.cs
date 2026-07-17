using UnityEngine;

public abstract class EnvInteractionState : BaseState<EnvInteractionStateMachine.EEnvInteractionState>
{
    protected EnvInteractionContext _context;
    float _movingAwayOffset = 0.05f; 
    bool _shouldReset;
    public EnvInteractionState(EnvInteractionContext context, EnvInteractionStateMachine.EEnvInteractionState stateKey) : base(stateKey)
    {
        _context = context;
    }

    protected bool CheckShouldReset()
    {
        if (_shouldReset)
        {
            _context.LowestDistance = Mathf.Infinity;
            _shouldReset = false;
            return true;
        }
        //bool isStoppedd = _context.Rigidbody.velocity.sqrMagnitude == 0.0f;
        bool isMovingAway = CheckIsMovingAway();
        bool isBadAngle = CheckIsBadAngle();
        
        if (isMovingAway || isBadAngle ) // !isStoppedd
        {
            _context.LowestDistance = Mathf.Infinity;
            return true;
        }
        return false;
    }

    protected bool CheckIsBadAngle()
    {
        if (_context.CurrentInterceptingCollider == null)
        {
            return false;
        }
        
        Vector3 targetDir = _context.ClosestPointOnColliderFromShoulder - _context.CurrentShoulderTransform.position;
        Vector3 shoulderDirection = _context.CurrentBodySide == EnvInteractionContext.EBodySide.Right ? _context._rootTransform.right : -_context._rootTransform.right;
        
        float dotProduct = Vector3.Dot(shoulderDirection, targetDir.normalized);
        bool isBadAngle = dotProduct < 0f;
        return isBadAngle;
    }
    protected bool CheckIsMovingAway()
    {
        float currentDistanceToTarget = Vector3.Distance(_context._rootTransform.position, _context.ClosestPointOnColliderFromShoulder);
        
        bool isSeearchingForNewInteraction = _context.CurrentInterceptingCollider == null;
        if (isSeearchingForNewInteraction)
        {
            return false;
        }
        bool isGettingCloserToTarget = currentDistanceToTarget <= _context.LowestDistance;
        if (isGettingCloserToTarget)
        {
            _context.LowestDistance = currentDistanceToTarget;
            return false;
        }
        bool isMovingAway = currentDistanceToTarget > _context.LowestDistance + _movingAwayOffset;
        if (isMovingAway)
        {
            _context.LowestDistance = Mathf.Infinity;
            return true;
        }
        return false;
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
            _shouldReset = true;
        }
    }
    
    //set the position of the target for the IKConstraint
    private void SetIKTargetPosition()
    {
        _context.ClosestPointOnColliderFromShoulder = GetClosestPointOnCollider(_context.CurrentInterceptingCollider, 
            new Vector3(_context.CurrentShoulderTransform.position.x, _context.CharacterShouldeHeight, _context.CurrentShoulderTransform.position.z));
        
        // create an offset for the target 
        Vector3 rayDirection = _context.CurrentShoulderTransform.position - _context.ClosestPointOnColliderFromShoulder;
        Vector3 normalizedDir =  rayDirection.normalized;
        float offsetDistance = 0.05f;
        Vector3 offset =  normalizedDir * offsetDistance;
        
        Vector3 offsetPosition = _context.ClosestPointOnColliderFromShoulder + offset;
        _context.CurrentIKTargetTransform.position = new Vector3(offsetPosition.x, _context.InteractionPointYOffset, offsetPosition.z);
    }
}
