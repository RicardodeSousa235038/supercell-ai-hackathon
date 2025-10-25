using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ClassIntroController : MonoBehaviour
{
    [System.Serializable]
    public class CharacterClass
    {
        public string className;
        public string subtitle;
        public Sprite characterSprite;
        public Color themeColor;
        
        [TextArea(3, 5)]
        public string background;
        
        public Ability[] abilities;
        
        public int health;
        public int damage;
        public int speed; // 1-4 stars
        
        [Header("Layout")]
        public bool useFlippedLayout = false; // Toggle to use Character2Scene
    }
    
    [System.Serializable]
    public class Ability
    {
        public string abilityName;
        public string abilityIcon; // emoji or icon name
        [TextArea(2, 3)]
        public string description;
    }
    
    [Header("Scene References")]
    public GameObject openingScene;
    public GameObject character1Scene; // Original layout
    public GameObject character2Scene; // Flipped layout
    
    [Header("Opening Scene")]
    public TextMeshProUGUI openingText;
    public TextMeshProUGUI questionText;
    
    [Header("Character1Scene - Original Layout")]
    public TextMeshProUGUI characterTitle1;
    public TextMeshProUGUI characterSubtitle1;
    public Image characterSpriteImage1;
    public TextMeshProUGUI backgroundText1;
    public Transform abilitiesContainer1;
    public GameObject abilityPrefab;
    
    [Header("Character2Scene - Flipped Layout")]
    public TextMeshProUGUI characterTitle2;
    public TextMeshProUGUI characterSubtitle2;
    public Image characterSpriteImage2;
    public TextMeshProUGUI backgroundText2;
    public Transform abilitiesContainer2;
    
    [Header("Character Classes")]
    public CharacterClass[] classes;
    
    [Header("Settings")]
    public float sceneDuration = 4f;
    public float fadeDuration = 1f;
    public KeyCode skipKey = KeyCode.Space;
    public string nextSceneName = "ClassSelection";
    
    private int currentClassIndex = -1; // -1 = opening
    private CanvasGroup openingCanvasGroup;
    private CanvasGroup character1CanvasGroup;
    private CanvasGroup character2CanvasGroup;
    private bool isTransitioning = false;
    private bool usingFlippedLayout = false;
    
    void Start()
    {
        SetupCanvasGroups();
        StartCoroutine(PlayIntroSequence());
    }
    
    void Update()
    {
        if (Input.GetKeyDown(skipKey) && !isTransitioning)
        {
            StopAllCoroutines();
            StartCoroutine(SkipToNext());
        }
    }
    
    void SetupCanvasGroups()
    {
        // Setup opening scene canvas group
        openingCanvasGroup = openingScene.GetComponent<CanvasGroup>();
        if (openingCanvasGroup == null)
            openingCanvasGroup = openingScene.AddComponent<CanvasGroup>();
        
        // Setup Character1Scene canvas group
        character1CanvasGroup = character1Scene.GetComponent<CanvasGroup>();
        if (character1CanvasGroup == null)
            character1CanvasGroup = character1Scene.AddComponent<CanvasGroup>();
        
        // Setup Character2Scene canvas group
        if (character2Scene != null)
        {
            character2CanvasGroup = character2Scene.GetComponent<CanvasGroup>();
            if (character2CanvasGroup == null)
                character2CanvasGroup = character2Scene.AddComponent<CanvasGroup>();
        }
        
        // Initial state: opening active, character scenes inactive
        openingScene.SetActive(true);
        character1Scene.SetActive(false);
        if (character2Scene != null)
            character2Scene.SetActive(false);
        
        openingCanvasGroup.alpha = 0f;
        character1CanvasGroup.alpha = 0f;
        if (character2CanvasGroup != null)
            character2CanvasGroup.alpha = 0f;
    }
    
    IEnumerator PlayIntroSequence()
    {
        // Show opening question
        yield return ShowOpening();
        yield return new WaitForSeconds(sceneDuration);
        
        // Show each character class
        for (int i = 0; i < classes.Length; i++)
        {
            yield return TransitionToCharacter(i);
            yield return new WaitForSeconds(sceneDuration);
        }
        
        // Transition to class selection
        LoadClassSelection();
    }
    
    IEnumerator ShowOpening()
    {
        isTransitioning = true;
        openingScene.SetActive(true);
        
        // Set text
        openingText.text = "In a dungeon, where danger is hidden\nand can come from every place...";
        questionText.text = "Which class are you going to take?";
        
        // Fade in
        yield return FadeCanvasGroup(openingCanvasGroup, 0f, 1f, fadeDuration);
        isTransitioning = false;
    }
    
    IEnumerator TransitionToCharacter(int classIndex)
    {
        isTransitioning = true;
        
        // Fade out current scene
        if (currentClassIndex == -1)
        {
            // Coming from opening
            yield return FadeCanvasGroup(openingCanvasGroup, 1f, 0f, fadeDuration);
            openingScene.SetActive(false);
        }
        else
        {
            // Coming from another character - fade out the active one
            if (usingFlippedLayout && character2CanvasGroup != null)
            {
                yield return FadeCanvasGroup(character2CanvasGroup, 1f, 0f, fadeDuration);
                character2Scene.SetActive(false);
            }
            else
            {
                yield return FadeCanvasGroup(character1CanvasGroup, 1f, 0f, fadeDuration);
                character1Scene.SetActive(false);
            }
        }
        
        // Setup new character
        currentClassIndex = classIndex;
        usingFlippedLayout = classes[classIndex].useFlippedLayout;
        
        SetupCharacterDisplay(classes[classIndex], usingFlippedLayout);
        
        // Fade in the correct character scene
        if (usingFlippedLayout && character2Scene != null)
        {
            character2Scene.SetActive(true);
            yield return FadeCanvasGroup(character2CanvasGroup, 0f, 1f, fadeDuration);
        }
        else
        {
            character1Scene.SetActive(true);
            yield return FadeCanvasGroup(character1CanvasGroup, 0f, 1f, fadeDuration);
        }
        
        isTransitioning = false;
    }
    
    void SetupCharacterDisplay(CharacterClass charClass, bool flipped)
    {
        // Choose which UI elements to use
        TextMeshProUGUI title = flipped ? characterTitle2 : characterTitle1;
        TextMeshProUGUI subtitle = flipped ? characterSubtitle2 : characterSubtitle1;
        Image sprite = flipped ? characterSpriteImage2 : characterSpriteImage1;
        TextMeshProUGUI background = flipped ? backgroundText2 : backgroundText1;
        Transform abilitiesParent = flipped ? abilitiesContainer2 : abilitiesContainer1;
        
        // Set title and subtitle
        title.text = charClass.className.ToUpper();
        title.color = charClass.themeColor;
        subtitle.text = $"\"{charClass.subtitle}\"";
        
        // Set character sprite
        sprite.sprite = charClass.characterSprite;
        sprite.color = Color.white;
        
        // Set background text
        background.text = charClass.background;
        
        // Clear and populate abilities
        foreach (Transform child in abilitiesParent)
        {
            Destroy(child.gameObject);
        }
        
        foreach (Ability ability in charClass.abilities)
        {
            GameObject abilityObj = Instantiate(abilityPrefab, abilitiesParent);
            
            // Find ability components
            TextMeshProUGUI abilityName = abilityObj.transform.Find("AbilityName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI abilityDesc = abilityObj.transform.Find("AbilityDescription").GetComponent<TextMeshProUGUI>();
            
            abilityName.text = $"{ability.abilityIcon} {ability.abilityName}";
            abilityName.color = charClass.themeColor;
            abilityDesc.text = ability.description;
        }
    }
    
    IEnumerator SkipToNext()
    {
        isTransitioning = true;
        
        if (currentClassIndex == -1)
        {
            // Skip opening, go to first character
            yield return TransitionToCharacter(0);
        }
        else if (currentClassIndex < classes.Length - 1)
        {
            // Go to next character
            yield return TransitionToCharacter(currentClassIndex + 1);
        }
        else
        {
            // End of sequence
            LoadClassSelection();
        }
        
        isTransitioning = false;
    }
    
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        
        cg.alpha = end;
    }
    
    void LoadClassSelection()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}

// SETUP GUIDE:
/*
HIERARCHY STRUCTURE:
Canvas
├── OpeningScene
│   ├── OpeningText
│   └── QuestionText
├── Character1Scene (INACTIVE at start)
│   ├── LeftSide
│   │   └── CharacterSprite
│   └── RightSide
│       ├── CharacterTitle
│       ├── CharacterSubtitle
│       ├── BackgroundPanel
│       │   ├── BackgroundHeader
│       │   └── BackgroundText
│       └── AbilitiesPanel
│           ├── AbilitiesHeader
│           └── AbilitiesContainer
├── Character2Scene (INACTIVE at start) - FLIPPED VERSION
│   ├── LeftSide (anchored RIGHT)
│   │   └── CharacterSprite
│   └── RightSide (anchored LEFT)
│       ├── CharacterTitle
│       ├── CharacterSubtitle
│       ├── BackgroundPanel
│       │   ├── BackgroundHeader
│       │   └── BackgroundText
│       └── AbilitiesPanel
│           ├── AbilitiesHeader
│           └── AbilitiesContainer
└── SkipText

INSPECTOR ASSIGNMENTS:
Scene References:
- Opening Scene: OpeningScene
- Character1Scene: Character1Scene
- Character2Scene: Character2Scene

Opening Scene:
- Opening Text: OpeningScene/OpeningText
- Question Text: OpeningScene/QuestionText

Character1Scene - Original Layout:
- Character Title 1: Character1Scene/RightSide/CharacterTitle
- Character Subtitle 1: Character1Scene/RightSide/CharacterSubtitle
- Character Sprite Image 1: Character1Scene/LeftSide/CharacterSprite
- Background Text 1: Character1Scene/RightSide/BackgroundPanel/BackgroundText
- Abilities Container 1: Character1Scene/RightSide/AbilitiesPanel/AbilitiesContainer
- Ability Prefab: (your prefab from Assets)

Character2Scene - Flipped Layout:
- Character Title 2: Character2Scene/RightSide/CharacterTitle
- Character Subtitle 2: Character2Scene/RightSide/CharacterSubtitle
- Character Sprite Image 2: Character2Scene/LeftSide/CharacterSprite
- Background Text 2: Character2Scene/RightSide/BackgroundPanel/BackgroundText
- Abilities Container 2: Character2Scene/RightSide/AbilitiesPanel/AbilitiesContainer

CHARACTER CLASSES EXAMPLE:
Element 0 - Warrior:
- Use Flipped Layout: ☐ (unchecked) → Uses Character1Scene
Element 1 - Mage:
- Use Flipped Layout: ☑ (checked) → Uses Character2Scene
Element 2 - Rogue:
- Use Flipped Layout: ☐ (unchecked) → Uses Character1Scene
*/