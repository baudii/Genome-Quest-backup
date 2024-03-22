using UnityEngine;

public class PlayerMovement : MonoBehaviour, IFreezeInput
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sr;
    Vector2 input;
    bool inputFrozen;
    Vector2 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    public void SetInitialPosition()
    {
        print("setting");
        transform.position = initialPosition;
        SetFlipState(-1);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            moveSpeed *= 3;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
            moveSpeed /= 3;
#endif
        if (!inputFrozen)
            input.x = Input.GetAxisRaw("Horizontal");

        SetFlipState(input.x);
        animator.SetBool("isMoving", input.x != 0);
    }
    private void FixedUpdate()
    {
        rb.velocity = input * moveSpeed * Time.deltaTime;
    }

    public void SetInput(float inputX)
    {
        input = new Vector2(inputX, 0);
    }

    public void SetFlipState(float inputX)
    {
        if (inputX < 0)
        {
            sr.flipX = true;
        }
        else if (inputX > 0)
        {
            sr.flipX = false;
        }
    }

    public void FreezeInput()
    {
        inputFrozen = true;
        input = Vector2.zero;
    }

    public void UnfreezeInput()
    {
        inputFrozen = false;
    }
}
