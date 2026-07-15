using UnityEngine;

public class ApproachState : EnvInteractionState
{
   public ApproachState(EnvInteractionContext context, EnvInteractionStateMachine.EEnvInteractionState estate) : base(
      context, estate)
   {
      EnvInteractionContext Context = context;
   }
   public override void EnterState(){}
   public override void ExitState(){}
   public override void UpdateState(){}

   public override EnvInteractionStateMachine.EEnvInteractionState GetNextState()
   {
      return StateKey;
   }
   public override void OnTriggerEnter(Collider other){}
   public override void OnTriggerStay(Collider other){}
   public override void OnTriggerExit(Collider other){}
}

