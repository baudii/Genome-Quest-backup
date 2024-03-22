using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_DialogueController : MonoBehaviour
{
    [SerializeField] GameObject loadingAnimation;
    [SerializeField] PlayerDialogueController playerDialogueController;
    [SerializeField] Button leaveButton;
    [SerializeField] TextMeshProUGUI outputField;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] RectTransform outputRectTransform;
    [SerializeField] float textFillAnimationDelay;
    [SerializeField] float pauseBetweenSentases;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip blopAudio;
    [SerializeField] GameObject objective;
    string currentText;
    const float maxInboxHeight = 250;
    const float minInboxHeight = 65;
    bool isAnimating;
    bool inDialogue;
    bool canAnswer;
    static UI_DialogueController instance;
    Action OnOutputEnd;

    public UnityEvent OnCloseDialogue;

    Action<string> OnInputSubmit;
    public static UI_DialogueController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UI_DialogueController>(true);
            }
            return instance;
        }
    }

    private void Update()
    {
        if (!inDialogue)
            return;

        if (Input.anyKeyDown)
        {
            if (isAnimating)
            {
                StopAllCoroutines();
                outputField.text = currentText;
                OnOutputGenerated();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (inputField.interactable)
            {
                SubmitInputfield();
            }
        }

        if (outputField.isTextOverflowing && outputRectTransform.sizeDelta.y < maxInboxHeight)
        {
            outputRectTransform.sizeDelta = new Vector2(outputRectTransform.sizeDelta.x, outputRectTransform.sizeDelta.y + outputField.fontSize);
        }
    }

    void OnOutputGenerated()
    {
        if (canAnswer)
        {
            inputField.interactable = true;
            inputField.Select();
        }
        isAnimating = false;
        OnOutputEnd?.Invoke();
        OnOutputEnd = null;
    }

    public void Wait()
    {
        inputField.interactable = false;
        outputField.text = "";
        loadingAnimation.SetActive(true);
        inputField.text = "";
    }

    public void DisableInputField()
    {
        inputField.interactable = false;
        inputField.text = "";
    }

    public void EnableInputField()
    {
        inputField.interactable = true;
        inputField.text = "";
    }

    public void SubmitInputfield()
    {
        OnInputSubmit?.Invoke(inputField.text);
        Wait();
    }

    public void GenerateOutput(string text, Action OnGenerated = null)
    {
        loadingAnimation.SetActive(false);
        outputRectTransform.sizeDelta = new Vector2(outputRectTransform.sizeDelta.x, outputField.fontSize);
        inputField.interactable = false;
        currentText = text;
        OnOutputEnd = OnGenerated;
        StartCoroutine(AnimatedText(text));
    }

    public void InitiateDialogue(bool canAnswer = true, Action<string> OnInputSubmit = null)
    {
        if (inDialogue)
            return;

        loadingAnimation.SetActive(false);
        outputRectTransform.sizeDelta = new Vector2(outputRectTransform.sizeDelta.x, outputField.fontSize);
        objective.SetActive(false);
        leaveButton.interactable = true;
        this.OnInputSubmit = OnInputSubmit;
        gameObject.SetActive(true);
        inDialogue = true;
        inputField.gameObject.SetActive(canAnswer);
        inputField.interactable = canAnswer;
        this.canAnswer = canAnswer;
        playerDialogueController.InitiateDialogue();
    }

    
    public void CloseDialogue()
    {
        StopAllCoroutines();
        loadingAnimation.SetActive(false);
        objective.SetActive(true);
        outputField.text = "";
        inputField.text = "";
        canAnswer = false;
        inDialogue = false;
        isAnimating = false;
        gameObject.SetActive(false);
        playerDialogueController.StopDialogue();
        OnCloseDialogue?.Invoke();
    }

    IEnumerator AnimatedText(string text)
    {
        if (isAnimating)
            yield break;
        isAnimating = true;
        outputField.text = "";
        int i = 0;
        foreach (char c in text)
        {
            outputField.text += c;
            if (i == 2)
            {
                audioSource.PlayOneShot(blopAudio);
                i = -1;
            }
            i++;
            if (c == '.' || c == ',')
                yield return new WaitForSeconds(textFillAnimationDelay + pauseBetweenSentases);
            else
                yield return new WaitForSeconds(textFillAnimationDelay);
        }
        
        OnOutputGenerated();
    }
}
