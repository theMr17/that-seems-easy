using UnityEngine;
using UnityEngine.UI;

public class OptionsUi : MonoBehaviour
{
  public const string KEY_SFX_VOLUME = "SfxVolume";
  public const string KEY_MUSIC_VOLUME = "MusicVolume";

  public Slider sfxVolumeSlider;
  public Slider musicVolumeSlider;
  public Button closeButton;

  private void Start()
  {
    gameObject.SetActive(false);

    sfxVolumeSlider.value = PlayerPrefs.GetFloat(KEY_SFX_VOLUME, 1.0f);
    sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);

    musicVolumeSlider.value = PlayerPrefs.GetFloat(KEY_MUSIC_VOLUME, 1.0f);
    musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

    closeButton.onClick.AddListener(CloseOptions);
  }

  private void SetSfxVolume(float value)
  {
    PlayerPrefs.SetFloat(KEY_SFX_VOLUME, value);
  }

  private void SetMusicVolume(float value)
  {
    PlayerPrefs.SetFloat(KEY_MUSIC_VOLUME, value);
  }

  private void CloseOptions()
  {
    gameObject.SetActive(false);
  }
}
