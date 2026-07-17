using UnityEngine;

public class TouchState : EnvInteractionState
{
    float _elapsedTime = 0f ;
    float _resetTreshold = 5f;
    public TouchState (EnvInteractionContext context, EnvInteractionStateMachine.EEnvInteractionState estate) : base(
        context, estate)
    {
        EnvInteractionContext Context = context;
    }

    public override void EnterState()
    {
        _elapsedTime = 0f;
    }
    public override void ExitState(){}

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
    }

    public override EnvInteractionStateMachine.EEnvInteractionState GetNextState()
    {
        if (_elapsedTime > _resetTreshold || CheckShouldReset())
        {
            return EnvInteractionStateMachine.EEnvInteractionState.Reset;
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
