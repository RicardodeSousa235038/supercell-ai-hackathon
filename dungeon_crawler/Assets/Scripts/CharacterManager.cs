using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    [Header("Available Character Sprites")]
    public Sprite beastClass;
    public Sprite vampire;
    public Sprite fighter;
    public Sprite archer;
    public Sprite knight;
    public Sprite thief;

    [Header("Currently Selected")]
    public string selectedCharacterClass = "Knight"; // Default

    void Awake()
    {
        // Singleton pattern - only one CharacterManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persists between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this when player selects a character
    public void SelectCharacter(string characterClass)
    {
        selectedCharacterClass = characterClass;
        Debug.Log("Character selected: " + characterClass);
    }

    // Get the sprite for the selected character
    public Sprite GetSelectedCharacterSprite()
    {
        switch (selectedCharacterClass.ToLower())
        {
            case "beast class":
                return beastClass;
            case "vampire":
                return vampire;
            case "fighter":
                return fighter;
            case "archer":
                return archer;
            case "knight":
                return knight;
            case "thief":
                return thief;
            default:
                Debug.LogWarning("Unknown character class: " + selectedCharacterClass);
                return knight; // Default to knight
        }
    }
}