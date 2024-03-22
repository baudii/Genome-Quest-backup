using UnityEngine;

public class Speaker : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void StartAnimation()
    {
        animator.SetBool("Play", true);
    }
    public void StopAnimation()
    {
        animator.SetBool("Play", false);
    }
}
