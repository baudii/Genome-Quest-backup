using System.Collections;
using TMPro;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    [SerializeField] GameObject messagesParent;
    [SerializeField] GameObject[] messages;
    [SerializeField] TextMeshProUGUI correctGuessesText;
    GameObject currentMessage;

    static GameEnd instance;
    public static GameEnd Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameEnd>(true);
            return instance;
        }
    }

    public void EndGame(int correctGuesses)
    {
        string endText = "";
        if (correctGuesses == 0)
        {
            endText = "Oops...\r\n\r\nyou guessed 0/3\r\n<size=35>You didn't correctly diagnose the patient";
        }
        else if (correctGuesses == 1)
        {
            endText = "Oops...\r\n\r\nyou guessed 1/3\r\n<size=35>You didn't correctly diagnose the patient";
        }
        else if (correctGuesses == 2)
        {
            endText = "So close!\r\n\r\nyou guessed 2/3\r\n<size=35>You were on a right track!";
        }
        else
        {
            endText = "Phenomenal!\r\n\r\nyou guessed 3/3!\r\n<size=35>You correctly diagnosed a rare disease";
        }
        correctGuessesText.text = endText;
        gameObject.SetActive(true);
        GetComponent<Animator>().Play("BlackScreenFadeIn");
        messagesParent.SetActive(true);
        foreach (GameObject message in messages)
        {
            message.SetActive(false);
        }
        StartCoroutine(ShowEndText());
    }

    IEnumerator ShowEndText()
    {
        foreach (var message in messages)
        {
            if (currentMessage != null)
                currentMessage.SetActive(false);
            message.SetActive(true);
            currentMessage = message;
            yield return new WaitUntil(() => Input.anyKeyDown);
            yield return null;
        }
    }
}
