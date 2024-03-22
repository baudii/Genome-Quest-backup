using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour, IFreezeInput
{
    public CinemachineVirtualCamera currentVcam;
    List<Interactable> interactables;
    Interactable currentInteractable;
    bool inputFrozen;
    private void Awake()
    {
        interactables = new List<Interactable>();
    }

    void Update()
    {
        if (inputFrozen)
            return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }
    public void AddInteractable(Interactable interactable, bool interactImediate = false)
    {
        interactables.Add(interactable);
        if (interactImediate)
        {
            currentInteractable = interactable;
            Interact();
            return;
        }
        SetCurrentInteractable();
    }

    public void RemoveInteractable(Interactable interactable)
    {
        if (interactables.Contains(interactable))
            interactables.Remove(interactable);
        SetCurrentInteractable();
    }

    void SetCurrentInteractable()
    {
        currentInteractable?.StopHighLight();
        float dist = -100;
        foreach (var interactable in interactables)
        {
            var distTemp = Vector2.Distance(transform.position, interactable.transform.position);
            if (Mathf.Abs(dist) > distTemp)
            {
                currentInteractable = interactable;
                dist = distTemp;
            }
        }
        if (dist < 0)
            currentInteractable = null;
        currentInteractable?.HighLight();
    }
    public static bool toggled = false;
    public void Interact()
    {
        if (currentInteractable != null)
        {
            if (currentInteractable is PulseMonitor && !toggled)
            {
                toggled = true;
                Objective.Instance.ObjectiveGoToDesk();
            }
            else if (toggled && currentInteractable.name == "Desk")
            {
                Objective.Instance.ObjectiveAssignTreatment();
            }

            currentInteractable.InteractWith(this);
        }
    }
/*    public void TryInteract(Interactable interactable)
    {
        if (Vector2.Distance(interactable.transform.position, transform.position) <= minInteractDistance)
        {
            interactable.Interact(this);
        }
    }*/

    public void FreezeInput()
    {
        inputFrozen = true;
    }

    public void UnfreezeInput()
    {
        inputFrozen = false;
    }
}
