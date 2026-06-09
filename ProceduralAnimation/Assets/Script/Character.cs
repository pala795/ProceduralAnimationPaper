using UnityEngine;
using UnityEngine.Serialization;

namespace Script
{
    public class Character : MonoBehaviour
    { 
        private InputSystem_Actions _playerInput;
        private Vector2 _moveInput;
        private bool _isMoving;
        [SerializeField] private float maxMovementSpeed;
        [SerializeField] private Animator _animator;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            _playerInput = new InputSystem_Actions();
            _playerInput.Player.Enable();
            _playerInput.Player.Move.performed +=  ctx => _moveInput = ctx.ReadValue<Vector2>();
            _playerInput.Player.Move.canceled +=  ctx => _moveInput = Vector2.zero;
        }
        
        // Update is called once per frame
        void Update()
        {
            if (_moveInput.magnitude != 0)
            {
                _animator.SetBool("Walking", true);
                _isMoving = true;
                Vector3 targetPosition = new Vector3(transform.position.x + _moveInput.x, transform.position.y, transform.position.z + _moveInput.y);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxMovementSpeed * Time.deltaTime);  
                transform.LookAt(targetPosition, Vector3.up);
            }
            else
            {
                _isMoving = false;
                _animator.SetBool("Walking", false);
            }
        }
    }
}
