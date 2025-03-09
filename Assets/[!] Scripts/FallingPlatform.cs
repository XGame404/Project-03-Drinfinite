using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.2f; 
    [SerializeField] private float destroyDelay = 1f; 
    [SerializeField] private float fallSpeed = 5f; 
    [SerializeField] private GameObject CoinSpawnPoint;
    [SerializeField] private GameObject CoinPrefab;
    private int RandomNumb;

    private Rigidbody rb;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        RandomNumb = Random.Range(0, 101);
        if (RandomNumb < 31) 
        {
            Instantiate(CoinPrefab, CoinSpawnPoint.transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallRoutine());
        }
    }

    private IEnumerator FallRoutine()
    {
        yield return new WaitForSeconds(fallDelay);

        rb.isKinematic = true;
        rb.useGravity = true;
        isFalling = true;

        Destroy(gameObject, destroyDelay);
    }

    void Update()
    {
        if (isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }
}
