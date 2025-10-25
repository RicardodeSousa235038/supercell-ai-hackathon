using UnityEngine;

public class ApplyCharacterSkin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on " + gameObject.name);
            return;
        }

        ApplySkin();
    }

    void ApplySkin()
    {
        if (CharacterManager.Instance == null)
        {
            Debug.LogError("CharacterManager not found! Make sure it exists in the scene.");
            return;
        }

        Sprite selectedSprite = CharacterManager.Instance.GetSelectedCharacterSprite();

        if (selectedSprite != null)
        {
            spriteRenderer.sprite = selectedSprite;
            Debug.Log("Applied " + CharacterManager.Instance.selectedCharacterClass + " skin to " + gameObject.name);
        }
        else
        {
            Debug.LogError("No sprite found for selected character!");
        }
    }
}