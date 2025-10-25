using UnityEngine;

public class CarriageMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    public GameStateManager.GameState destinationState = GameStateManager.GameState.TownMap;

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
                // Switch to the destination state
                GameStateManager.Instance.SwitchState(destinationState);
            }
        }
    }
}