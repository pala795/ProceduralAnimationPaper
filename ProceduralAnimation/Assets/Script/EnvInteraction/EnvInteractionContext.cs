using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvInteractionContext
{
    public enum EBodySide
    {
        Left,
        Right
    }
    private TwoBoneIKConstraint _leftHandIKConstraint;
    private TwoBoneIKConstraint _rightHandIKConstraint;
    private MultiRotationConstraint _leftHandRotationConstraint;
    private MultiRotationConstraint _rightHandRotationConstraint;
    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    public Transform _rootTransform;
    //constructor
    public EnvInteractionContext(TwoBoneIKConstraint leftHandIKConstraint, TwoBoneIKConstraint rightHandIKConstraint,
        MultiRotationConstraint leftHandRotationConstraint, MultiRotationConstraint rightHandRotationConstraint, Rigidbody rigidbody, CapsuleCollider capsuleCollider, Transform rootTransform)
    {
        _leftHandIKConstraint = leftHandIKConstraint;
        _rightHandIKConstraint = rightHandIKConstraint;
        _leftHandRotationConstraint = leftHandRotationConstraint;
        _rightHandRotationConstraint = rightHandRotationConstraint;
        _rigidbody = rigidbody;
        _capsuleCollider = capsuleCollider;
        _rootTransform = rootTransform;
    }
    //readonly variables
    public TwoBoneIKConstraint LeftHandIKConstraint => _leftHandIKConstraint;
    public TwoBoneIKConstraint RightHandIKConstraint => _rightHandIKConstraint;
    public MultiRotationConstraint LeftHandRotationConstraint => _leftHandRotationConstraint;
    public MultiRotationConstraint RightHandRotationConstraint => _rightHandRotationConstraint;
    public Rigidbody Rigidbody => _rigidbody;
    public CapsuleCollider CapsuleCollider => _capsuleCollider;
    public Transform RootTransform => _rootTransform;
    
    
    public TwoBoneIKConstraint currentIKConstraint{get; private set;}
    public MultiRotationConstraint currentIKRotationConstraint{get; private set;}
    public Transform currentIKTargetTransform{get; private set;}
    public Transform currentShoulderTransform{get; private set;}
    public EBodySide currentBodySide{get; private set;}
    
    public void SetBodySide(Vector3 positionToCheck)
    {
        Vector3 leftShoulderPosition = _leftHandIKConstraint.data.root.transform.position;
        Vector3 rightShoulderPosition = _rightHandIKConstraint.data.root.transform.position;
        
        bool isLeftCloser = Vector3.Distance(positionToCheck, leftShoulderPosition) < Vector3.Distance(positionToCheck, rightShoulderPosition);
        if (isLeftCloser)
        {
            currentBodySide = EBodySide.Left;
            currentIKConstraint = _leftHandIKConstraint;
            currentIKRotationConstraint = _leftHandRotationConstraint;
        }
        else
        {
            currentBodySide = EBodySide.Right;
            currentIKConstraint = _rightHandIKConstraint;
            currentIKRotationConstraint = _rightHandRotationConstraint;
        }
        currentShoulderTransform = currentIKConstraint.data.root.transform;
        currentIKTargetTransform = currentIKConstraint.data.root.transform;
    }
}
