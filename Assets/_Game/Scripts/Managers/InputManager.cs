using _Game.Scripts.LiDAR;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [Header("Components")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerLook _playerLook;
    [SerializeField] private RayGun _rayGun;

    private PlayerInput _playerInput;
    private PlayerInput.OnFootActions _onFootActions;

    protected override void Awake()
    {
        InitializeComponents();
        SetupInputActions();
    }

    private void InitializeComponents()
    {
        _playerInput = new PlayerInput();
        
        _onFootActions = _playerInput.OnFoot;

        if (!_playerController) _playerController = GetComponent<PlayerController>();
        if (!_playerLook) _playerLook = GetComponent<PlayerLook>();
        if (!_rayGun) _rayGun = GetComponent<RayGun>();
    }

    private void SetupInputActions()
    {
        _onFootActions.Enable();
        
        // Jump
        _onFootActions.Jump.performed += _ =>
        {
            _playerController.Jump();
        };

        // Shooting actions
        _onFootActions.Scan.started += _ => ChangeRayGunState(true);
        _onFootActions.Scan.canceled += _ => ChangeRayGunState(false);
        _onFootActions.Paint.started += _ => ChangeRayGunPaintingState(true);
        _onFootActions.Paint.canceled += _ => ChangeRayGunPaintingState(false);
        
        // Pause Menu
        _onFootActions.Menu.started += _ =>
        {
            GameManager.Instance.TogglePauseMenu();
        };
    }

    private void ChangeRayGunState(bool scanning)
    {
        _rayGun.Scanning = scanning;
    }

    private void ChangeRayGunPaintingState(bool painting)
    {
        _rayGun.Painting = painting;
    }

    private void FixedUpdate()
    {
        _playerController.ProcessMove(_onFootActions.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        _playerLook.ProcessLook(_onFootActions.Look.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        _onFootActions.Disable();
    }
}