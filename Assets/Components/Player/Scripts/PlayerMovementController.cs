using UnityEngine;
using UnityEngine.InputSystem;

namespace Components.Player.Scripts
{

    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Input Actions")] 
        [SerializeField] private InputActionReference _playerMoveActionRef;
        [SerializeField] private InputActionReference _playerRunActionRef;
        
        [Header("Movement Settings")]
        [SerializeField] private float _playerWalkSpeed = 2f;
        [SerializeField] private float _playerRunSpeed = 7f;
        [SerializeField] private float _playerRotationSpeed = 720f;
        private float _playerCurrentSpeed;
        
        [Header("Components")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _characterGameObject;
        [SerializeField] private Transform _playerCamera;
        
        private const string WALK_PARAMETER = "IsWalking";
        private const string RUN_PARAMETER = "IsRunning";

        private void OnEnable()
        {
            _playerMoveActionRef.action.Enable();
            _playerRunActionRef.action.Enable();
        }

        private void OnDisable()
        {
            _playerMoveActionRef.action.Disable();
            _playerRunActionRef.action.Disable();
        }

        private void Start()
        {
            _playerCurrentSpeed = _playerWalkSpeed;
        }

        /// <summary>
        /// Handle player movement and rotation.
        /// </summary>
        void Update()
        {
            // Read movement inputs.
            Vector2 movementInput = _playerMoveActionRef.action.ReadValue<Vector2>();
            Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
            
            // Read camera inputs.
            Vector3 cameraForward = _playerCamera.forward;
            Vector3 cameraRight = _playerCamera.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            
            // Current camera direction.
            Vector3 currentForward = movementInput.y * cameraForward;
            Vector3 currentRight = movementInput.x * cameraRight;
            
            Vector3 currentDirection = currentForward + currentRight;

            transform.Translate(currentDirection * (_playerCurrentSpeed * Time.deltaTime));
            
            if (movement != Vector3.zero)
            {
                _animator.SetBool(WALK_PARAMETER, true);
                
                if (_playerRunActionRef.action.WasPerformedThisFrame())
                {
                    _animator.SetBool(RUN_PARAMETER, true);
                    _playerCurrentSpeed = _playerRunSpeed;
                }
                else if (_playerRunActionRef.action.WasReleasedThisFrame())
                {
                    _animator.SetBool(RUN_PARAMETER, false);
                    _playerCurrentSpeed = _playerWalkSpeed;
                }
                
                // Rotate character only.
                Quaternion playerRotation = Quaternion.LookRotation(currentDirection, Vector3.up);
                _characterGameObject.rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, _playerRotationSpeed);
            }
            else
            {
                _animator.SetBool(WALK_PARAMETER, false);
            }
        }
    }
}