using UnityEngine;
using System.Collections;

public class RandomEncounterSystem : MonoBehaviour
{
    [Header("Encounter Settings")]
    [Range(0f, 100f)]
    public float encounterChance = 20f;
    public float checkInterval = 2f;

    [Header("Requirements")]
    public float minDistanceToTrigger = 1f;

    [Header("Enemy Setup")]
    public GameObject[] possibleEnemies;
    public int minEnemies = 1;
    public int maxEnemies = 3;

    private Transform player;
    private Vector3 lastPosition;
    private float timeSinceLastCheck = 0f;
    private bool encounterActive = false;

    void Start()
    {
        player = transform;
        lastPosition = player.position;
    }

    void Update()
    {
        // CRITICAL: Check if GameStateManager exists FIRST!
        if (GameStateManager.Instance == null)
        {
            return; // Exit immediately if no GameStateManager
        }

        // Don't check during battles
        if (encounterActive) return;
        if (GameStateManager.Instance.GetCurrentState() == GameStateManager.GameState.Battle) return;

        // Only check in dungeons
        GameStateManager.GameState currentState = GameStateManager.Instance.GetCurrentState();
        if (currentState != GameStateManager.GameState.Dungeon1 &&
            currentState != GameStateManager.GameState.Dungeon2 &&
            currentState != GameStateManager.GameState.Dungeon3)
        {
            return;
        }

        timeSinceLastCheck += Time.deltaTime;

        if (timeSinceLastCheck >= checkInterval)
        {
            float distanceMoved = Vector3.Distance(player.position, lastPosition);

            if (distanceMoved >= minDistanceToTrigger)
            {
                CheckForEncounter();
            }

            lastPosition = player.position;
            timeSinceLastCheck = 0f;
        }
    }

    void CheckForEncounter()
    {
        float roll = Random.Range(0f, 100f);

        if (roll <= encounterChance)
        {
            Debug.Log($"Random encounter! (Rolled {roll}, needed {encounterChance})");
            TriggerEncounter();
        }
    }

    void TriggerEncounter()
    {
        encounterActive = true;
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        // Freeze player
        MonoBehaviour[] playerScripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in playerScripts)
        {
            if (script != this && script != null)
            {
                script.enabled = false;
            }
        }

        Debug.Log("Battle starting!");
        yield return new WaitForSeconds(0.5f);

        // Switch to battle
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SwitchState(GameStateManager.GameState.Battle);
        }

        yield return new WaitForSeconds(0.3f);

        // Spawn random enemies
        if (BattleManager.Instance != null)
        {
            int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
            BattleManager.Instance.StartRandomBattle(possibleEnemies, enemyCount);
        }
    }

    public void OnBattleEnd()
    {
        encounterActive = false;

        // Re-enable player scripts
        MonoBehaviour[] playerScripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in playerScripts)
        {
            if (script != this && script != null)
            {
                script.enabled = true;
            }
        }
    }
}