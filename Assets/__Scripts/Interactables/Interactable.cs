using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Interactable : MonoBehaviour
{
    //[SerializeField] bool disableKeyboardInputs;
    [SerializeField] public bool interactImediate;
    [SerializeField] public UnityEvent OnInteract;
    [SerializeField] UnityEvent Highlight;
    [SerializeField] UnityEvent StopHighlight;
    bool disableHighlight;
    public bool notInteractable;


    private void Start()
    {
        StopHighlight?.Invoke();
    }

/*    private void Update()
    {
        if (!disableKeyboardInputs)
            if (Input.GetKeyDown(KeyCode.DownArrow))

    }*/

    public void InteractWith(Interactor caller)
    {
        if (notInteractable)
            return;
        Interact(caller);
    }
    public void SetInteractable(bool value)
    {
        notInteractable = !value;
    }

    public virtual void Interact(Interactor caller) 
    {
        StopHighLight();
        OnInteract?.Invoke();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Interactor interactor))
        {
            interactor.AddInteractable(this, interactImediate);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        disableHighlight = false;
        if (collision.transform.TryGetComponent(out Interactor interactor))
        {
            interactor.RemoveInteractable(this);
        }
    }

    public void DisableHighlightUntilExit()
    {
        disableHighlight = true;
    }

    public virtual void HighLight()
    {
        if (disableHighlight || notInteractable)
            return;

        Highlight?.Invoke();
    }

    public virtual void StopHighLight()
    {
        StopHighlight?.Invoke();
    }
}
