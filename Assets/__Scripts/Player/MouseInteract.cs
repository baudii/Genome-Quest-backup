using System;
using UnityEngine;

public class MouseInteract : MonoBehaviour, IFreezeInput
{
    [SerializeField] float mouseDetectionRadius;
    bool frozen;
    Camera mainCam;
    Interactable currentInteractable;
    Interactor interactor;
    private void Awake()
    {
        mainCam = Camera.main;
        interactor = GetComponent<Interactor>();
    }
    public void FreezeInput()
    {
        frozen = true;
    }

    public void UnfreezeInput()
    {
        frozen = false;
    }

    private void Update()
    {
        if (frozen)
            return;

        var pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        HighLight(pos);
    }

    void HighLight(Vector2 pos)
    {
        var colliders = Physics2D.OverlapCircleAll(pos, mouseDetectionRadius);
        foreach (var coll in colliders)
        {
            if (coll.TryGetComponent(out Interactable interactible))
            {
                if (currentInteractable != null)
                    currentInteractable.StopHighLight();
                interactible.HighLight();
                currentInteractable = interactible;
            }
        }
        if (colliders.Length == 0 && currentInteractable != null)
        {
            currentInteractable.StopHighLight();
            currentInteractable = null;
        }
    }
}
