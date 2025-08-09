using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
  [SerializeField] private Button _playButton;
  [SerializeField] private Button _optionsButton;
  [SerializeField] private Button _exitButton;
  [SerializeField] private GameObject _optionsPanel;

  private void Awake()
  {
    _playButton.onClick.AddListener(OnPlayButtonClicked);
    _optionsButton.onClick.AddListener(OnOptionsButtonClicked);
    _exitButton.onClick.AddListener(OnExitButtonClicked);
  }

  private void OnPlayButtonClicked()
  {
    SceneManager.LoadScene("LevelSelectionScene");
  }

  private void OnOptionsButtonClicked()
  {
    _optionsPanel.SetActive(true);
  }

  private void OnExitButtonClicked()
  {
    Application.Quit();
  }
}
