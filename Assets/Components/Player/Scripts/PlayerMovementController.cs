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
        [SerializeField] private GameObject _characterGameObject;
        
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
            // Read inputs.
            Vector2 movementInput = _playerMoveActionRef.action.ReadValue<Vector2>();
            Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);

            transform.Translate(movement * (_playerSpeed * Time.deltaTime), Space.World);
            
            if (movement != Vector3.zero)
            {
                _animator.SetBool(WALK_PARAMETER, true);
                
                // Rotate character only.
                Quaternion playerRotation = Quaternion.LookRotation(movement, Vector3.up);
                _characterGameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, _playerRotationSpeed);
            }
            else
            {
                _animator.SetBool(WALK_PARAMETER, false);
            }
        }
    }
}