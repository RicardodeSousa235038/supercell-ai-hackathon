using UnityEngine;

public class ApplyCharacterSkin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        Debug.Log($"ApplyCharacterSkin Start() on {gameObject.name}");

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on " + gameObject.name);
            return;
        }

        ApplySkin();
    }

    void Update()
    {
        // Press 'C' to manually apply skin for testing
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Manually triggering ApplySkin...");
            ApplySkin();
        }
    }

    void ApplySkin()
    {
        Debug.Log($"ApplySkin() called on {gameObject.name}");

        if (CharacterManager.Instance == null)
        {
            Debug.LogError("CharacterManager not found!");
            return;
        }

        Debug.Log($"=== APPLYING SKIN ===");
        Debug.Log($"Index: {CharacterManager.Instance.selectedCharacterIndex}");
        Debug.Log($"Class: {CharacterManager.Instance.selectedCharacterClass}");

        Sprite selectedSprite = CharacterManager.Instance.GetSelectedCharacterSprite();

        if (selectedSprite != null)
        {
            Debug.Log($"Sprite being applied: {selectedSprite.name}");
            spriteRenderer.sprite = selectedSprite;
            Debug.Log($"✅ SUCCESS! Applied {CharacterManager.Instance.selectedCharacterClass} to {gameObject.name}");
        }
        else
        {
            Debug.LogError("No sprite found for selected character!");
        }
    }
}