using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;
public class EnvInteractionStateMachine : StateManager<EnvInteractionStateMachine.EEnvInteractionState>
{
    public enum EEnvInteractionState
    {
        Search,
        Approach,
        Rise,
        Touch,
        Reset,
    }

    private EnvInteractionContext Context { get; set; }
    
    [SerializeField] private TwoBoneIKConstraint _leftHandIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightHandIKConstraint;
    [SerializeField] private MultiRotationConstraint _leftHandRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightHandRotationConstraint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _capsuleCollider;


    private void Awake()
    {
        Validate();
        
        Context = new EnvInteractionContext(_leftHandIKConstraint, _rightHandIKConstraint, _leftHandRotationConstraint,  _rightHandRotationConstraint, _rigidbody,  _capsuleCollider, transform.root);
        IntializeStates();
        ConstructEnvDetectionCollider();
    }
    
    //controls if all variables are assigned
    private void Validate()
    {
        Assert.IsNotNull(_leftHandIKConstraint, "_leftHandIKConstraint not set");
        Assert.IsNotNull(_rightHandIKConstraint,  "_rightHandIKConstraint not set");
        Assert.IsNotNull(_leftHandRotationConstraint,  "_leftHandRotationConstraint not set");
        Assert.IsNotNull(_rightHandRotationConstraint,  "_rightHandRotationConstraint not set");
        Assert.IsNotNull(_rigidbody,  "_rigidbody not set");
        Assert.IsNotNull(_capsuleCollider,  "_capsuleCollider  not set");
    }

    private void IntializeStates()
    {
        // add states to inherited StateManager "States" dictionary and set initial state
        states.Add(EEnvInteractionState.Reset, new ResetState(Context, EEnvInteractionState.Reset));
        states.Add(EEnvInteractionState.Approach, new ApproachState(Context, EEnvInteractionState.Approach));
        states.Add(EEnvInteractionState.Rise, new RiseState(Context, EEnvInteractionState.Rise));
        states.Add(EEnvInteractionState.Search, new SearchState(Context, EEnvInteractionState.Search));
        states.Add(EEnvInteractionState.Touch, new TouchState(Context, EEnvInteractionState.Touch));
        currentState = states[EEnvInteractionState.Reset];
    }

    private void ConstructEnvDetectionCollider()
    {
        float wingspan = _capsuleCollider.height;
        
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingspan, wingspan, wingspan);
        boxCollider.center = new Vector3(_capsuleCollider.center.x, _capsuleCollider.center.y + (0.25f * wingspan), _capsuleCollider.center.z + (0.5f * wingspan));
        boxCollider.isTrigger = true;
    }
}
