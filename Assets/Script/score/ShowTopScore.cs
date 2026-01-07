using TMPro;
using UnityEngine;

public class ShowTopScore : MonoBehaviour
{
    // Top Score
    public TMP_Text TopScore1;
    public TMP_Text TopScore2;
    public TMP_Text TopScore3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TopScore1.text = PlayerPrefs.GetFloat("temps1").ToString();
        TopScore2.text = PlayerPrefs.GetFloat("temps2").ToString();
        TopScore3.text = PlayerPrefs.GetFloat("temps3").ToString();
    }

}
