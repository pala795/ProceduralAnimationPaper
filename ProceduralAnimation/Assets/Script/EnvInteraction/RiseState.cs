using UnityEngine;

public class RiseState : EnvInteractionState
{
    public RiseState (EnvInteractionContext context, EnvInteractionStateMachine.EEnvInteractionState estate) : base(
        context, estate)
    {
        EnvInteractionContext Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("RiseState Enter");
    }
    public override void ExitState(){}

    public override void UpdateState()
    {
        Debug.Log("RiseState Update");
    }

    public override EnvInteractionStateMachine.EEnvInteractionState GetNextState()
    {
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other){}
    public override void OnTriggerStay(Collider other){}
    public override void OnTriggerExit(Collider other){}
}
