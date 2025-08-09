using UnityEngine;

public class Camera : MonoBehaviour
{
  private void Start()
  {
    if (DemoLevelManager.Instance != null)
    {
      GetComponent<UnityEngine.Camera>().backgroundColor = DemoLevelManager.Instance.GetLevelThemeColors().backgroundColor;
    }
  }
}
