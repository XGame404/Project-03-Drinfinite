using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharSelector : MonoBehaviour
{
    [SerializeField] GameObject[] CharSelectors;
    [SerializeField] Button NextButton_Left;
    [SerializeField] Button NextButton_Right;
    [SerializeField] private AudioClip ButtonClickSound;
    private AudioSource _audioSource;
    public int currentIndex = 0;

    [SerializeField] private bool DumpPackSelector;

    private void Start()
    {
        if (DumpPackSelector == false)
        {
            currentIndex = GameDataManager.GetSelectedChar();

            if (NextButton_Left != null)
            {
                NextButton_Left.onClick.AddListener(ShowPreviousShip);
            }

            if (NextButton_Right != null)
            {
                NextButton_Right.onClick.AddListener(ShowNextShip);
            }

            _audioSource = GetComponent<AudioSource>();

            UpdateShipModelSelectors();
        }
        else 
        {
            currentIndex = 0;

            if (NextButton_Left != null)
            {
                NextButton_Left.onClick.AddListener(ShowPreviousShip);
            }

            if (NextButton_Right != null)
            {
                NextButton_Right.onClick.AddListener(ShowNextShip);
            }

            _audioSource = GetComponent<AudioSource>();

            UpdateShipModelSelectors();
        }
    }

    private void ShowPreviousShip()
    {
            _audioSource.PlayOneShot(ButtonClickSound);
            currentIndex = (currentIndex - 1 + CharSelectors.Length) % CharSelectors.Length;
            UpdateShipModelSelectors();

    }

    private void ShowNextShip()
    {
            _audioSource.PlayOneShot(ButtonClickSound);
            currentIndex = (currentIndex + 1) % CharSelectors.Length;
            UpdateShipModelSelectors();
        
    }

    private void UpdateShipModelSelectors()
    {
        for (int i = 0; i < CharSelectors.Length; i++)
        {
            if (i == currentIndex)
                CharSelectors[i].SetActive(true);
            else
                CharSelectors[i].SetActive(false);
        }
    }
}
