using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelIndicatorUI : MonoBehaviour
{
  [Header("UI Prefabs & References")]
  public GameObject circlePrefab;
  public Transform circleParent;

  [Header("Sprites")]
  public Sprite filledCircleSprite;
  public Sprite hollowCircleSprite;

  private List<Image> circles = new List<Image>();

  private void OnEnable()
  {
    UpdateUI();
  }

  public void UpdateUI()
  {
    // Clear old circles
    foreach (Transform child in circleParent)
    {
      Destroy(child.gameObject);
    }
    circles.Clear();

    // Ensure we have a theme loaded
    var manager = SelectLevelManager.Instance;
    if (manager.CurrentTheme == null) return;

    // Create circles for each level
    int levelCount = manager.CurrentTheme.levels.Length;
    for (int i = 0; i < levelCount; i++)
    {
      GameObject circle = Instantiate(circlePrefab, circleParent);
      Image img = circle.GetComponent<Image>();

      if (img != null)
      {
        img.enabled = true;
        img.sprite = (i <= manager.CurrentLevelIndex) ? filledCircleSprite : hollowCircleSprite;
      }

      circles.Add(img);
    }
  }

  public void HighlightCurrentLevel()
  {
    var manager = SelectLevelManager.Instance;
    if (manager.CurrentTheme == null || circles.Count == 0) return;

    for (int i = 0; i < circles.Count; i++)
    {
      circles[i].sprite = (i == manager.CurrentLevelIndex) ? filledCircleSprite : hollowCircleSprite;
    }
  }
}
