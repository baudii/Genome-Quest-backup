using UnityEngine;

public class DialogueInteractable : Interactable
{
    [SerializeField] bool isAiStart;
    [SerializeField] string aiStartLine;
    [SerializeField] CharacterDialogueComponent cdc;


    public override void Interact(Interactor caller)
    {
        if (isAiStart)
        {

        }
        else
        {
            cdc.StartDialogue();
        }

        base.Interact(caller);
    }
}
