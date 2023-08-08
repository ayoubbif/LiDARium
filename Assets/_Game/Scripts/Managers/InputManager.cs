using UnityEngine;
using _Game.Scripts.LiDAR;

public class InputManager : Singleton<InputManager>
{
    [Header("Components")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private RayGun rayGun;

    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFootActions;

    protected override void Awake()
    {
        InitializeComponents();
        SetupInputActions();
    }

    private void InitializeComponents()
    {
        playerInput = new PlayerInput();
        
        onFootActions = playerInput.OnFoot;

        if (!playerController) playerController = GetComponent<PlayerController>();
        if (!playerLook) playerLook = GetComponent<PlayerLook>();
        if (!rayGun) rayGun = GetComponent<RayGun>();
    }

    private void SetupInputActions()
    {
        onFootActions.Enable();
        SetupJumpActions();
        SetupShootActions();
        SetupMenuActions();
    }

    private void SetupJumpActions()
    {
        onFootActions.Jump.performed += _ => playerController.Jump();
    }

    private void SetupShootActions()
    {
        onFootActions.Scan.started += _ => ChangeRayGunState(true);
        onFootActions.Scan.canceled += _ => ChangeRayGunState(false);
        onFootActions.Paint.started += _ => ChangeRayGunPaintingState(true);
        onFootActions.Paint.canceled += _ => ChangeRayGunPaintingState(false);
    }

    private void SetupMenuActions()
    {
        onFootActions.Menu.started += _ => GameManager.Instance.TogglePauseMenu();
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
        playerController.ProcessMove(onFootActions.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        playerLook.ProcessLook(onFootActions.Look.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        onFootActions.Disable();
    }
}