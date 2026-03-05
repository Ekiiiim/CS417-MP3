using UnityEngine;

public class GeneratorDeployer : MonoBehaviour
{
    [Header("What to deploy")]
    public GameObject generatorPrefab;

    [Header("Where to deploy")]
    public Transform spawnPoint;

    [Header("Limit (optional)")]
    public bool oneTime = false;

    private bool deployed = false;

    public void Deploy()
    {
        if (oneTime && deployed) return;
        deployed = true;

        if (generatorPrefab == null)
        {
            Debug.LogError("GeneratorDeployer: generatorPrefab is not assigned.");
            return;
        }

        Transform t = (spawnPoint != null) ? spawnPoint : transform;
        Instantiate(generatorPrefab, t.position, t.rotation);
    }
}