using System;
using System.Collections.Generic;
using System.Linq;
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
    private List<float> highScores = new List<float>(); // Changé en List<float>
    private float currentTime = 0f;
    private bool isTimerRunning;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        StartTimer();
        textScore.text = NumberCopy.ToString() + "/3";

        // Charger les highscores
        LoadHighScores();

        Debug.Log("Highscores chargés: " + string.Join(", ", highScores));
    }

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
            // Formater le temps pour un affichage plus lisible
            textChrono.text = Math.Round(currentTime).ToString() + " secondes";
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
        textScore.text = NumberCopy.ToString() + "/3";
    }

    // Timer
    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        if (!isTimerRunning) return;

        isTimerRunning = false;

        Debug.Log("Temps final: " + currentTime.ToString("F2"));
        Debug.Log("Highscores avant ajout: " + string.Join(", ", highScores));

        // Ajouter le nouveau score
        highScores.Add(currentTime);

        // Trier les scores en ordre croissant (les meilleurs temps sont les plus petits)
        highScores.Sort();

        // Garder seulement les 3 meilleurs scores
        if (highScores.Count > 3)
        {
            highScores = highScores.Take(3).ToList();
        }

        Debug.Log("Highscores après ajout: " + string.Join(", ", highScores));

        // Sauvegarder
        SaveHighScores();
    }

    public void ResetTimer()
    {
        currentTime = 0f;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    private void LoadHighScores()
    {
        highScores.Clear();

        // Charger les 3 meilleurs temps
        for (int i = 1; i <= 3; i++)
        {
            // Utilisez la même clé que celle que vous utilisez pour sauvegarder
            float score = PlayerPrefs.GetFloat("temps" + i, 0f);

            // Ne pas ajouter les scores nuls (première exécution)
            if (score > 0)
            {
                highScores.Add((float)Math.Round(score,2));
            }
        }

        // Si aucun score n'est chargé, initialiser avec des valeurs par défaut
        if (highScores.Count == 0)
        {
            highScores.Add(999f); // Temps très élevé comme placeholder
            highScores.Add(999f);
            highScores.Add(999f);
        }
    }

    private void SaveHighScores()
    {
        // Sauvegarder les 3 meilleurs scores
        for (int i = 0; i < 3; i++)
        {
            if (i < highScores.Count)
            {
                PlayerPrefs.SetFloat("temps" + (i + 1), (float)Math.Round(highScores[i],2));
            }
            else
            {
                PlayerPrefs.SetFloat("temps" + (i + 1), 999);
            }
        }
        PlayerPrefs.Save();
    }

    // Pour afficher les highscores dans le debug
    public void PrintHighScores()
    {
        for (int i = 0; i < highScores.Count; i++)
        {
            Debug.Log($"Highscore {i + 1}: {highScores[i]}");
        }
    }
}