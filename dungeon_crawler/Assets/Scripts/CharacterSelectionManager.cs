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
        if (allCards.Count == 0)
        {
            allCards.AddRange(FindObjectsByType<CharacterCardSelector>(FindObjectsSortMode.None));
        }

        SelectDefaultCard();
    }

    public void SelectCard(CharacterCardSelector cardToSelect)
    {
        foreach (CharacterCardSelector card in allCards)
        {
            card.SetSelected(false);
        }

        cardToSelect.SetSelected(true);
        currentlySelectedCard = cardToSelect;

        Debug.Log("Selected character: " + cardToSelect.characterClass);
    }

    void SelectDefaultCard()
    {
        CharacterCardSelector defaultCard = allCards.Find(card => card.characterClass.ToLower() == "knight");

        if (defaultCard != null)
        {
            SelectCard(defaultCard);
        }
        else if (allCards.Count > 0)
        {
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