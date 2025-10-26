using UnityEngine;

public class PlayerCharacterLoader : MonoBehaviour
{
    [Header("Character Sprites")]
    [SerializeField] private Sprite fighterSprite;
    [SerializeField] private Sprite knightSprite;
    [SerializeField] private Sprite thiefSprite;
    [SerializeField] private Sprite beastSprite;
    [SerializeField] private Sprite vampireSprite;
    [SerializeField] private Sprite archerSprite;
    
    [Header("Player References")]
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    public static int PlayerHealth { get; private set; }
    public static int PlayerDamage { get; private set; }
    public static int PlayerSpeed { get; private set; }
    public static string PlayerClass { get; private set; }
    
    void Start()
    {
        LoadSelectedCharacter();
    }
    
    void LoadSelectedCharacter()
    {
        string selectedChar = PlayerPrefs.GetString("SelectedCharacter", "Fighter");
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        
        PlayerClass = selectedChar;
        
        Debug.Log($"Loading character: {selectedChar} (Index: {selectedIndex})");
        
        switch (selectedIndex)
        {
            case 0:
                ApplyCharacter(fighterSprite, "Fighter", 100, 15, 2);
                break;
            
            case 1:
                ApplyCharacter(knightSprite, "Knight", 90, 18, 2);
                break;
            
            case 2:
                ApplyCharacter(thiefSprite, "Thief", 80, 20, 4);
                break;
            
            case 3:
                ApplyCharacter(beastSprite, "Beast", 120, 12, 3);
                break;
            
            case 4:
                ApplyCharacter(vampireSprite, "Vampire", 70, 22, 3);
                break;
            
            case 5:
                ApplyCharacter(archerSprite, "Archer", 75, 19, 3);
                break;
            
            default:
                Debug.LogWarning($"Unknown character index: {selectedIndex}. Loading Fighter.");
                ApplyCharacter(fighterSprite, "Fighter", 100, 15, 2);
                break;
        }
    }
    
    void ApplyCharacter(Sprite sprite, string className, int health, int damage, int speed)
    {
        PlayerClass = className;
        PlayerHealth = health;
        PlayerDamage = damage;
        PlayerSpeed = speed;
        
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sprite = sprite;
            Debug.Log($"Applied {className} sprite to player");
        }
        else
        {
            Debug.LogWarning("Player SpriteRenderer not assigned!");
        }
        
        Debug.Log($"Character loaded: {className} - HP:{health}, DMG:{damage}, SPD:{speed}");
    }
}
