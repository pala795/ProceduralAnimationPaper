using UnityEngine;

public class SearchState : EnvInteractionState
{
    public float approachDistanceTreshold = 2f;
    public SearchState (EnvInteractionContext context, EnvInteractionStateMachine.EEnvInteractionState estate) : base(
        context, estate)
    {
        EnvInteractionContext Context = context;
    }
    public override void EnterState(){}
    public override void ExitState(){}
    public override void UpdateState(){}

    //Controls if the point on the interactable object is close enough then make the transition to approach state
    public override EnvInteractionStateMachine.EEnvInteractionState GetNextState()
    {
        if (CheckShouldReset())
        {
            return EnvInteractionStateMachine.EEnvInteractionState.Reset; 
        }
        bool isCloseeToTarget = Vector3.Distance(_context.ClosestPointOnColliderFromShoulder,
            _context._rootTransform.position) < approachDistanceTreshold;
        bool isClosestPointOnColliderValid = _context.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;
        if (isCloseeToTarget && isClosestPointOnColliderValid)
        {
            return EnvInteractionStateMachine.EEnvInteractionState.Approach;
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
