using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerInput.OnFootActions _onFootActions;

    private PlayerController _playerController;
    private PlayerLook _playerLook;
    
    // New variable to handle shooting
    private bool _isScanning;
    //[SerializeField] private RayGun _rayGun;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _onFootActions = _playerInput.OnFoot;

        _playerController = GetComponent<PlayerController>();
        _playerLook = GetComponent<PlayerLook>();

        _onFootActions.Jump.performed += _ => _playerController.Jump();
    }
    
    private void FixedUpdate()
    {
        _playerController.ProcessMove(_onFootActions.Movement.ReadValue<Vector2>());

        if (_isScanning)
        {
            // Add continuous shooting logic here (if needed)
        }
    }

    private void LateUpdate()
    {
        _playerLook.ProcessLook(_onFootActions.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        _onFootActions.Enable();
    }

    private void OnDisable()
    {
        _onFootActions.Disable();
    }
}