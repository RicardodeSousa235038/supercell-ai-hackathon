using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector2 minBounds = new Vector2(-20, -15);
    public Vector2 maxBounds = new Vector2(20, 15);
    
    void LateUpdate()
    {
        // Only follow in exploration mode
        if (GameStateManager.Instance.GetCurrentState() != GameStateManager.GameState.TownExploration)
        {
            return;
        }
        
        if (player == null) return;
        
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
        
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}