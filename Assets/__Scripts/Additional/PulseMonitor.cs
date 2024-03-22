using UnityEngine;

public class PulseMonitor : Interactable
{
    [SerializeField] Animator animator;
    [SerializeField] Transform vitalsUI;
    bool once;
    public override void Interact(Interactor caller)
    {
        caller.FreezeInput();
        caller.GetComponent<PlayerMovement>().FreezeInput();
        vitalsUI.gameObject.SetActive(true);
        if (once)
        {
            return;
        }
        once = true;
        base.Interact(caller);
    }
    public void SetPulseSpeed()
    {
        if (animator.speed == 3)
            animator.speed = 1;
        else
            animator.speed = 3;
    }
}
