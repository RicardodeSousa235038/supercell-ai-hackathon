using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        // Only move if in exploration state
        if (GameStateManager.Instance.GetCurrentState() != GameStateManager.GameState.TownExploration)
        {
            return;
        }
        
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
    
    void FixedUpdate()
    {
        if (GameStateManager.Instance.GetCurrentState() == GameStateManager.GameState.TownExploration)
        {
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }
}