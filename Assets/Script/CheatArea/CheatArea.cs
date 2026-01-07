using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using LitMotion;
using LitMotion.Extensions;
using static System.Net.Mime.MediaTypeNames;

// Gére les table, et l'action de copier
public class CheatArea : MonoBehaviour
{
    public bool IsCopying=false;
    private float TimeToCopy = 5.0f;
    private float TimePassed;
    private bool ResultCopy = false;
    private bool BeenCopied = false;

    private IEnumerator coroutine;

    public void StartCopy()
    {
        
        if (!IsCopying) // Si il ne copie pas encore
        {
            IsCopying = true;
            coroutine = Cheating();
            StartCoroutine(coroutine);
        }
    }

    public void StopCopy()
    {
        if (coroutine != null) {
            IsCopying = false;
            TimePassed = 0;
            StopCoroutine(coroutine);
            coroutine= null;
        }
        TimeBarManager.Instance.resetProgress();
    }

    public bool GetResult()
    {
        if (BeenCopied)
        {
            return ResultCopy; //retourne le résultat du score}
        }
        return false;
    }

    IEnumerator Cheating()
    {
        
        while(TimePassed<TimeToCopy)
        {
            TimePassed += Time.deltaTime;
            Debug.Log(TimePassed);
            TimeBarManager.Instance.SetProgress(TimePassed);
            TimeBarManager.Instance.SetTextToShow(tag);
            yield return new WaitForEndOfFrame();
        }

        switch (tag)
        {
            case "BadStudent":

                ResultCopy = false;
                BeenCopied = true;
                break;
            case "GoodStudent":
                Debug.Log("Une bonne réponse");
                ScoreManager.Instance.addScore();
                BeenCopied = true;
                tag = "AlreadyCopied";
                break;
            case "AlreadyCopied":
                Debug.Log("Deja copié !");  
                break;
            default:
                Debug.Log("Pas de tag ici");
                break;
        }

        StopCopy();
        Debug.Log("Copié");  
    }
}


