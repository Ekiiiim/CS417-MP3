using UnityEngine;

public class HouseEaseIn : MonoBehaviour
{
    public float easeSpeed = 8.0f;
    private Vector3 initialScale = Vector3.zero;
    private Vector3 targetScale = Vector3.one;
    private bool isUnlocking = false;

    public void Unlock()
    {
        isUnlocking = true;
    }

    void Start()
    {
        transform.localScale = initialScale;
    }

    void Update()
    {
        if (isUnlocking)
        {
            transform.localScale += (targetScale - transform.localScale) * easeSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                transform.localScale = targetScale;
                isUnlocking = false;
            }
        }
    }
}
