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
    private void StartIkTracking(Collider interceptingCollider)
    {
        Vector3 closestPointFromRoot = GetClosestPointOnCollider(interceptingCollider, _context._rootTransform.position);
        _context.SetBodySide(closestPointFromRoot);
        
    }
    private void UpdateIkTracking(Collider interceptingCollider)
    {
        Debug.Log("UpdateIkTracking");
    }

    private void ResetIkTracking(Collider interceptingCollider)
    {
        Debug.Log("ResetIkTracking");
    }
}
