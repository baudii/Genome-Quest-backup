using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [SerializeField, Range(0,10)] float timeToFade;
    [SerializeField] AudioClip notificationSound;
    TextMeshProUGUI textField;
    AudioSource audioSource;
    Animator animator;
    static NotificationUI instance;
    public static NotificationUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NotificationUI>(true);
            }
            return instance;
        }
    }
    private void Awake()
    {
        textField = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //gameObject.SetActive(false);
        animator.SetBool("Visible", false);
    }

    public void Notify(string message)
    {
        StopAllCoroutines();
        if (textField != null)
        {
            textField.text = message;
        }
        gameObject.SetActive(true);
        audioSource.PlayOneShot(notificationSound);
        animator.Play("UI_Appear");
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(timeToFade);
        animator.Play("UI_Fader");
        yield return new WaitForSeconds(4);
        gameObject.SetActive(false);
    }
}
