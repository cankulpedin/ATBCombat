using UnityEngine;

public class PlayerController : MonoBehaviour, IDialogueObserver
{
    private float Speed = 5f;

    private Rigidbody2D rb;
    private bool playerCanMove = true;

    void Start()
    {
        FindFirstObjectByType<DialogueManager>().RegisterObserver(this);

        rb = GetComponent<Rigidbody2D>();
    }

    public void NotifyDialogueStarted()
    {
        playerCanMove = false;
    }

    public void NotifyDialogueEnded()
    {
        playerCanMove = true;
    }

    void Update()
    {
        if(playerCanMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(horizontalInput, verticalInput);
            movement.Normalize();

            rb.velocity = new Vector2(movement.x * Speed, movement.y * Speed);
        }
    }
}
