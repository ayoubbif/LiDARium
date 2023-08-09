using _Game.Scripts.LiDAR;
using UnityEngine;
using UnityEngine.Serialization;

public class InputManager : Singleton<InputManager>
{
    [Header("Components")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private RayGun rayGun;
    [SerializeField] private PointRenderer pointRenderer;

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

        if (!playerController) playerController = GetComponent<PlayerController>();
        if (!playerLook) playerLook = GetComponent<PlayerLook>();
        if (!rayGun) rayGun = GetComponent<RayGun>();
    }

    private void SetupInputActions()
    {
        _onFootActions.Enable();
        
        // Jump
        _onFootActions.Jump.performed += _ =>
        {
            playerController.Jump();
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
        
        // Clear Points
        _onFootActions.Clear.performed += _ =>
        {
            pointRenderer.ClearAllPoints();
        };
    }

    private void ChangeRayGunState(bool scanning)
    {
        rayGun.Scanning = scanning;
    }

    private void ChangeRayGunPaintingState(bool painting)
    {
        rayGun.Painting = painting;
    }

    private void FixedUpdate()
    {
        playerController.ProcessMove(_onFootActions.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        playerLook.ProcessLook(_onFootActions.Look.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        _onFootActions.Disable();
    }
}