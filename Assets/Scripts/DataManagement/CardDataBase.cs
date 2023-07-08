using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static CardDataBase instance;
    public List<CardDetails> cardDetails;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
