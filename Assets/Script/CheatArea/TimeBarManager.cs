using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LitMotion;
using LitMotion.Extensions;
using static UnityEngine.GraphicsBuffer;
using System.Collections;

public class TimeBarManager : MonoBehaviour
{

    public static TimeBarManager Instance { get; private set; }
    [Header("Slider")]
    public Slider slider;

    [Header("Texte")]
    public TMP_Text textGoodCopy;
    private string textToShow;

    bool IsShow =false;
    float progress;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        textGoodCopy.gameObject.SetActive(false);
    }

    public void SetTextToShow(string txt)
    {
        textToShow= txt;
    }
    public void changeMaxProgressBar(int nb)
    {
        slider.maxValue = nb;
    }
    public void SetProgress(float nb)
    {
        IsShow = true;
        progress = nb;
        updateProgressBar();
        if (progress >= 5)
        {
            StartCoroutine(AnimateText());
        }
    }
    public void resetProgress()
    {
        IsShow = false;
        progress = 0;
        updateProgressBar();
    }

    void updateProgressBar()
    {
        slider.value = progress;
    }

    void showProgressBar()
    {
        slider.GetComponent<Renderer>().enabled = IsShow;
    }

    IEnumerator AnimateText()
    {
        switch (textToShow)
        {
            case "BadStudent":
                textGoodCopy.text = "Mauvaise réponse !";
                break;
            case "GoodStudent":
                textGoodCopy.text = "Bonne réponse !";
                break;
            case "AlreadyCopied":
                textGoodCopy.text = "Déja copié !";
                break;
            default:
                textGoodCopy.text = "Pas de tag !";
                break;
        }
        
        textGoodCopy.gameObject.SetActive(true);
        LMotion.Create(Vector3.zero, Vector3.one, 1f)
            .WithEase(Ease.OutBack)
            .BindToLocalScale(textGoodCopy.transform);
        LMotion.Create(-15f, 15f, 1f)
            .WithEase(Ease.InOutSine)
            .WithLoops(4, LoopType.Yoyo)
            .BindToLocalEulerAnglesZ(textGoodCopy.transform);
        yield return new WaitForSeconds(2);
        textGoodCopy.gameObject.SetActive(false);
    }
}
