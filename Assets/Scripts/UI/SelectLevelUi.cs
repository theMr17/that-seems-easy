using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelUi : MonoBehaviour
{
  public Transform levelButtonContainer;
  public GameObject levelButtonPrefab;
  public Sprite lockedThemeIcon; // Optional placeholder icon for locked themes

  private void Start()
  {
    PopulateThemeButtons();
  }

  private void PopulateThemeButtons()
  {
    // Clear existing buttons
    foreach (Transform child in levelButtonContainer)
    {
      Destroy(child.gameObject);
    }

    var themes = SelectLevelManager.Instance.levelThemes;

    foreach (var theme in themes)
    {
      var buttonObj = Instantiate(levelButtonPrefab, levelButtonContainer);

      // Theme icon
      if (buttonObj.transform.Find("LevelThemeIcon").TryGetComponent<Image>(out var themeIcon))
      {
        themeIcon.sprite = theme.themeIcon;
      }

      // Theme name
      if (buttonObj.transform.Find("LevelThemeName").TryGetComponent<TextMeshProUGUI>(out var themeNameText))
      {
        themeNameText.text = theme.levelThemeName;
      }

      var btn = buttonObj.GetComponent<Button>();
      btn.onClick.RemoveAllListeners();

      bool isUnlocked = SelectLevelManager.Instance.IsThemeUnlocked(theme);

      if (isUnlocked)
      {
        btn.interactable = true;
        btn.onClick.AddListener(() => SelectLevelManager.Instance.LoadLastUnlockedLevel(theme));
      }
      else
      {
        btn.interactable = false;

        if (buttonObj.transform.Find("PlayerImage").TryGetComponent<Image>(out var playerImage))
        {
          playerImage.sprite = lockedThemeIcon;
        }
      }
    }
  }
}
