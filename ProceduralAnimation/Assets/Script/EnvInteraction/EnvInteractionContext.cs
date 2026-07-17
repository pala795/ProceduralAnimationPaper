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
    private Vector3 _leftOriginalTargetPosition;
    private Vector3 _rightOriginalTargetPosition;

    public Transform _rootTransform;

    //constructor
    public EnvInteractionContext(TwoBoneIKConstraint leftHandIKConstraint, TwoBoneIKConstraint rightHandIKConstraint,
        MultiRotationConstraint leftHandRotationConstraint, MultiRotationConstraint rightHandRotationConstraint,
        Rigidbody rigidbody, CapsuleCollider capsuleCollider, Transform rootTransform)
    {
        _leftHandIKConstraint = leftHandIKConstraint;
        _rightHandIKConstraint = rightHandIKConstraint;
        _leftHandRotationConstraint = leftHandRotationConstraint;
        _rightHandRotationConstraint = rightHandRotationConstraint;
        _rigidbody = rigidbody;
        _capsuleCollider = capsuleCollider;
        _rootTransform = rootTransform;
        _leftOriginalTargetPosition = _leftHandIKConstraint.data.target.transform.localPosition;
        _rightOriginalTargetPosition = _rightHandIKConstraint.data.target.transform.localPosition;
        CurrentOriginalTargetRotation = leftHandIKConstraint.data.target.transform.rotation;
        CharacterShouldeHeight = LeftHandIKConstraint.data.root.position.y;
        SetBodySide(Vector3.positiveInfinity);
    }

    //readonly variables
    public TwoBoneIKConstraint LeftHandIKConstraint => _leftHandIKConstraint;
    public TwoBoneIKConstraint RightHandIKConstraint => _rightHandIKConstraint;
    public MultiRotationConstraint LeftHandRotationConstraint => _leftHandRotationConstraint;
    public MultiRotationConstraint RightHandRotationConstraint => _rightHandRotationConstraint;
    public Rigidbody Rigidbody => _rigidbody;
    public CapsuleCollider CapsuleCollider => _capsuleCollider;
    public Transform RootTransform => _rootTransform;

    public float CharacterShouldeHeight { get; set; }
    public Collider CurrentInterceptingCollider { get; set; }
    public TwoBoneIKConstraint CurrentIKConstraint { get; private set; }
    public MultiRotationConstraint CurrentIKRotationConstraint { get; private set; }
    public Transform CurrentIKTargetTransform { get; private set; }
    public Transform CurrentShoulderTransform { get; private set; }
    public EBodySide CurrentBodySide { get; private set; }
    public Vector3 ClosestPointOnColliderFromShoulder { get; set; } = Vector3.positiveInfinity;
    public float InteractionPointYOffset { get; set; } = 0;
    public float ColliderCenterY { get; set; }
    public Vector3 CurrentOriginalTargetPosition { get; private set; }
    public Quaternion CurrentOriginalTargetRotation { get; private set; }
    public float LowestDistance { get; set; } = Mathf.Infinity;

public void SetBodySide(Vector3 positionToCheck)
    {
        Vector3 leftShoulderPosition = _leftHandIKConstraint.data.root.transform.position;
        Vector3 rightShoulderPosition = _rightHandIKConstraint.data.root.transform.position;
        
        bool isLeftCloser = Vector3.Distance(positionToCheck, leftShoulderPosition) < Vector3.Distance(positionToCheck, rightShoulderPosition);
        if (isLeftCloser)
        {
            CurrentBodySide = EBodySide.Left;
            CurrentIKConstraint = _leftHandIKConstraint;
            CurrentIKRotationConstraint = _leftHandRotationConstraint;
            CurrentOriginalTargetPosition = _leftOriginalTargetPosition;
        }
        else
        {
            CurrentBodySide = EBodySide.Right;
            CurrentIKConstraint = _rightHandIKConstraint;
            CurrentIKRotationConstraint = _rightHandRotationConstraint;
            CurrentOriginalTargetPosition = _rightOriginalTargetPosition;
        }
        CurrentShoulderTransform = CurrentIKConstraint.data.root.transform;
        CurrentIKTargetTransform = CurrentIKConstraint.data.target.transform;
    }
}
