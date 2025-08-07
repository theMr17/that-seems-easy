using UnityEngine;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
  [SerializeField] private Button _playButton;
  [SerializeField] private Button _optionsButton;
  [SerializeField] private Button _exitButton;

  private void Awake()
  {
    _playButton.onClick.AddListener(OnPlayButtonClicked);
    _optionsButton.onClick.AddListener(OnOptionsButtonClicked);
    _exitButton.onClick.AddListener(OnExitButtonClicked);
  }

  private void OnPlayButtonClicked()
  {
    Debug.Log("Play button clicked.");
  }

  private void OnOptionsButtonClicked()
  {
    Debug.Log("Options button clicked.");
  }

  private void OnExitButtonClicked()
  {
    Application.Quit();
  }
}
