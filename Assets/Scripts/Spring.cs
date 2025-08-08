using UnityEngine;

public class Spring : MonoBehaviour
{

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

    }
  }
}
