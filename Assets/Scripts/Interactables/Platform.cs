using UnityEngine;

public class Platform : MonoBehaviour
{
  private void Start()
  {
    if (DemoLevelManager.Instance != null)
    {
      GetComponent<SpriteRenderer>().color = DemoLevelManager.Instance.GetLevelThemeColors().tileColor;
    }
  }
}
