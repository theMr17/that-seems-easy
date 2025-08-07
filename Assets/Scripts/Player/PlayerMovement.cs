using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerMovement Instance { get; private set; }

    private Rigidbody2D _rigidbody2D;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _rayCastDistance = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    private float _horizontalMovement;
    private bool _jumpRequested;
    private bool _isGrounded;

    private void Awake()
    {
        Instance = this;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rigidbody2D.linearVelocityX = _horizontalMovement * _moveSpeed;

        if (_jumpRequested && IsGrounded())
        {
            _rigidbody2D.linearVelocityY = _jumpForce;
        }

        _jumpRequested = false;
    }

    public void MoveHorizontal(InputAction.CallbackContext context)
    {
        _horizontalMovement = context.ReadValue<Vector2>().normalized.x;
    }

    public void InitiateJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            _jumpRequested = true;
    }

    private bool IsGrounded()
    {
        Vector2 origin = transform.position;
        Debug.DrawRay(origin, Vector2.down * _rayCastDistance, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, _rayCastDistance, _groundLayer);
        return hit.collider != null;
    }

}
