using UnityEngine;
using UnityEngine.UI;

public class CharacterCardSelector : MonoBehaviour
{
    [Header("Card Settings")]
    public string characterClass; // Set this in Inspector (e.g., "Knight", "Archer", etc.)

    [Header("Visual Feedback")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.green;
    public Image cardImage; // Reference to the card's Image component

    [Header("Optional - Selection Border")]
    public GameObject selectionBorder; // Optional: a border/outline that appears when selected

    private Button button;
    private bool isSelected = false;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnCardClicked);
        }

        if (cardImage == null)
        {
            cardImage = GetComponent<Image>();
        }

        // Set initial state
        UpdateVisuals();
    }

    void OnCardClicked()
    {
        // Tell the manager to select this character (and deselect others)
        CharacterSelectionManager.Instance.SelectCard(this);
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        UpdateVisuals();

        // If this card is selected, update the CharacterManager
        if (isSelected)
        {
            CharacterManager.Instance.SelectCharacter(characterClass);
        }
    }

    void UpdateVisuals()
    {
        if (cardImage != null)
        {
            cardImage.color = isSelected ? selectedColor : normalColor;
        }

        if (selectionBorder != null)
        {
            selectionBorder.SetActive(isSelected);
        }
    }

    public bool IsSelected()
    {
        return isSelected;
    }
}