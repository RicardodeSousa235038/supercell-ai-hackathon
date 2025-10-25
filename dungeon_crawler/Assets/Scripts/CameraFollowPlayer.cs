using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform townPlayer;
    public Transform dungeon1Player;
    public Transform dungeon2Player;
    public Transform dungeon3Player;

    public float smoothSpeed = 0.125f;
    public Vector2 minBounds = new Vector2(-20, -15);
    public Vector2 maxBounds = new Vector2(20, 15);

    void LateUpdate()
    {
        // Get current active player based on state
        Transform currentPlayer = null;

        GameStateManager.GameState state = GameStateManager.Instance.GetCurrentState();

        switch (state)
        {
            case GameStateManager.GameState.TownExploration:
                currentPlayer = townPlayer;
                break;
            case GameStateManager.GameState.DungeonExploration1:
                currentPlayer = dungeon1Player;
                break;
            case GameStateManager.GameState.DungeonExploration2:
                currentPlayer = dungeon2Player;
                break;
            case GameStateManager.GameState.DungeonExploration3:
                currentPlayer = dungeon3Player;
                break;
            default:
                return; // Don't follow in other states
        }

        if (currentPlayer == null) return;

        Vector3 targetPosition = new Vector3(currentPlayer.position.x, currentPlayer.position.y, transform.position.z);

        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}