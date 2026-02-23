using UnityEngine;
using UnityEngine.InputSystem;

namespace Components.Player.Scripts
{

    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Input Actions")] 
        [SerializeField] private InputActionReference _playerMoveActionRef;

        [Header("Movement Settings")]
        [SerializeField] private float _playerSpeed = 2f;
        [SerializeField] private float _playerRotationSpeed = 720f;
        
        [Header("Components")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _characterGameObject;
        [SerializeField] private Transform _playerCamera;
        
        private const string WALK_PARAMETER = "IsWalking";

        private void OnEnable()
        {
            _playerMoveActionRef.action.Enable();
        }

        private void OnDisable()
        {
            _playerMoveActionRef.action.Disable();
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

            transform.Translate(currentDirection * (_playerSpeed * Time.deltaTime));
            
            if (movement != Vector3.zero)
            {
                _animator.SetBool(WALK_PARAMETER, true);
                
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