using System.Collections;
using UnityEngine;

public class Star : MonoBehaviour
{
  [SerializeField] private ParticleSystem _collectionEffect;

  private void Start()
  {
    if (DemoLevelManager.Instance != null)
    {
      GetComponent<SpriteRenderer>().color = DemoLevelManager.Instance.GetLevelThemeColors().tileColor;
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Player"))
    {
      SoundManager.Instance.PlayStarCollect(transform.position);
      StartCoroutine(AnimateCollection());
    }
  }

  private IEnumerator AnimateCollection()
  {
    _collectionEffect.Play();

    GetComponent<SpriteRenderer>().enabled = false;

    yield return new WaitForSeconds(_collectionEffect.main.duration + _collectionEffect.main.startLifetime.constantMax);

    DemoLevelManager.Instance.CompleteLevel();
  }
}
