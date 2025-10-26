using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    [Header("Available Character Sprites")]
    public Sprite fighter;      // Index 0
    public Sprite knight;       // Index 1
    public Sprite thief;        // Index 2
    public Sprite beastClass;   // Index 3
    public Sprite vampire;      // Index 4
    public Sprite archer;       // Index 5

    [Header("Currently Selected Character")]
    public string selectedCharacterClass = "Fighter";
    public int selectedCharacterIndex = 0;

    [Header("Character Stats")]
    public int playerHealth = 100;
    public int playerMaxHealth = 100;
    public int playerDamage = 15;
    public int playerSpeed = 2;
    public int playerDefense = 5;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("CharacterManager created and set to DontDestroyOnLoad");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Duplicate CharacterManager destroyed");
        }
    }

    void Start()
    {
        LoadSavedCharacter();
    }

    // Call this when player selects a character
    public void SelectCharacter(int characterIndex)
    {
        selectedCharacterIndex = characterIndex;

        switch (characterIndex)
        {
            case 0: // Fighter
                ApplyCharacterStats("Fighter", 100, 15, 2, 5);
                break;

            case 1: // Knight
                ApplyCharacterStats("Knight", 120, 18, 2, 8);
                break;

            case 2: // Thief
                ApplyCharacterStats("Thief", 80, 20, 4, 3);
                break;

            case 3: // Beast Class
                ApplyCharacterStats("Beast Class", 150, 12, 3, 6);
                break;

            case 4: // Vampire
                ApplyCharacterStats("Vampire", 90, 22, 3, 4);
                break;

            case 5: // Archer
                ApplyCharacterStats("Archer", 75, 19, 3, 4);
                break;

            default:
                ApplyCharacterStats("Fighter", 100, 15, 2, 5);
                break;
        }

        SaveCharacterSelection();
        Debug.Log($"Character selected: Index {characterIndex} = {selectedCharacterClass}");
    }

    void ApplyCharacterStats(string className, int health, int damage, int speed, int defense)
    {
        selectedCharacterClass = className;
        playerMaxHealth = health;
        playerHealth = health;
        playerDamage = damage;
        playerSpeed = speed;
        playerDefense = defense;
    }

    public Sprite GetSelectedCharacterSprite()
    {
        Debug.Log($"Getting sprite for index {selectedCharacterIndex}");

        switch (selectedCharacterIndex)
        {
            case 0: return fighter;
            case 1: return knight;
            case 2: return thief;
            case 3: return beastClass;
            case 4: return vampire;
            case 5: return archer;
            default:
                Debug.LogWarning($"Unknown character index: {selectedCharacterIndex}, returning fighter");
                return fighter;
        }
    }

    // Save character selection
    public void SaveCharacterSelection()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
        PlayerPrefs.SetString("SelectedCharacter", selectedCharacterClass);
        PlayerPrefs.SetInt("PlayerMaxHealth", playerMaxHealth);
        PlayerPrefs.SetInt("PlayerHealth", playerHealth);
        PlayerPrefs.SetInt("PlayerDamage", playerDamage);
        PlayerPrefs.SetInt("PlayerSpeed", playerSpeed);
        PlayerPrefs.SetInt("PlayerDefense", playerDefense);
        PlayerPrefs.Save();

        Debug.Log($"Saved character: {selectedCharacterClass} (Index: {selectedCharacterIndex})");
    }

    // Load saved character
    void LoadSavedCharacter()
    {
        if (PlayerPrefs.HasKey("SelectedCharacterIndex"))
        {
            selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
            selectedCharacterClass = PlayerPrefs.GetString("SelectedCharacter", "Fighter");
            playerMaxHealth = PlayerPrefs.GetInt("PlayerMaxHealth", 100);
            playerHealth = PlayerPrefs.GetInt("PlayerHealth", 100);
            playerDamage = PlayerPrefs.GetInt("PlayerDamage", 15);
            playerSpeed = PlayerPrefs.GetInt("PlayerSpeed", 2);
            playerDefense = PlayerPrefs.GetInt("PlayerDefense", 5);

            Debug.Log($"Loaded saved character: {selectedCharacterClass} (Index: {selectedCharacterIndex})");
        }
        else
        {
            Debug.Log("No saved character found, using default (Fighter)");
        }
    }

    // Useful methods for gameplay
    public void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - playerDefense, 0);
        playerHealth -= actualDamage;
        playerHealth = Mathf.Max(playerHealth, 0);
        SaveCharacterSelection();
    }

    public void Heal(int amount)
    {
        playerHealth += amount;
        playerHealth = Mathf.Min(playerHealth, playerMaxHealth);
        SaveCharacterSelection();
    }

    public bool IsDead()
    {
        return playerHealth <= 0;
    }

    public void RestoreHealth()
    {
        playerHealth = playerMaxHealth;
        SaveCharacterSelection();
    }
}