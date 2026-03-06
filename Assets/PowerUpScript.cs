using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    public ResourceManager gameManager;
    public float lifeTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GlobalManagers").GetComponent<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void UsePowerUp()
    {
        gameManager.ActivatePowerUp();
        Destroy(gameObject);
    }
}
