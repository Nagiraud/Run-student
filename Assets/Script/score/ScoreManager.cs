using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [SerializeField] private TMP_Text textScore;
    
    // Nombre copie
    public int NumberCopy = 0;

    // Chronométre
    [SerializeField] private TMP_Text textChrono;
    private string[] highScore;
    private float currentTime = 0f;
    private bool isTimerRunning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartTimer();
        textScore.text = NumberCopy.ToString()+"/3";
    }

    private void OnDisable()
    {
        StopTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
            textChrono.text = currentTime.ToString();
        }
    }

    //Score
    public void addScore()
    {
        NumberCopy += 1;
        UpdateScore();
    }

    public int GetScore()
    {
        return NumberCopy;
    }
    public void UpdateScore()
    {
        textScore.text = NumberCopy.ToString()+"/3";
    }


    // Timer
    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = 0f;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}

