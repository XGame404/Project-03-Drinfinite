using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public bool IsGameStarted;
    public bool IsGameEnded;

    public bool IsFirstClickPerformed = false;

    private GameObject Player_GO;
    [SerializeField] private GameObject[] CharactersList;
    [SerializeField] private int SelectedCharID;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        IsGameStarted = false;
        IsGameEnded = false;

        Player_GO = GameObject.FindGameObjectWithTag("Player");
        if (Player_GO == null)
        {
            Instantiate(CharactersList[GameDataManager.GetSelectedChar()],
                        this.gameObject.transform.position,
                        CharactersList[GameDataManager.GetSelectedChar()].transform.rotation);
        }
    }

    void Update()
    {
        
    }

    public void GameStart() 
    {
        if (IsGameStarted == false) 
        {
            IsGameStarted = true;
        }
    }

    public void GameEnd()
    {
        if (IsGameEnded == false)
        {
            IsGameEnded = true;
            Invoke("ReloadLevel", 2f);
        }
    }

    public void FirstClickPerform()
    {
        if (IsFirstClickPerformed == false)
        {
            IsFirstClickPerformed = true;
        }
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene("Result");
    }
}
