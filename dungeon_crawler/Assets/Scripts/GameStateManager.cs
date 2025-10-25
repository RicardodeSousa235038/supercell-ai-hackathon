using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public enum GameState
    {
        TownMap,
        Town1,
        Town2,
        Town3,
        DungeonMap,
        Dungeon1,
        Dungeon2,
        Dungeon3,
        Battle
    }

    [Header("State Objects")]
    public GameObject townMapObjects;
    public GameObject town1Objects;
    public GameObject town2Objects;
    public GameObject town3Objects;
    public GameObject dungeonMapObjects;
    public GameObject dungeon1Objects;
    public GameObject dungeon2Objects;
    public GameObject dungeon3Objects;
    public GameObject battleObjects;

    [Header("Backgrounds")]
    public SpriteRenderer backgroundRenderer;
    public Sprite townMapBackground;
    public Sprite town1Background;
    public Sprite town2Background;
    public Sprite town3Background;
    public Sprite dungeonMapBackground;
    public Sprite dungeon1Background;
    public Sprite dungeon2Background;
    public Sprite dungeon3Background;
    public Sprite battleBackground;

    [Header("Camera")]
    public Camera mainCamera;
    public Vector3 townMapCameraPos = new Vector3(0, 0, -10);
    public Vector3 town1CameraPos = new Vector3(0, 0, -10);
    public Vector3 town2CameraPos = new Vector3(0, 0, -10);
    public Vector3 town3CameraPos = new Vector3(0, 0, -10);
    public Vector3 dungeonMapCameraPos = new Vector3(0, 0, -10);
    public Vector3 dungeon1CameraPos = new Vector3(0, 0, -10);
    public Vector3 dungeon2CameraPos = new Vector3(0, 0, -10);
    public Vector3 dungeon3CameraPos = new Vector3(0, 0, -10);
    public Vector3 battleCameraPos = new Vector3(0, 0, -10);

    private GameState currentState;

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
        SwitchState(GameState.TownMap);  // Start with Town Map instead
    }

    public void SwitchState(GameState newState)
    {
        currentState = newState;

        // Disable all states
        townMapObjects.SetActive(false);
        town1Objects.SetActive(false);
        town2Objects.SetActive(false);
        town3Objects.SetActive(false);
        dungeonMapObjects.SetActive(false);
        dungeon1Objects.SetActive(false);
        dungeon2Objects.SetActive(false);
        dungeon3Objects.SetActive(false);
        battleObjects.SetActive(false);

        // Enable and setup current state
        switch (currentState)
        {
            case GameState.TownMap:
                townMapObjects.SetActive(true);
                backgroundRenderer.sprite = townMapBackground;
                mainCamera.transform.position = townMapCameraPos;
                break;

            case GameState.Town1:
                town1Objects.SetActive(true);
                backgroundRenderer.sprite = town1Background;
                mainCamera.transform.position = town1CameraPos;
                break;

            case GameState.Town2:
                town2Objects.SetActive(true);
                backgroundRenderer.sprite = town2Background;
                mainCamera.transform.position = town2CameraPos;
                break;

            case GameState.Town3:
                town3Objects.SetActive(true);
                backgroundRenderer.sprite = town3Background;
                mainCamera.transform.position = town3CameraPos;
                break;

            case GameState.DungeonMap:
                dungeonMapObjects.SetActive(true);
                backgroundRenderer.sprite = dungeonMapBackground;
                mainCamera.transform.position = dungeonMapCameraPos;
                break;

            case GameState.Dungeon1:
                dungeon1Objects.SetActive(true);
                backgroundRenderer.sprite = dungeon1Background;
                mainCamera.transform.position = dungeon1CameraPos;
                break;

            case GameState.Dungeon2:
                dungeon2Objects.SetActive(true);
                backgroundRenderer.sprite = dungeon2Background;
                mainCamera.transform.position = dungeon2CameraPos;
                break;

            case GameState.Dungeon3:
                dungeon3Objects.SetActive(true);
                backgroundRenderer.sprite = dungeon3Background;
                mainCamera.transform.position = dungeon3CameraPos;
                break;

            case GameState.Battle:
                battleObjects.SetActive(true);
                backgroundRenderer.sprite = battleBackground;
                mainCamera.transform.position = battleCameraPos;
                break;
        }
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }
}