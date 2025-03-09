using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float speedIncrement = 0.1f;
    [SerializeField] private float maxSpeed = 20.0f;
    [SerializeField] private float lerpSpeed = 10f;
    [SerializeField] private GameObject[] Flames;
    private bool isMovingLeft = true;
    private Camera mainCamera;
    private Vector3 camDistance;
    private bool able2Move = true;
    private float speedIncreaseInterval = 1f;
    private float nextSpeedIncreaseTime = 0f;
    [SerializeField] private GameObject CoinGatheredEffect;
    private GameObject Score_GO;
    private TMP_Text Score_Text;
    private int PlayerScoreAndCoin = 0;

    void Start()
    {
        if (GameManager.instance.IsGameStarted == false)
        {
            able2Move = true;

            foreach (var flame in Flames)
            {
                flame.SetActive(false);
            }

            isMovingLeft = true;

            mainCamera = Camera.main;

            if (mainCamera != null)
            {
                camDistance = mainCamera.transform.position - transform.position;
            }

            nextSpeedIncreaseTime = Time.time + speedIncreaseInterval;

            Score_GO = GameObject.FindGameObjectWithTag("ScoreText");
            if (Score_GO != null)
            {
                Score_Text = Score_GO.GetComponent<TMP_Text>();
            }

            PlayerScoreAndCoin = 0;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.instance.GameStart();
        }

        if (GameManager.instance.IsGameStarted == true)
        {
            MovementSystem();

            if (Input.GetMouseButtonDown(0))
            {
                GameManager.instance.FirstClickPerform();
                foreach (var flame in Flames)
                {
                    flame.SetActive(true);
                }
            }

            if (Time.time >= nextSpeedIncreaseTime && moveSpeed < maxSpeed)
            {
                moveSpeed = Mathf.Min(moveSpeed + speedIncrement, maxSpeed);
                nextSpeedIncreaseTime = Time.time + speedIncreaseInterval;
            }
        }

        if (transform.position.y < -2f)
        {
            GameManager.instance.GameEnd();
            able2Move = false;
            GameDataManager.NewestCoinNumbGathered(PlayerScoreAndCoin);
        }

        Score_Text.text = PlayerScoreAndCoin.ToString("D4");
    }

    void MovementSystem()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);

        if (Input.GetMouseButtonDown(0) && GameManager.instance.IsFirstClickPerformed == true && able2Move == true)
        {
            if (isMovingLeft)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
                isMovingLeft = false;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                isMovingLeft = true;
            }
        }

        Vector3 desiredPosition = transform.position + camDistance;

        if (able2Move == true)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, Time.deltaTime * lerpSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Debug.Log("Coin collected! Current score: " + PlayerScoreAndCoin);
            Destroy(other.gameObject);
            Instantiate(CoinGatheredEffect, other.transform.position, Quaternion.identity);
            PlayerScoreAndCoin++;
            GameDataManager.AddCoins(1);

        }
    }
}