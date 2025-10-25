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
        // Only move if in an exploration state
        GameStateManager.GameState state = GameStateManager.Instance.GetCurrentState();

        if (state != GameStateManager.GameState.TownMap &&
            state != GameStateManager.GameState.Town1 &&
            state != GameStateManager.GameState.Town2 &&
            state != GameStateManager.GameState.Town3 &&
            state != GameStateManager.GameState.DungeonMap &&
            state != GameStateManager.GameState.Dungeon1 &&
            state != GameStateManager.GameState.Dungeon2 &&
            state != GameStateManager.GameState.Dungeon3)
        {
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        GameStateManager.GameState state = GameStateManager.Instance.GetCurrentState();

        if (state == GameStateManager.GameState.TownMap ||
            state == GameStateManager.GameState.Town1 ||
            state == GameStateManager.GameState.Town2 ||
            state == GameStateManager.GameState.Town3 ||
            state == GameStateManager.GameState.DungeonMap ||
            state == GameStateManager.GameState.Dungeon1 ||
            state == GameStateManager.GameState.Dungeon2 ||
            state == GameStateManager.GameState.Dungeon3)
        {
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }
}