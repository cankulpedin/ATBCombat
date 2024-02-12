using UnityEngine;

public class PlayerController : MonoBehaviour, IPauseObserver
{
    private const float initialSpeed = 5f;
    private float Speed = initialSpeed;

    private Rigidbody2D rb;
    private bool playerCanMove = true;


    void Start()
    {
        FindFirstObjectByType<GameManager>().RegisterObserver(this);

        rb = GetComponent<Rigidbody2D>();
    }

    public void NotifyPause()
    {
        playerCanMove = false;
        
    }

    public void NotifyUnpause()
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
        } else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
