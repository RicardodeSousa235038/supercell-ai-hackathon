using UnityEngine;
using System.Collections.Generic;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance;

    [Header("All Character Cards")]
    public List<CharacterCardSelector> allCards = new List<CharacterCardSelector>();

    private CharacterCardSelector currentlySelectedCard;

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
        // Auto-find all character cards if list is empty
        if (allCards.Count == 0)
        {
            allCards.AddRange(FindObjectsByType<CharacterCardSelector>(FindObjectsSortMode.None));
        }

        // Optionally select a default card (e.g., Knight)
        SelectDefaultCard();
    }

    public void SelectCard(CharacterCardSelector cardToSelect)
    {
        // Deselect all other cards
        foreach (CharacterCardSelector card in allCards)
        {
            card.SetSelected(false);
        }

        // Select the clicked card
        cardToSelect.SetSelected(true);
        currentlySelectedCard = cardToSelect;

        Debug.Log("Selected character: " + cardToSelect.characterClass);
    }

    void SelectDefaultCard()
    {
        // Find and select the Knight card by default
        CharacterCardSelector defaultCard = allCards.Find(card => card.characterClass.ToLower() == "knight");

        if (defaultCard != null)
        {
            SelectCard(defaultCard);
        }
        else if (allCards.Count > 0)
        {
            // If Knight not found, select first card
            SelectCard(allCards[0]);
        }
    }

    public CharacterCardSelector GetSelectedCard()
    {
        return currentlySelectedCard;
    }

    public bool HasSelectedCard()
    {
        return currentlySelectedCard != null;
    }
}