using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Door : Interactable
{
    [Header("Door")]
    [SerializeField] Transform destination;
    [SerializeField] CinemachineVirtualCamera destinationVcam;
    [SerializeField] public UnityEvent OnMidTransition;
    [SerializeField] public UnityEvent OnTransitionComplete;
    [SerializeField] Door connectedDoor;
    public override void Interact(Interactor caller)
    {
        IFreezeInput[] inputs = caller.GetComponents<IFreezeInput>();
        foreach (var input in inputs)
        {
            input.FreezeInput();
        }
        if (connectedDoor != null)
        {
            connectedDoor.DisableHighlightUntilExit();
        }
        base.Interact(caller);
        StartCoroutine(TransitionScene(caller));
    }

    IEnumerator TransitionScene(Interactor caller)
    {
        yield return new WaitForSeconds(0.5f);
        if (destination != null)
            caller.transform.position = destination.position;
        destinationVcam.Priority = 10;
        caller.currentVcam.Priority = 0;
        caller.currentVcam = destinationVcam;
        OnMidTransition?.Invoke();

        yield return new WaitForSeconds(0.5f);

        IFreezeInput[] inputs = caller.GetComponents<IFreezeInput>();
        foreach (var input in inputs)
        {
            input.UnfreezeInput();
        }
        OnTransitionComplete?.Invoke();
    }
}
