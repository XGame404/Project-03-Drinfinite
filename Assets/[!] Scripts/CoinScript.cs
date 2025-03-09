using UnityEngine;

public class CoinScript : MonoBehaviour
{
    void Update()
    {
        this.gameObject.transform.Rotate(0, -50f * Time.deltaTime, 0);
    }
}
