using UnityEngine;

public class AreaTransition : MonoBehaviour
{
    public GameStateManager.GameState targetArea;
    public Vector3 spawnPosition = Vector3.zero;  // Where to spawn the player/carriage

    private bool isTransitioning = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = true;
            Debug.Log("Entering: " + targetArea);

            // Move the player/carriage to spawn position
            if (spawnPosition != Vector3.zero)
            {
                other.transform.position = spawnPosition;
            }

            GameStateManager.Instance.SwitchState(targetArea);

            // Reset flag after a short delay
            Invoke("ResetTransition", 0.5f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTransitioning = false;
        }
    }

    void ResetTransition()
    {
        isTransitioning = false;
    }
}