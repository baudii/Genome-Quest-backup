using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    [SerializeField] float textFillAnimationDelay;
    [SerializeField] AudioClip blopAudio;
    [SerializeField] AudioSource audioSource;
    [SerializeField] TextMeshProUGUI textArea;
    [SerializeField] UnityEvent OnFinishAnimation;

    IEnumerator Start()
    {
        textArea.text = "";
        yield return new WaitForSeconds(0.3f);
        string text = "GENOME QUEST";
        foreach (char c in text)
        {
            textArea.text += c;
            if (c == ' ')
            {
                yield return new WaitForSeconds(textFillAnimationDelay + 0.6f);
                continue;
            }
            audioSource.PlayOneShot(blopAudio);
            yield return new WaitForSeconds(textFillAnimationDelay);
        }
        OnFinishAnimation?.Invoke();
    }
}
