using UnityEngine;
using System.Collections;

public class EyeController : MonoBehaviour
{
    [Header("References")]
    public Transform eyeBall;
    public Transform pupil;

    [Header("Tracking")]
    public Transform target;
    public float moveRange = 0.4f;

    [Header("Blinking")]
    public float blinkInterval = 5.0f;
    public float blinkSpeed = 10.0f;

    private Vector3 centerPoint;
    private Vector3 normalScale;
    private Vector3 currentGoalScale;

    void Start()
    {
        centerPoint = pupil.localPosition;
        normalScale = eyeBall.localScale;
        currentGoalScale = normalScale;

        StartCoroutine(BlinkSequence());
    }

    void Update()
    {
        if (target == null) return;

        HandleTracking();
    }

    void HandleTracking()
    {
        Vector3 worldDir = target.position - eyeBall.position;
        Vector3 localDir = eyeBall.InverseTransformDirection(worldDir).normalized;

        float newX = localDir.x * moveRange;
        float newZ = localDir.z * moveRange;
        pupil.localPosition = new Vector3(newX, centerPoint.y, newZ);
    }

    IEnumerator BlinkSequence()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkInterval);

            while (eyeBall.localScale.z > 0.01f)
            {
                float delta = (0f - eyeBall.localScale.z) * blinkSpeed * Time.deltaTime;
                eyeBall.localScale = new Vector3(eyeBall.localScale.x, eyeBall.localScale.y, eyeBall.localScale.z + delta);
                yield return null;
            }

            while (eyeBall.localScale.z < (currentGoalScale.z - 0.01f))
            {
                float delta = (currentGoalScale.z - eyeBall.localScale.z) * blinkSpeed * Time.deltaTime;
                eyeBall.localScale = new Vector3(eyeBall.localScale.x, eyeBall.localScale.y, eyeBall.localScale.z + delta);
                yield return null;
            }
        }
    }
}