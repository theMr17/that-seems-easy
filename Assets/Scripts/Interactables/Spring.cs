using UnityEngine;

public class Spring : MonoBehaviour
{
  [SerializeField] private float initialSpringJumpVelocity = 25f;

  private void Start()
  {
    if (DemoLevelManager.Instance != null)
    {
      GetComponent<SpriteRenderer>().color = DemoLevelManager.Instance.GetLevelThemeColors().tileColor;
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Player"))
    {
      // Apply jump force to the player
      if (collision.gameObject.TryGetComponent<PlayerMovement>(out var player))
      {
        GetComponent<Animator>()?.SetTrigger("SpringJump");
        player.BounceFromSpring(initialSpringJumpVelocity);
      }

      // // Optional: Trigger spring animation
      // if (_animator != null)
      // {
      //   _animator.SetTrigger(springAnimationParameter);
      // }
    }
  }
}
