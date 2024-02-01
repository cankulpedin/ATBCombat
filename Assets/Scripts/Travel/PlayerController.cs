using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float Speed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize();

        rb.velocity = new Vector2(movement.x * Speed, movement.y * Speed);

    }
}
