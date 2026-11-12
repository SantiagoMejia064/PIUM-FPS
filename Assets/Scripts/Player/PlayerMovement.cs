using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 12f;

    [Header("Camara")]
    public Transform cameraPivot;
    public float mouseSensitivity = 0.1f;
    public float gamepadLookSpeed = 120f;
    public float minCameraPitch = -35f;
    public float maxCameraPitch = 70f;

    [SerializeField] private Animator movementAnimator;

    [Header("Salto y gravedad")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");

    private CharacterController characterController;
    private PlayerInput playerInput;
    private Transform playerCamera;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    private float verticalVelocity;
    private float cameraPitch;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        Camera[] playerCameras = GetComponentsInChildren<Camera>(true);

        for (int i = 0; i < playerCameras.Length; i++)
        {
            if (!playerCameras[i].name.Contains("Weapon"))
            {
                playerCamera = playerCameras[i].transform;
                break;
            }
        }

        if (cameraPivot == null && playerCamera != null)
        {
            cameraPivot = playerCamera;
        }

        if (playerInput.actions == null)
        {
            Debug.LogError("PlayerInput no tiene asignado NIS: " + name);
            enabled = false;
            return;
        }

        moveAction = playerInput.actions.FindAction("Move", false);
        lookAction = playerInput.actions.FindAction("Look", false);
        jumpAction = playerInput.actions.FindAction("Jump", false);
        sprintAction = playerInput.actions.FindAction("Sprint", false);

        if (moveAction == null || lookAction == null || jumpAction == null || sprintAction == null)
        {
            Debug.LogError("Falta Move, Look, Jump o Sprint en NIS: " + name);
            enabled = false;
            return;
        }

        if (cameraPivot != null)
        {
            cameraPitch = cameraPivot.localEulerAngles.x;

            if (cameraPitch > 180f)
            {
                cameraPitch -= 360f;
            }
        }
    }

    private void Update()
    {
        LookAround();
        MovePlayer();
    }

    private void LookAround()
    {
        if (playerCamera == null || cameraPivot == null)
        {
            return;
        }

        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        float lookX;
        float lookY;

        if (lookAction.activeControl != null && lookAction.activeControl.device is Mouse)
        {
            lookX = lookInput.x * mouseSensitivity;
            lookY = lookInput.y * mouseSensitivity;
        }
        else
        {
            lookX = lookInput.x * gamepadLookSpeed * Time.deltaTime;
            lookY = lookInput.y * gamepadLookSpeed * Time.deltaTime;
        }

        transform.Rotate(0f, lookX, 0f);

        cameraPitch -= lookY;
        cameraPitch = Mathf.Clamp(cameraPitch, minCameraPitch, maxCameraPitch);

        cameraPivot.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    private void MovePlayer()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        if (movementAnimator != null)
        {
            movementAnimator.SetFloat(Horizontal, moveInput.x);
            movementAnimator.SetFloat(Vertical, moveInput.y);
        }

        bool jumpPressed = jumpAction.WasPressedThisFrame();
        bool sprinting = sprintAction.IsPressed();

        Vector3 cameraForward = transform.forward;
        Vector3 cameraRight = transform.right;

        if (playerCamera != null)
        {
            cameraForward = playerCamera.forward;
            cameraRight = playerCamera.right;
        }

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * moveInput.y;
        moveDirection += cameraRight * moveInput.x;

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        if (moveInput.y > 0.1f && moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        if (jumpPressed && characterController.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        float currentSpeed = moveSpeed;

        if (sprinting)
        {
            currentSpeed = sprintSpeed;
        }

        Vector3 velocity = moveDirection * currentSpeed;
        velocity.y = verticalVelocity;

        characterController.Move(velocity * Time.deltaTime);
    }
}
