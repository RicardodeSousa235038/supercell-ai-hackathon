using UnityEngine;

public class RandomEncounter : MonoBehaviour
{
    public float encounterChance = 0.05f;
    public LayerMask grassLayer;
    
    private float stepCounter = 0f;
    private float stepThreshold = 0.5f;
    private Vector2 lastPosition;
    
    void Start()
    {
        lastPosition = transform.position;
    }
    
    void Update()
    {
        // Only check in exploration mode
        if (GameStateManager.Instance.GetCurrentState() != GameStateManager.GameState.TownExploration)
        {
            return;
        }
        
        float distanceMoved = Vector2.Distance(transform.position, lastPosition);
        
        if (distanceMoved > stepThreshold)
        {
            stepCounter++;
            lastPosition = transform.position;
            CheckForEncounter();
        }
    }
    
    void CheckForEncounter()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 0f, grassLayer);
        
        if (hit.collider != null)
        {
            if (Random.value < encounterChance)
            {
                TriggerBattle();
            }
        }
    }
    
    void TriggerBattle()
    {
        Debug.Log("Battle triggered!");
        GameStateManager.Instance.SwitchState(GameStateManager.GameState.Battle);
    }
}