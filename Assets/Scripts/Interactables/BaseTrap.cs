using UnityEngine;

public abstract class BaseTrap : MonoBehaviour
{
  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.TryGetComponent<PlayerMovement>(out var player))
    {
      player.Die();
    }
  }
}
