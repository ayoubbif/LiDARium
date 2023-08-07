using _Game.Scripts.LiDAR;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerInput.OnFootActions _onFootActions;

    private PlayerController _playerController;
    private PlayerLook _playerLook;
    
    // New variable to handle shooting
    private bool _isScanning;
    [SerializeField] private RayGun _rayGun;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _onFootActions = _playerInput.OnFoot;

        _playerController = GetComponent<PlayerController>();
        _playerLook = GetComponent<PlayerLook>();

        _onFootActions.Jump.performed += _ => _playerController.Jump();

        // Add listeners for the shoot action
        _onFootActions.Scan.started += _ => StartShooting();
        _onFootActions.Scan.canceled += _ => StopShooting();
    }

    private void StartShooting()
    {
        _isScanning = true;
        _rayGun.Scanning = true;
    }

    private void StopShooting()
    {
        _isScanning = false;
        _rayGun.Scanning = false;
    }

    private void FixedUpdate()
    {
        _playerController.ProcessMove(_onFootActions.Movement.ReadValue<Vector2>());
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