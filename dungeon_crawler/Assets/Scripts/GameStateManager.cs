using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    
    public enum GameState
    {
        WorldMap,
        TownExploration,
        DungeonExploration1,
        DungeonExploration2,
        DungeonExploration3,
        Battle
    }
    
    [Header("State Objects")]
    public GameObject worldMapObjects;
    public GameObject townExplorationObjects;
    public GameObject dungeon1Objects;
    public GameObject dungeon2Objects;
    public GameObject dungeon3Objects;
    public GameObject battleObjects;
    
    [Header("Backgrounds")]
    public SpriteRenderer backgroundRenderer;
    public Sprite worldMapBackground;
    public Sprite townBackground;
    public Sprite dungeon1Background;
    public Sprite dungeon2Background;
    public Sprite dungeon3Background;
    public Sprite battleBackground;
    
    [Header("Camera")]
    public Camera mainCamera;
    public Vector3 worldMapCameraPos = new Vector3(0, 0, -10);
    public Vector3 townCameraPos = new Vector3(0, 0, -10);
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
        SwitchState(GameState.WorldMap);
    }
    
    public void SwitchState(GameState newState)
    {
        currentState = newState;
        
        // Disable all states
        worldMapObjects.SetActive(false);
        townExplorationObjects.SetActive(false);
        dungeon1Objects.SetActive(false);
        dungeon2Objects.SetActive(false);
        dungeon3Objects.SetActive(false);
        battleObjects.SetActive(false);
        
        // Enable and setup current state
        switch (currentState)
        {
            case GameState.WorldMap:
                worldMapObjects.SetActive(true);
                backgroundRenderer.sprite = worldMapBackground;
                mainCamera.transform.position = worldMapCameraPos;
                break;
                
            case GameState.TownExploration:
                townExplorationObjects.SetActive(true);
                backgroundRenderer.sprite = townBackground;
                mainCamera.transform.position = townCameraPos;
                break;
                
            case GameState.DungeonExploration1:
                dungeon1Objects.SetActive(true);
                backgroundRenderer.sprite = dungeon1Background;
                mainCamera.transform.position = dungeon1CameraPos;
                break;
                
            case GameState.DungeonExploration2:
                dungeon2Objects.SetActive(true);
                backgroundRenderer.sprite = dungeon2Background;
                mainCamera.transform.position = dungeon2CameraPos;
                break;
                
            case GameState.DungeonExploration3:
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