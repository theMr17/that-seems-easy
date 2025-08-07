using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
  public static PlayerInput playerInput;

  public static Vector2 movement;
  public static bool jumpWasPressed;
  public static bool jumpIsHeld;
  public static bool jumpWasReleased;
  public static bool runIsHeld;

  private InputAction _moveAction;
  private InputAction _jumpAction;

  void Awake()
  {
    playerInput = GetComponent<PlayerInput>();

    _moveAction = playerInput.actions["Move"];
    _jumpAction = playerInput.actions["Jump"];
  }

  void Update()
  {
    movement = _moveAction.ReadValue<Vector2>();

    jumpWasPressed = _jumpAction.WasPressedThisFrame();
    jumpIsHeld = _jumpAction.IsPressed();
    jumpWasReleased = _jumpAction.WasReleasedThisFrame();

  }
}
