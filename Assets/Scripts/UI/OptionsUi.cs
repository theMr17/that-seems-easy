using UnityEngine;
using UnityEngine.UI;

public class OptionsUi : MonoBehaviour
{
  public Slider sfxVolumeSlider;
  public Slider musicVolumeSlider;
  public Button closeButton;

  private void Start()
  {
    gameObject.SetActive(false);

    if (SoundManager.Instance != null)
      sfxVolumeSlider.value = SoundManager.Instance.GetVolume();
    sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);

    if (MusicManager.Instance != null)
      musicVolumeSlider.value = MusicManager.Instance.GetVolume();
    musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

    closeButton.onClick.AddListener(CloseOptions);
  }

  private void SetSfxVolume(float value)
  {
    SoundManager.Instance.ChangeVolume(value);
  }

  private void SetMusicVolume(float value)
  {
    MusicManager.Instance.ChangeVolume(value);
  }

  private void CloseOptions()
  {
    gameObject.SetActive(false);
  }
}
