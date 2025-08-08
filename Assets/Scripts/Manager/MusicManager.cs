using UnityEngine;

public class MusicManager : MonoBehaviour
{
  private const string PLAYER_PREFS_MUSIC_EFFECTS_VOLUME = "MusicVolume";

  public static MusicManager Instance { get; private set; }

  private AudioSource _audioSource;

  private void Awake()
  {
    Instance = this;

    _audioSource = GetComponent<AudioSource>();

    float _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_EFFECTS_VOLUME, 0.3f);
    _audioSource.volume = _volume;
  }

  public void ChangeVolume(float volume)
  {
    _audioSource.volume = volume;

    PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_EFFECTS_VOLUME, volume);
    PlayerPrefs.Save();
  }

  public float GetVolume()
  {
    return PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_EFFECTS_VOLUME, 0.3f);
  }
}
