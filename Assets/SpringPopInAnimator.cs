using UnityEngine;

public class SpringPopInAnimator : MonoBehaviour
{
  public Vector3 targetScale = new Vector3(1f, 1f, 1f);
  public float stiffness = 150f;
  public float damping = 10f;

  private float currentScale = 0f;
  private float velocity = 0f;
  private bool isAnimating = false;

  void Update()
  {
    if (isAnimating)
    {
      HandleSpringScale();
    }
  }

  public void TriggerPopIn()
  {
    currentScale = 0f;
    velocity = 0f;
    gameObject.SetActive(true);
    isAnimating = true;
  }

  public bool IsAnimating()
  {
    return isAnimating;
  }

  private void HandleSpringScale()
  {
    float force = (1f - currentScale) * stiffness;
    velocity += force * Time.deltaTime;
    velocity -= velocity * damping * Time.deltaTime;
    currentScale += velocity * Time.deltaTime;
    transform.localScale = targetScale * currentScale;

    if (Mathf.Abs(1f - currentScale) < 0.001f && Mathf.Abs(velocity) < 0.001f)
    {
      transform.localScale = targetScale;
      isAnimating = false;
    }
  }
}
