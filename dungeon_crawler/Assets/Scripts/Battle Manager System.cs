using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("Battle Setup")]
    public Transform playerBattlePosition;
    public Transform[] enemySpawnPositions;

    [Header("Player Reference")]
    public GameObject playerBattlePrefab;

    [Header("UI References")]
    public GameObject battleUI;
    public UnityEngine.UI.Text playerHealthText;
    public UnityEngine.UI.Text enemyHealthText;
    public UnityEngine.UI.Text battleLogText;

    private GameObject playerBattleInstance;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private BattleUnit playerUnit;
    private BattleUnit currentEnemyTarget;
    private bool battleActive = false;
    private GameStateManager.GameState returnState;  // Remember where to return after battle

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (battleUI != null)
        {
            battleUI.SetActive(false);
        }
    }

    public void StartRandomBattle(GameObject[] possibleEnemies, int enemyCount)
    {
        if (battleActive) return;

        battleActive = true;

        // Remember which state to return to
        returnState = GameStateManager.Instance.GetCurrentState();

        StartCoroutine(SetupBattle(possibleEnemies, enemyCount));
    }

    public void StartHydraBattle(GameObject hydraPrefab, int hydraCount)
    {
        if (battleActive) return;

        battleActive = true;

        // For boss battles, return to dungeon 3
        returnState = GameStateManager.GameState.Dungeon3;

        // Create array with just hydras
        GameObject[] hydras = new GameObject[hydraCount];
        for (int i = 0; i < hydraCount; i++)
        {
            hydras[i] = hydraPrefab;
        }

        StartCoroutine(SetupBattle(hydras, hydraCount));
    }

    IEnumerator SetupBattle(GameObject[] possibleEnemies, int enemyCount)
    {
        // Spawn player
        if (playerBattlePrefab != null && playerBattlePosition != null)
        {
            playerBattleInstance = Instantiate(playerBattlePrefab, playerBattlePosition.position, Quaternion.identity);
            playerBattleInstance.transform.SetParent(transform);
            playerUnit = playerBattleInstance.GetComponent<BattleUnit>();
        }

        // Spawn enemies
        for (int i = 0; i < enemyCount && i < enemySpawnPositions.Length; i++)
        {
            if (possibleEnemies.Length > 0)
            {
                GameObject enemyPrefab = possibleEnemies[Random.Range(0, possibleEnemies.Length)];
                GameObject enemy = Instantiate(enemyPrefab, enemySpawnPositions[i].position, Quaternion.identity);
                enemy.transform.SetParent(transform);
                activeEnemies.Add(enemy);

                yield return new WaitForSeconds(0.3f);
            }
        }

        // Show battle UI
        if (battleUI != null)
        {
            battleUI.SetActive(true);
        }

        // Set first target
        if (activeEnemies.Count > 0)
        {
            currentEnemyTarget = activeEnemies[0].GetComponent<BattleUnit>();
        }

        UpdateUI();
        AddBattleLog("Battle Start!");
    }

    public void PlayerAttack()
    {
        if (!battleActive || playerUnit == null || currentEnemyTarget == null) return;

        int damage = playerUnit.Attack();
        currentEnemyTarget.TakeDamage(damage);

        AddBattleLog($"You dealt {damage} damage!");
        UpdateUI();

        if (currentEnemyTarget.IsDead())
        {
            StartCoroutine(OnEnemyDefeated());
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    public void PlayerDefend()
    {
        if (!battleActive || playerUnit == null) return;

        playerUnit.Defend();
        AddBattleLog("You brace for impact!");
        UpdateUI();

        StartCoroutine(EnemyTurn());
    }

    public void PlayerUseItem()
    {
        if (!battleActive || playerUnit == null) return;

        // Simple heal for now (you can expand this)
        int healAmount = 30;
        playerUnit.Heal(healAmount);
        AddBattleLog($"You healed {healAmount} HP!");
        UpdateUI();

        StartCoroutine(EnemyTurn());
    }

    public void PlayerFlee()
    {
        float fleeChance = Random.Range(0f, 100f);

        if (fleeChance > 50f)
        {
            AddBattleLog("You escaped!");
            StartCoroutine(EndBattle(true, true));  // Won = true, fled = true
        }
        else
        {
            AddBattleLog("Can't escape!");
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        // All enemies attack
        foreach (GameObject enemyObj in activeEnemies)
        {
            BattleUnit enemy = enemyObj.GetComponent<BattleUnit>();
            if (enemy != null && !enemy.IsDead())
            {
                int damage = enemy.Attack();
                playerUnit.TakeDamage(damage);

                AddBattleLog($"{enemy.unitName} dealt {damage} damage!");
                UpdateUI();

                yield return new WaitForSeconds(0.5f);

                if (playerUnit.IsDead())
                {
                    StartCoroutine(OnPlayerDefeated());
                    yield break;
                }
            }
        }

        // Reset player defense
        playerUnit.ResetDefense();
    }

    IEnumerator OnEnemyDefeated()
    {
        AddBattleLog($"{currentEnemyTarget.unitName} defeated!");

        activeEnemies.Remove(currentEnemyTarget.gameObject);
        Destroy(currentEnemyTarget.gameObject);

        yield return new WaitForSeconds(1f);

        if (activeEnemies.Count == 0)
        {
            AddBattleLog("Victory!");
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(EndBattle(true, false));  // Won = true, fled = false
        }
        else
        {
            // Target next enemy
            currentEnemyTarget = activeEnemies[0].GetComponent<BattleUnit>();
            UpdateUI();
        }
    }

    IEnumerator OnPlayerDefeated()
    {
        AddBattleLog("You were defeated!");
        yield return new WaitForSeconds(2f);

        StartCoroutine(EndBattle(false, false));  // Won = false, fled = false
    }

    IEnumerator EndBattle(bool playerWon, bool fled)
    {
        battleActive = false;

        // Clean up
        if (playerBattleInstance != null)
        {
            Destroy(playerBattleInstance);
        }

        foreach (GameObject enemy in activeEnemies)
        {
            Destroy(enemy);
        }
        activeEnemies.Clear();

        if (battleUI != null)
        {
            battleUI.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        // Return to previous state
        GameStateManager.Instance.SwitchState(returnState);

        // Notify random encounter system
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            RandomEncounterSystem encounterSystem = player.GetComponent<RandomEncounterSystem>();
            if (encounterSystem != null)
            {
                encounterSystem.OnBattleEnd();
            }
        }
    }

    void UpdateUI()
    {
        if (playerUnit != null && playerHealthText != null)
        {
            playerHealthText.text = $"HP: {playerUnit.currentHealth}/{playerUnit.maxHealth}";
        }

        if (currentEnemyTarget != null && enemyHealthText != null)
        {
            enemyHealthText.text = $"{currentEnemyTarget.unitName}\nHP: {currentEnemyTarget.currentHealth}/{currentEnemyTarget.maxHealth}";
        }
    }

    void AddBattleLog(string message)
    {
        if (battleLogText != null)
        {
            battleLogText.text = message + "\n" + battleLogText.text;

            // Keep only last 5 messages
            string[] lines = battleLogText.text.Split('\n');
            if (lines.Length > 5)
            {
                battleLogText.text = string.Join("\n", lines, 0, 5);
            }
        }

        Debug.Log($"[Battle] {message}");
    }
}