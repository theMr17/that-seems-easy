using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUi : MonoBehaviour
{
  [SerializeField] private Button pauseButton;
  [SerializeField] private GameObject pauseMenuPanel;
  [SerializeField] private Button resumeButton;
  [SerializeField] private Button mainMenuButton;

  private void Start()
  {
    pauseMenuPanel.SetActive(false);
    pauseButton.onClick.AddListener(PauseGame);
    resumeButton.onClick.AddListener(ResumeGame);
    mainMenuButton.onClick.AddListener(GoToMainMenu);
  }

  private void PauseGame()
  {
    Time.timeScale = 0f;
    pauseMenuPanel.SetActive(true);
  }

  private void ResumeGame()
  {
    Time.timeScale = 1f;
    pauseMenuPanel.SetActive(false);
  }

  private void GoToMainMenu()
  {
    Time.timeScale = 1f;
    SceneLoader.Instance.LoadScene(SceneLoader.Scene.MainMenuScene);
  }
}
