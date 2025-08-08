using UnityEngine;

public class Trigger : MonoBehaviour
{
  [SerializeField] private string animatorParameterName;

  private void Start()
  {

  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      // Trigger the animation parameter
      if (DemoLevelManager.Instance != null)
      {
        DemoLevelManager.Instance.TriggerAnimation(animatorParameterName);
      }

      // Disable the collider to prevent re-triggering
      GetComponent<Collider2D>().enabled = false;
    }
  }
}
