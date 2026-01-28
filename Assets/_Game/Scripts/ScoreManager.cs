using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Text movesText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private GameObject newRecordObject;

    private int _currentMoves;
    private int _bestScore;
    private string _levelKey; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _levelKey = SceneManager.GetActiveScene().name + "_BestScore";
        _bestScore = PlayerPrefs.GetInt(_levelKey, 999);

        UpdateUI();
    }

    public void AddMove()
    {
        _currentMoves++;
        UpdateUI();
    }

    public void UndoMove()
    {
        if (_currentMoves > 0)
        {
            _currentMoves--;
        }
        UpdateUI();
    }
    public void CompleteLevel()
    {
        if (_currentMoves < _bestScore)
        {
            _bestScore = _currentMoves;

            PlayerPrefs.SetInt(_levelKey, _bestScore);
            PlayerPrefs.Save();

            if (newRecordObject != null)
            {
                newRecordObject.SetActive(true);
            }
        }
    }
    private void UpdateUI()
    {
        if (movesText != null)
        {
            movesText.text = "Moves: " + _currentMoves;
        }

        if (highScoreText != null)
        {
            string displayScore = (_bestScore == 999) ? "-" : _bestScore.ToString();
            highScoreText.text = "Best: " + displayScore;
        }
    }
}