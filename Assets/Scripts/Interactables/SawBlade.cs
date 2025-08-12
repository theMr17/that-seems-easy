using UnityEngine;

public class SawBlade : BaseTrap
{
  private void Start()
  {
    if (DemoLevelManager.Instance != null)
    {
      GetComponent<SpriteRenderer>().color = DemoLevelManager.Instance.GetLevelThemeColors().tileColor;
    }
  }
}
