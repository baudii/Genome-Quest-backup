using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class NPC_ImmediateDialogue : MonoBehaviour
{
    [SerializeField, TextArea(minLines: 10, maxLines: 20)] string[] lines;
    [SerializeField, TextArea(minLines: 10, maxLines: 20)] string[] lines2;
    [SerializeField] UnityEvent OnDialogueFinished;
    [SerializeField] Interactable interactable;
    int currentLines = 0;
    public void SayEachLine()
    {
        if (currentLines == 0)
            StartCoroutine(Lines1());
        else if (currentLines == 2)
            StartCoroutine(Lines2());
    }

    IEnumerator Lines2()
    {
        var dialogueController = UI_DialogueController.Instance;
        dialogueController.InitiateDialogue(canAnswer: false);
        bool canContinue = false;
        foreach (string text in lines2)
        {
            dialogueController.GenerateOutput(text, () => { canContinue = true; });

            yield return new WaitUntil(() => canContinue);

            canContinue = false;
            yield return null;

            yield return new WaitUntil(() => Input.anyKeyDown);
        }
        dialogueController.CloseDialogue();
        Destroy(this);
    }

    IEnumerator Lines1()
    {
        var dialogueController = UI_DialogueController.Instance;
        dialogueController.InitiateDialogue(canAnswer: false);
        bool canContinue = false;
        foreach (string text in lines)
        {
            dialogueController.GenerateOutput(text, () => { canContinue = true; });
            
            yield return new WaitUntil(() => canContinue);

            canContinue = false;
            yield return null;

            yield return new WaitUntil(() => Input.anyKeyDown);
        }
        dialogueController.CloseDialogue();
        OnDialogueFinished?.Invoke();
        currentLines++;
    }


    public void NextLine() => currentLines++;
}
