using System.Collections;
using UnityEngine;

// Gére les table, et l'action de copier
public class TableCopy : MonoBehaviour
{
    public bool IsCopying=false;
    private float TimeToCopy = 5.0f;
    private float TimePassed;

    private IEnumerator coroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            Debug.Log("STOOOOOOP");
        }
    }

    IEnumerator Cheating()
    {
        
        while(TimePassed<TimeToCopy)
        {
            TimePassed += Time.deltaTime;
            Debug.Log(TimePassed);
            yield return new WaitForEndOfFrame();
        }

        switch (tag)
        {
            case "BadStudent":
                Debug.Log("Pas de réponse");
                break;
            case "GoodStudent":
                Debug.Log("Une bonne réponse");
                break;
            default:
                Debug.Log("Pas de tag ici");
                break;
        }
        Debug.Log("Copié");  
    }
}
