using UnityEngine;
using UnityEngine.InputSystem;

namespace Components.Camera.Scripts
{
    public class CameraRotation : MonoBehaviour
    {
        [Header("Rotation Inputs Reference")] 
        [SerializeField] private InputActionReference _cameraRotationActionRef;
        
        [Header("Rotation Settings")]
        [SerializeField] private float _rotationSpeed = 100f;
        [SerializeField] private float _startRotation = 10f;
        private float _xRotation, _yRotation;
        
        [Header("Components")]
        [SerializeField] private Transform _pivotRotation;

        private void OnEnable()
        {
            _cameraRotationActionRef.action.Enable();
        }

        private void OnDisable()
        {
            _cameraRotationActionRef.action.Disable();
        }

        void Update()
        {
            var direction = _cameraRotationActionRef.action.ReadValue<Vector2>();
            
            _xRotation += direction.x * _rotationSpeed * Time.deltaTime;
            _yRotation += direction.y * _rotationSpeed * Time.deltaTime;
            _yRotation = Mathf.Clamp(_yRotation, -20f, 18f);
            
            transform.rotation = Quaternion.Euler(_yRotation + _startRotation, _xRotation, 0);
            _pivotRotation.localRotation = Quaternion.Euler(_yRotation, _xRotation, 0);
        }
    }
}
