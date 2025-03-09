using UnityEngine;
using System.Collections;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    [SerializeField] private Transform lastDefaultPlatform;

    private Vector3 currentPlatformPosition;
    private Vector3 newPlatformPosition;

    private float spawnCooldown = 0.1f;
    private float nextSpawnTime = 0f;

    private float platformGap = 7.5f; 
    private float maxGap = 7.5f; 
    private float gapIncreaseRate = 0.5f; 
    private float gapIncreaseInterval = 5f;

    void Start()
    {
        currentPlatformPosition = lastDefaultPlatform.position;
        StartCoroutine(IncreaseGapOverTime());
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (GameManager.instance.IsGameEnded == false && player != null)
        {
            float distanceZ = player.transform.position.z - currentPlatformPosition.z;

            if (Mathf.Abs(distanceZ) <= 20 && Time.time >= nextSpawnTime)
            {
                nextSpawnTime = Time.time + spawnCooldown;

                GeneratePlatform();
                GameObject newPlatform = Instantiate(platform, newPlatformPosition, Quaternion.identity);
                if (newPlatform != null)
                {
                    currentPlatformPosition = newPlatformPosition;
                }
            }
        }
    }

    void GeneratePlatform()
    {
        newPlatformPosition = currentPlatformPosition;

        int randomLeftRight = Random.Range(0, 2);

        if (randomLeftRight == 0)
        {
            newPlatformPosition.x += platformGap;
        }
        else
        {
            newPlatformPosition.z += platformGap;
        }
    }

    IEnumerator IncreaseGapOverTime()
    {
        while (platformGap < maxGap)
        {
            yield return new WaitForSeconds(gapIncreaseInterval);
            platformGap = Mathf.Min(platformGap + gapIncreaseRate, maxGap);
        }
    }
}
