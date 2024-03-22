using UnityEngine;

public class NPC : Interactable
{
    [SerializeField] Transform player;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] SpriteRenderer hlSr;
    [SerializeField] bool dontLookAtPlayer;
    /*    [SerializeField] Animator animator;
        public override void HighLight()
        {
            base.HighLight();
            animator.SetBool("HL", true);
        }

        public override void StopHighLight()
        {
            base.StopHighLight();
            animator.SetBool("HL", false);
        }*/

    private void FixedUpdate()
    {
        if (dontLookAtPlayer)
        {
            return;
        }
        if (Vector2.Distance(transform.position, player.position) < 0.5f)
        {
            sr.flipX = player.position.x - transform.position.x < 0;
            hlSr.flipX = player.position.x - transform.position.x < 0;
        }
    }

    public void DisableInteractImediate()
    {
        interactImediate = false;
    }
}
