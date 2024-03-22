using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDialogueController : MonoBehaviour
{
    List<IFreezeInput> freezeInputs;

    private void Start()
    {
        freezeInputs = GetComponents<IFreezeInput>().ToList();
    }

    public void InitiateDialogue()
    {
        foreach (var freezeInput in freezeInputs)
        {
            freezeInput.FreezeInput();
        }
    }

    public void StopDialogue()
    {
        foreach(var freezeInput in freezeInputs)
        {
            freezeInput.UnfreezeInput();
        }
    }
}
