using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HydraBattleManager : MonoBehaviour
{
    public static HydraBattleManager Instance;

    [Header("Hydra Setup")]
    public GameObject hydraPrefab;

    [Header("Spawn Positions")]
    public Transform hydraSpawn1;
    public Transform hydraSpawn2;
    public Transform hydraSpawn3;

    [Header("Battle Settings")]
    public float spawnDelay = 1f;

    private List<GameObject> activeHydras = new List<GameObject>();
    private bool battleActive = false;

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

    public void StartHydraBattle(int hydraCount)
    {
        if (battleActive) return;

        battleActive = true;
        StartCoroutine(SpawnHydras(hydraCount));
    }

    IEnumerator SpawnHydras(int count)
    {
        Transform[] spawnPositions = { hydraSpawn1, hydraSpawn2, hydraSpawn3 };

        for (int i = 0; i < count && i < spawnPositions.Length; i++)
        {
            if (spawnPositions[i] != null && hydraPrefab != null)
            {
                GameObject hydra = Instantiate(hydraPrefab, spawnPositions[i].position, Quaternion.identity);
                hydra.transform.SetParent(transform); // Keep it organized
                activeHydras.Add(hydra);

                Debug.Log($"Hydra {i + 1} spawned!");

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        Debug.Log("All hydras spawned! Battle begins!");
    }

    public void OnHydraDefeated(GameObject hydra)
    {
        activeHydras.Remove(hydra);
        Destroy(hydra);

        Debug.Log($"Hydra defeated! {activeHydras.Count} remaining.");

        if (activeHydras.Count == 0)
        {
            OnBattleComplete();
        }
    }

    void OnBattleComplete()
    {
        battleActive = false;
        Debug.Log("All hydras defeated! VICTORY!");

        // You can add your victory logic here
        // For now, maybe just return to dungeon
        StartCoroutine(ReturnToDungeon());
    }

    IEnumerator ReturnToDungeon()
    {
        yield return new WaitForSeconds(2f);

        // Return to Dungeon3
        GameStateManager.Instance.SwitchState(GameStateManager.GameState.Dungeon3);
    }
}