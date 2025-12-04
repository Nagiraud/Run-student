using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [SerializeField] private TMP_Text textScore;
    public int NumberCopy = 0;

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
        textScore.text = NumberCopy.ToString()+"/3";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
