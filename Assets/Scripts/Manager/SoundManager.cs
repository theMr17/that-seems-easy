using UnityEngine;

public class SoundManager : MonoBehaviour
{
  private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

  public static SoundManager Instance { get; private set; }

  [SerializeField] private AudioClipRefsSo _audioClipRefsSo;

  private float _volume = 1f;

  private void Awake()
  {
    Instance = this;

    _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
  }

  private void Start()
  {

  }

  private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
  {
    PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
  }

  private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
  {
    AudioSource.PlayClipAtPoint(audioClip, position, volume);
  }

  public void PlayStarCollect(Vector3 position)
  {
    PlaySound(_audioClipRefsSo.collectStar, position, _volume);
  }

  public void ChangeVolume(float volume)
  {
    _volume = volume;

    PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
    PlayerPrefs.Save();
  }

  public float GetVolume()
  {
    return _volume;
  }
}