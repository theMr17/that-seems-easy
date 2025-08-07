using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement LocalInstance { get; private set; }

    [Header("References")]
    public PlayerMovementSo playerMovementSo;
    [SerializeField] private Collider2D _feetCollider;
    [SerializeField] private Collider2D _headCollider;
    private Animator _animator;

    private Rigidbody2D _rigidbody;

    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    public float verticalVelocity { get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;

    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    private float _jumpBufferTime;
    private bool _jumpReleasedDuringBuffer;

    private float _coyoteTimer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _isFacingRight = true;
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();

        _animator.SetFloat("velocityX", Math.Abs(_rigidbody.linearVelocityX));
        _animator.SetFloat("velocityY", _rigidbody.linearVelocityY);


        if (_isGrounded)
        {
            Move(playerMovementSo.groundAcceleration, playerMovementSo.groundDeceleration, InputManager.movement);

        }
        else
        {
            Move(playerMovementSo.airAcceleration, playerMovementSo.airDeceleration, InputManager.movement);
        }
    }

    private void Update()
    {

        CountTimers();
        JumpChecks();
    }

    private void JumpChecks()
    {
        // when we press the jump button
        if (InputManager.jumpWasPressed)
        {
            _jumpBufferTime = playerMovementSo.jumpBufferTime;
            _jumpReleasedDuringBuffer = false;


        }

        // when we release the jump button
        if (InputManager.jumpWasReleased)
        {
            if (_jumpBufferTime > 0f)
            {
                _jumpReleasedDuringBuffer = true;
            }

            if (_isJumping && verticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = playerMovementSo.timeforUpwardsCancel;
                    verticalVelocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = verticalVelocity;
                }
            }
        }

        // initiate jump with jump buffering and coyote time
        if (_jumpBufferTime > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
        {
            InitiateJump(1);

            _isFalling = false;

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = verticalVelocity;
            }

        }
        // double jump
        else if (_jumpBufferTime > 0f && _isJumping && _numberOfJumpsUsed < playerMovementSo.numberOfJumpsAllowed)
        {
            InitiateJump(1);
            _isFastFalling = false;

        }
        // air jump after coyote time lapsed
        else if (_jumpBufferTime > 0f && _isFalling && _numberOfJumpsUsed < playerMovementSo.numberOfJumpsAllowed - 1)
        {
            InitiateJump(2);
            _isFastFalling = false;
        }

        // landed
        if ((_isJumping || _isFalling) && _isGrounded && verticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;

            verticalVelocity = Physics2D.gravity.y;
            _animator.SetBool("isJumping", false);
        }
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if (!_isJumping)
        {
            _isJumping = true;
            _animator.SetBool("isJumping", true);
        }

        _jumpBufferTime = 0f;
        _numberOfJumpsUsed += numberOfJumpsUsed;
        verticalVelocity = playerMovementSo.initialJumpVelocity;
    }

    private void Jump()
    {
        // apply gravity while jumping
        if (_isJumping)
        {
            // check for head bump
            if (_bumpedHead)
            {
                _isFastFalling = true;
            }

            // gravity on ascending
            if (verticalVelocity >= 0f)
            {
                // apex controls
                _apexPoint = Mathf.InverseLerp(playerMovementSo.initialJumpVelocity, 0f, verticalVelocity);

                if (_apexPoint > playerMovementSo.apexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }
                    else
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if (_timePastApexThreshold < playerMovementSo.apexHangTime)
                        {
                            verticalVelocity = 0f;
                        }
                        else
                        {
                            verticalVelocity = -0.01f;
                        }
                    }
                }
                else
                {
                    verticalVelocity += playerMovementSo.gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
            }
            // gravity on descending
            else if (!_isFastFalling)
            {
                verticalVelocity += playerMovementSo.gravity * playerMovementSo.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (verticalVelocity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }

        // jump cut
        if (_isFastFalling)
        {
            if (_fastFallTime >= playerMovementSo.timeforUpwardsCancel)
            {
                verticalVelocity += playerMovementSo.gravity * playerMovementSo.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else
            {
                verticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, _fastFallTime / playerMovementSo.timeforUpwardsCancel);
            }

            _fastFallTime += Time.fixedDeltaTime;
        }

        // normal gravity while falling
        if (!_isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }

            verticalVelocity += playerMovementSo.gravity * Time.fixedDeltaTime;
        }

        // clamp fall speed
        verticalVelocity = Mathf.Clamp(verticalVelocity, -playerMovementSo.maxFallSpeed, 50f);

        if (_isJumping && !_isGrounded && verticalVelocity < 0f)
        {
            _isFalling = true;
        }

        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocityX, verticalVelocity);
    }

    private void CountTimers()
    {
        _jumpBufferTime -= Time.deltaTime;

        if (!_isGrounded)
        {
            _coyoteTimer -= Time.deltaTime;
        }
        else
        {
            _coyoteTimer = playerMovementSo.jumpCoyoteTime;
        }
    }

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TurnChecks(moveInput);

            Vector2 targetVelocity;
            if (InputManager.runIsHeld)
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * playerMovementSo.maxRunSpeed;
            }
            else
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * playerMovementSo.maxWalkSpeed;
            }

            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rigidbody.linearVelocity = new Vector2(_moveVelocity.x, _rigidbody.linearVelocityY);
        }
        else
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rigidbody.linearVelocity = new Vector2(_moveVelocity.x, _rigidbody.linearVelocityY);
        }
    }

    private void TurnChecks(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x < 0f)
        {
            _isFacingRight = false;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (!_isFacingRight && moveInput.x > 0f)
        {
            _isFacingRight = true;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetCollider.bounds.center.x, _feetCollider.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetCollider.bounds.size.x, playerMovementSo.groundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, playerMovementSo.groundDetectionRayLength, playerMovementSo.groundLayer);
        _isGrounded = _groundHit.collider != null;

        if (playerMovementSo.debugShowIsGroundedBox)
        {
            Color rayColor = _isGrounded ? Color.red : Color.green;

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * playerMovementSo.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * playerMovementSo.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - playerMovementSo.groundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
    }

    private void HeadBumpCheck()
    {
        Vector2 boxCastOrigin = new Vector2(_headCollider.bounds.center.x, _headCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_headCollider.bounds.size.x * playerMovementSo.headWidth, playerMovementSo.headDetectionRayLength);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, playerMovementSo.headDetectionRayLength, playerMovementSo.groundLayer);
        _bumpedHead = _headHit.collider != null;

        if (playerMovementSo.debugShowHeadBumpedBox)
        {
            Color rayColor = _bumpedHead ? Color.red : Color.green;

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.up * playerMovementSo.headDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.up * playerMovementSo.headDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y + playerMovementSo.headDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
    }
    private void CollisionChecks()
    {
        IsGrounded();
        HeadBumpCheck();
    }

    private void DrawJumpArc(float moveSpeed, Color gizmoColor)
    {
        Vector2 startPosition = new Vector2(_feetCollider.bounds.center.x, _feetCollider.bounds.min.y);
        Vector2 previousPosition = startPosition;

        Vector2 velocity = new Vector2(moveSpeed, playerMovementSo.initialJumpVelocity);
        Gizmos.color = gizmoColor;

        float timeStep = 2 * playerMovementSo.timeTillJumpApex / playerMovementSo.arcResolution;

        for (int i = 0; i < playerMovementSo.visualizationSteps; i++)
        {
            float simulationTime = i * timeStep;
            Vector2 displacement;
            Vector2 drawPoint;

            if (simulationTime < playerMovementSo.timeTillJumpApex) // Ascending
            {
                displacement = velocity * simulationTime + 0.5f * new Vector2(0, playerMovementSo.gravity) * simulationTime * simulationTime;
            }
            else if (simulationTime < playerMovementSo.timeTillJumpApex + playerMovementSo.apexHangTime) // Apex hang time
            {
                float apexTime = playerMovementSo.timeTillJumpApex + 0.5f * playerMovementSo.apexHangTime;
                displacement = velocity * playerMovementSo.timeTillJumpApex + 0.5f * new Vector2(0, playerMovementSo.gravity) * playerMovementSo.timeTillJumpApex * playerMovementSo.timeTillJumpApex;
                displacement += new Vector2(0, playerMovementSo.apexHangTime) * apexTime;
            }
            else // Descending
            {
                float descendTime = simulationTime - (playerMovementSo.timeTillJumpApex + playerMovementSo.apexHangTime);
                displacement = velocity * playerMovementSo.timeTillJumpApex + 0.5f * new Vector2(0, playerMovementSo.gravity) * playerMovementSo.timeTillJumpApex * playerMovementSo.timeTillJumpApex;
                displacement += new Vector2(moveSpeed, 0) * playerMovementSo.apexHangTime; // Horizontal movement during hang time
                displacement += new Vector2(moveSpeed, 0) * descendTime + 0.5f * new Vector2(0, playerMovementSo.gravity) * descendTime * descendTime;
            }

            drawPoint = startPosition + displacement;

            if (playerMovementSo.stopOnCollision)
            {
                RaycastHit2D hit = Physics2D.Raycast(previousPosition, drawPoint - previousPosition, Vector2.Distance(previousPosition, drawPoint), playerMovementSo.groundLayer);
                if (hit.collider != null)
                {
                    // If a hit is detected, stop drawing the arc at the hit point
                    Gizmos.DrawLine(previousPosition, hit.point);
                    break;
                }
            }

            Gizmos.DrawLine(previousPosition, drawPoint);
            previousPosition = drawPoint;
        }
    }

    private void OnDrawGizmos()
    {
        if (playerMovementSo.showWalkJumpArc)
        {
            DrawJumpArc(playerMovementSo.maxWalkSpeed, Color.white);
        }

        if (playerMovementSo.showRunJumpArc)
        {
            DrawJumpArc(playerMovementSo.maxRunSpeed, Color.red);
        }
    }
}
