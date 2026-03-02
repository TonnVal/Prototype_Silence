using UnityEngine;
using UnityEngine.InputSystem;

namespace Components.UIMenuController.Scripts
{
    public class UIMenuController : MonoBehaviour
    {
        [Header("Menu Components")]
        [SerializeField] private InputActionAsset _inputActions;
        [SerializeField] private GameObject _menu;

        private InputAction _playerMenuActionRef;
        private InputAction _uiMenuActionRef;

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
            _playerMenuActionRef = InputSystem.actions.FindAction("Player/Open Menu");
            _uiMenuActionRef = InputSystem.actions.FindAction("UI/Open Menu");
        }

        private void Update()
        {
            OpenMenu();
        }

        private void OpenMenu()
        {
            if (_playerMenuActionRef.WasPressedThisFrame())
            {
                _menu.SetActive(true);
                _inputActions.FindActionMap("Player").Disable();
                _inputActions.FindActionMap("UI").Enable();
            }
            else if (_uiMenuActionRef.WasPressedThisFrame())
            {
                _menu.SetActive(false);
                _inputActions.FindActionMap("UI").Disable();
                _inputActions.FindActionMap("Player").Enable();
            }
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
