using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Components.Player.Scripts
{

    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _playerWalkSpeed = 2f;
        [SerializeField] private float _playerSprintSpeed = 7f;
        [SerializeField] private float _playerRotationSpeed = 720f;
        private float _playerCurrentSpeed;
        
        [Header("Jump Settings")]
        [SerializeField] private float _jumpTimer;
        [SerializeField] private float _jumpDuration = 1f;
        [SerializeField] private float _jumpHeight = 1.5f;
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private AnimationCurve _fallCurve;
        
        [Header("Components")]
        [SerializeField] private InputActionAsset _inputActions;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _characterGameObject;
        [SerializeField] private Transform _playerCamera;
        
        [Header("Debug")]
        [SerializeField] private bool _isRunning;
        [SerializeField] private bool _isJumping;

        private InputAction _playerMoveActionRef;
        private InputAction _playerSprintActionRef;
        private InputAction _playerJumpActionRef;
        
        private const string WALK_PARAMETER = "IsWalking";
        private const string SPRINT_PARAMETER = "IsSprinting";
        private const string JUMP_PARAMETER = "IsJumping";

        private void OnEnable()
        {
            _inputActions.FindActionMap("Player").Enable();
        }

        private void OnDisable()
        {
            _inputActions.FindActionMap("Player").Disable();
        }

        private void Awake()
        {
            _playerMoveActionRef = InputSystem.actions.FindAction("Move");
            _playerSprintActionRef = InputSystem.actions.FindAction("Sprint");
            _playerJumpActionRef = InputSystem.actions.FindAction("Jump");
        }

        private void Start()
        {
            _playerCurrentSpeed = _playerWalkSpeed;
            _isRunning = false;
        }
        
        void Update()
        {
            HandlePlayerMove();
            
            if (_playerJumpActionRef.WasPerformedThisFrame() && !_isJumping)
            {
                HandlePlayerJump();
            }
        }

        /// <summary>
        /// Handle player movement and rotation.
        /// </summary>
        private void HandlePlayerMove()
        {
            // Read movement inputs.
            Vector2 movementInput = _playerMoveActionRef.ReadValue<Vector2>();
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
                
                if (_playerSprintActionRef.WasPressedThisFrame())
                {
                    _animator.SetBool(SPRINT_PARAMETER, true);
                    _playerCurrentSpeed = _playerSprintSpeed;
                }
                else if (_playerSprintActionRef.WasReleasedThisFrame())
                {
                    _animator.SetBool(SPRINT_PARAMETER, false);
                    _playerCurrentSpeed = _playerWalkSpeed;
                }
                
                // Rotate character only.
                Quaternion playerRotation = Quaternion.LookRotation(currentDirection, Vector3.up);
                _characterGameObject.rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, _playerRotationSpeed);
            }
            else
            {
                _animator.SetBool(WALK_PARAMETER, false);
                _animator.SetBool(SPRINT_PARAMETER, false);
            }
        }

        private void HandlePlayerJump()
        {
            if (_isJumping)
            {
                return;
            }

            StartCoroutine(Coroutine_PlayerJump());
        }

        private IEnumerator Coroutine_PlayerJump()
        {
            _isJumping = true;
            _animator.SetBool(JUMP_PARAMETER, true);
            
            float jumpHalfDuration = _jumpDuration / 2;
            _jumpTimer = 0f;
            
            // Jump logic
            while (_jumpTimer < jumpHalfDuration)
            {
                _jumpTimer += Time.deltaTime;
                var normalizedTime = Mathf.Clamp01(_jumpTimer / jumpHalfDuration);
    
                var targetHeight = _jumpCurve.Evaluate(normalizedTime) * _jumpHeight;
    
                var targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
                transform.position = targetPosition;
    
                // Wait for the next frame.
                yield return null;
            }

            // Fall logic
            _jumpTimer = 0f;

            while (_jumpTimer < jumpHalfDuration)
            {
                _jumpTimer += Time.deltaTime;
                var normalizedTime = Mathf.Clamp01(_jumpTimer / jumpHalfDuration);
    
                var targetHeight = _fallCurve.Evaluate(normalizedTime) * _jumpHeight;
    
                var targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
                transform.position = targetPosition;
    
                yield return null;
            }
            
            _animator.SetBool(JUMP_PARAMETER, false);
            _isJumping = false;
        }
    }
}