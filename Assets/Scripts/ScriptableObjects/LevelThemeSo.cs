using UnityEngine;

[CreateAssetMenu()]
public class LevelThemeSo : ScriptableObject
{
  public string levelThemeName;
  public Sprite themeIcon;
  public LevelSo[] levels;
  public bool isAvailable;
}
