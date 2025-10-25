using UnityEngine;

public class CarriageMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    private int currentWaypointIndex = 0;
    
    void Update()
    {
        if (waypoints.Length == 0) return;
        
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, 
                                                  targetWaypoint.position, 
                                                  speed * Time.deltaTime);
        
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;
            
            if (currentWaypointIndex >= waypoints.Length)
            {
                // Switch to town exploration
                GameStateManager.Instance.SwitchState(GameStateManager.GameState.TownExploration);
            }
        }
    }
}