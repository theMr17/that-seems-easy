using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
  public static SceneLoader Instance { get; private set; }

  public Animator transitionAnimator;
  public float transitionDuration = 1f;

  public enum Scene
  {
    MainMenuScene,
    LevelSelectionScene,
    Spring01,
    Spring02,
    Spring03,
    Spring04,
    Spring05,
    Gaps01,
    Gaps02,
    Gaps03,
    Gaps04,
    Gaps05,
    Spikes01,
    Spikes02,
    Spikes03,
    Spikes04,
    Spikes05,
    Push01,
    Push02,
    Push03,
    Push04,
    Push05,
  }

  private void Awake()
  {
    Instance = this;
  }

  public void LoadScene(Scene targetScene, bool useTransition = true)
  {
    if (useTransition && transitionAnimator != null)
    {
      StartCoroutine(LoadSceneWithTransition(targetScene));
    }
    else
    {
      SceneManager.LoadScene(targetScene.ToString());
    }
  }

  IEnumerator LoadSceneWithTransition(Scene targetScene)
  {
    transitionAnimator.SetTrigger("Start");
    yield return new WaitForSeconds(transitionDuration);
    SceneManager.LoadScene(targetScene.ToString());
  }
}