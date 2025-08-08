using UnityEngine;

class DemoLevelManager : MonoBehaviour
{
    public static DemoLevelManager Instance { get; private set; }
    [SerializeField] private LevelThemeColorSo colorSo;

    private void Awake()
    {
        Instance = this;
    }

    public LevelThemeColorSo GetLevelThemeColors()
    {
        return colorSo;
    }

    public void TriggerAnimation(string parameterName)
    {
        GetComponent<Animator>().SetTrigger(parameterName);
    }

    public void CompleteLevel()
    {
        Debug.Log("Level Completed!");
    }
}
