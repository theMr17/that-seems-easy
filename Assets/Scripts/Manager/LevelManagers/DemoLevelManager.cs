using UnityEngine;
using UnityEngine.SceneManagement;

class DemoLevelManager : MonoBehaviour
{
    public static DemoLevelManager Instance { get; private set; }
    [SerializeField] private LevelThemeColorSo colorSo;
    private Animator _animator;

    private void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
    }

    public LevelThemeColorSo GetLevelThemeColors()
    {
        return colorSo;
    }

    public void TriggerAnimation(string parameterName)
    {
        _animator.SetTrigger(parameterName);
    }

    public void Reset()
    {
        SceneLoader.Instance.LoadScene((SceneLoader.Scene)System.Enum.Parse(typeof(SceneLoader.Scene), SceneManager.GetActiveScene().name));
    }

    public void CompleteLevel()
    {
        SelectLevelManager.Instance.CompleteLevel();
    }
}
