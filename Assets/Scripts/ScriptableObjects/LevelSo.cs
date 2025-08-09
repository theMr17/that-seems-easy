using UnityEngine;

[CreateAssetMenu()]
public class LevelSo : ScriptableObject
{
  [Tooltip("Name of the scene to load")]
  public string sceneName;
}
