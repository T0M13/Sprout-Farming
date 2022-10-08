using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    public static CharacterController Instance;
    [Header("References")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject _spriteObj;
    [SerializeField] private Animator _animator;
    [Header("Movement")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private float speed;
    [SerializeField] private bool isMoving;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private bool isRunning;
    [SerializeField] private float runSpeed = 4.5f;
    [Header("Input Settings")]
    [SerializeField] private InputActionReference _playerMovement;
    [SerializeField] private InputActionReference _playerRun;
    [Header("Animations")]
    [SerializeField] private static string A_Horizontal = "Horizontal";
    [SerializeField] private static string A_LastHorizontal = "LastHorizontal";
    [SerializeField] private static string A_Vertical = "Vertical";
    [SerializeField] private static string A_LastVertical = "LastVertical";
    [SerializeField] private static string A_Speed = "Speed";
    [SerializeField] private static string A_IsRunning = "isRunning";

    public bool CanMove { get => canMove; set => canMove = value; }

    #region Input
    private void OnEnable()
    {
        _playerMovement.action.Enable();
        _playerRun.action.Enable();
    }

    private void OnDisable()
    {
        _playerMovement.action.Disable();
        _playerRun.action.Disable();
    }
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != null)
        {
            Destroy(this);
        }

        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = _spriteObj.GetComponent<Animator>();

        canMove = true;
    }

    private void Update()
    {
        if (!canMove) return;
        ReadInput();
        AnimateCharacter();
        HandleSpeed();
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        MoveCharacter();
    }

    private void ReadInput()
    {
        moveInput = _playerMovement.action.ReadValue<Vector2>();
        isMoving = _playerMovement.action.IsPressed();
        isRunning = _playerRun.action.IsPressed() && isMoving;
    }

    private void MoveCharacter()
    {
        _rigidbody.MovePosition(_rigidbody.position + moveInput * speed * Time.deltaTime);
    }

    private void AnimateCharacter()
    {
        _animator.SetFloat(A_Horizontal, moveInput.x);
        _animator.SetFloat(A_Vertical, moveInput.y);
        _animator.SetFloat(A_Speed, moveInput.sqrMagnitude);
        _animator.SetBool(A_IsRunning, isRunning);

        if (moveInput.x >= 1 || moveInput.x <= -1 || moveInput.y >= 1 || moveInput.y <= -1)
        {
            _animator.SetFloat(A_LastHorizontal, moveInput.x);
            _animator.SetFloat(A_LastVertical, moveInput.y);
        }
    }

    private void HandleSpeed()
    {
        if (isMoving)
        {
            speed = moveSpeed;
        }
        if (isMoving && isRunning)
        {
            speed = runSpeed;
        }
    }
}
