using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevelManager : MonoBehaviour
{
  public static SelectLevelManager Instance { get; private set; }

  [Header("Theme Settings")]
  public LevelThemeSo[] levelThemes; // Now stored here instead of UI

  private const string LAST_UNLOCKED_PREFIX = "LastUnlockedLevel_";
  private const string THEME_UNLOCKED_PREFIX = "ThemeUnlocked_";

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }
    Instance = this;
    DontDestroyOnLoad(gameObject);

    // Ensure the first theme is always unlocked
    if (!IsThemeUnlocked(levelThemes[0]))
    {
      UnlockTheme(levelThemes[0]);
    }
  }

  public void LoadLastUnlockedLevel(LevelThemeSo theme)
  {
    if (!IsThemeUnlocked(theme))
    {
      Debug.Log("Theme locked: " + theme.levelThemeName);
      return;
    }

    int lastUnlockedIndex = PlayerPrefs.GetInt(LAST_UNLOCKED_PREFIX + theme.levelThemeName, 0);

    if (lastUnlockedIndex >= theme.levels.Length)
    {
      lastUnlockedIndex = 0; // All completed → restart
    }

    var levelToLoad = theme.levels[lastUnlockedIndex];
    SceneManager.LoadScene(levelToLoad.sceneName);
  }

  public void UnlockNextLevel(LevelThemeSo theme, int unlockedIndex)
  {
    PlayerPrefs.SetInt(LAST_UNLOCKED_PREFIX + theme.levelThemeName, unlockedIndex);
    PlayerPrefs.Save();
  }

  public bool IsThemeUnlocked(LevelThemeSo theme)
  {
    return PlayerPrefs.GetInt(THEME_UNLOCKED_PREFIX + theme.levelThemeName, 0) == 1;
  }

  public void UnlockTheme(LevelThemeSo theme)
  {
    PlayerPrefs.SetInt(THEME_UNLOCKED_PREFIX + theme.levelThemeName, 1);
    PlayerPrefs.Save();
    UnlockNextLevel(theme, 0); // Always unlock first level when theme is unlocked
  }

  public void UnlockNextThemeIfNeeded(LevelThemeSo currentTheme)
  {
    int lastUnlockedIndex = PlayerPrefs.GetInt(LAST_UNLOCKED_PREFIX + currentTheme.levelThemeName, 0);

    if (lastUnlockedIndex >= currentTheme.levels.Length)
    {
      for (int i = 0; i < levelThemes.Length - 1; i++)
      {
        if (levelThemes[i] == currentTheme)
        {
          UnlockTheme(levelThemes[i + 1]);
          break;
        }
      }
    }
  }
}
