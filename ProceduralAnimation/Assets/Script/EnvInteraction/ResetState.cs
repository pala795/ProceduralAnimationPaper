using UnityEngine;

public class ResetState: EnvInteractionState
{
    public ResetState(EnvInteractionContext context, EnvInteractionStateMachine.EEnvInteractionState estate) : base(context , estate)
    {
        EnvInteractionContext _context = context;    
    }

    public override void EnterState()
    {
        Debug.Log("ResetState Enter");
    }
    public override void ExitState(){}

    public override void UpdateState()
    {
        Debug.Log("ResetState Update");
    }

    public override EnvInteractionStateMachine.EEnvInteractionState GetNextState()
    {
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other){}
    public override void OnTriggerStay(Collider other){}
    public override void OnTriggerExit(Collider other){}
}
