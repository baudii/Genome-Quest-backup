using System.Runtime.CompilerServices;
using UnityEngine;

public class ErrorHandler : MonoBehaviour
{
    static ErrorHandler instance;
    public static ErrorHandler Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ErrorHandler>(true);
            return instance;
        }
    }

    public void EnableErrorMessage()
    {
        print(gameObject.name);
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
