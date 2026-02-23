using UnityEngine;
using UnityEngine.InputSystem;

namespace Components.Player
{

    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Input Actions")] 
        [SerializeField] private InputActionReference _playerMoveActionRef;

        [Header("Movement Settings")]
        [SerializeField] private float _playerSpeed = 2f;

        private void OnEnable()
        {
            _playerMoveActionRef.action.Enable();
        }

        private void OnDisable()
        {
            _playerMoveActionRef.action.Disable();
        }

        /// <summary>
        /// Handle player movement.
        /// </summary>
        void Update()
        {
            // Read inputs.
            Vector2 movementInput = _playerMoveActionRef.action.ReadValue<Vector2>();
            Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);

            // Movement = direction + distance by time.
            var currentMovement = Vector3.ClampMagnitude(movement, _playerSpeed * Time.deltaTime);

            if (currentMovement != Vector3.zero)
            {
                transform.Translate(currentMovement, Space.World);
            }
        }
    }
}