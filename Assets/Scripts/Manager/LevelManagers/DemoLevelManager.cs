using UnityEngine;

class DemoLevelManager : MonoBehaviour
{
    public DemoLevelManager Instance { get; private set; }
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LevelThemeColorSo colorSo;

    private void Start()
    {
        mainCamera.backgroundColor = colorSo.backgroundColor;
    }
}