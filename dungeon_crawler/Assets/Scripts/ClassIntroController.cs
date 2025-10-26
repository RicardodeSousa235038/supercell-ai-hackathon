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
        public int speed;
        
        [Header("Layout")]
        public bool useFlippedLayout = false;
    }
    
    [System.Serializable]
    public class Ability
    {
        public string abilityName;
        public string abilityIcon;
        [TextArea(2, 3)]
        public string description;
    }
    
    [Header("Scene References")]
    public GameObject openingScene;
    public GameObject character1Scene;
    public GameObject character2Scene;
    
    [Header("Opening Scene")]
    public TextMeshProUGUI openingText;
    public TextMeshProUGUI questionText;
    
    [Header("Character Scenes")]
    public TextMeshProUGUI characterTitle1;
    public TextMeshProUGUI characterSubtitle1;
    public Image characterSpriteImage1;
    public TextMeshProUGUI backgroundText1;
    public Transform abilitiesContainer1;
    public Button continueButton1;
    public GameObject abilityPrefab;
    
    public TextMeshProUGUI characterTitle2;
    public TextMeshProUGUI characterSubtitle2;
    public Image characterSpriteImage2;
    public TextMeshProUGUI backgroundText2;
    public Transform abilitiesContainer2;
    public Button continueButton2;
    
    [Header("Character Classes")]
    public CharacterClass[] classes;
    
    [Header("Settings")]
    public float typewriterSpeed = 0.05f;
    public float fadeDuration = 1f;
    public Color flameColor1 = new Color(1f, 0.3f, 0f, 1f);
    public Color flameColor2 = new Color(0.8f, 0.1f, 0f, 1f);
    public bool autoAdvance = false;
    public float autoAdvanceDelay = 4f;
    public KeyCode skipKey = KeyCode.Space;
    
    [Header("Scene Transition")]
    public bool useSceneTransition = false;
    public string nextSceneName = "FirstMap";
    public GameObject selectionCanvas;
    
    private int currentClassIndex = -1;
    private CanvasGroup openingCanvasGroup;
    private CanvasGroup character1CanvasGroup;
    private CanvasGroup character2CanvasGroup;
    private bool isTransitioning = false;
    private bool usingFlippedLayout = false;
    private bool waitingForInput = false;
    
    void Start()
    {
        SetupCanvasGroups();
        SetupButtons();
        StartCoroutine(PlayIntroSequence());
    }
    
    void Update()
    {
        if (Input.GetKeyDown(skipKey) && waitingForInput && !isTransitioning)
        {
            OnContinuePressed();
        }
    }
    
    void SetupCanvasGroups()
    {
        openingCanvasGroup = openingScene.GetComponent<CanvasGroup>();
        if (openingCanvasGroup == null)
            openingCanvasGroup = openingScene.AddComponent<CanvasGroup>();
        
        character1CanvasGroup = character1Scene.GetComponent<CanvasGroup>();
        if (character1CanvasGroup == null)
            character1CanvasGroup = character1Scene.AddComponent<CanvasGroup>();
        
        if (character2Scene != null)
        {
            character2CanvasGroup = character2Scene.GetComponent<CanvasGroup>();
            if (character2CanvasGroup == null)
                character2CanvasGroup = character2Scene.AddComponent<CanvasGroup>();
        }
        
        openingScene.SetActive(true);
        character1Scene.SetActive(false);
        if (character2Scene != null)
            character2Scene.SetActive(false);
        
        openingCanvasGroup.alpha = 0f;
        character1CanvasGroup.alpha = 0f;
        if (character2CanvasGroup != null)
            character2CanvasGroup.alpha = 0f;
    }
    
    void SetupButtons()
    {
        if (continueButton1 != null)
        {
            continueButton1.onClick.AddListener(OnContinuePressed);
            continueButton1.gameObject.SetActive(false);
        }
        
        if (continueButton2 != null)
        {
            continueButton2.onClick.AddListener(OnContinuePressed);
            continueButton2.gameObject.SetActive(false);
        }
    }
    
    IEnumerator PlayIntroSequence()
    {
        yield return ShowOpeningWithFlameText();
        
        for (int i = 0; i < classes.Length; i++)
        {
            yield return TransitionToCharacter(i);
            
            waitingForInput = true;
            ShowContinueButton(true);
            
            if (autoAdvance)
            {
                yield return new WaitForSeconds(autoAdvanceDelay);
                waitingForInput = false;
            }
            else
            {
                while (waitingForInput)
                {
                    yield return null;
                }
            }
            
            ShowContinueButton(false);
        }
        
        LoadClassSelection();
    }
    
    IEnumerator ShowOpeningWithFlameText()
    {
        isTransitioning = true;
        openingScene.SetActive(true);
        
        string openingFullText = openingText.text;
        string questionFullText = questionText.text;
        openingText.text = "";
        questionText.text = "";
        
        yield return FadeCanvasGroup(openingCanvasGroup, 0f, 1f, fadeDuration);
        
        yield return FlameTypewriter(openingText, openingFullText, typewriterSpeed);
        yield return new WaitForSeconds(0.5f);
        yield return FadeOutText(openingText, 1f);
        yield return FlameTypewriter(questionText, questionFullText, typewriterSpeed);
        
        isTransitioning = false;
    }
    
    IEnumerator FlameTypewriter(TextMeshProUGUI textComponent, string fullText, float delay)
    {
        textComponent.text = "";
        textComponent.color = flameColor1;
        
        Coroutine pulseCoroutine = StartCoroutine(ContinuousPulse(textComponent));
        
        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(delay);
        }
        
        StopCoroutine(pulseCoroutine);
    }
    
    IEnumerator ContinuousPulse(TextMeshProUGUI textComponent)
    {
        while (true)
        {
            float t = Mathf.PingPong(Time.time * 2f, 1f);
            textComponent.color = Color.Lerp(flameColor1, flameColor2, t);
            yield return null;
        }
    }
    
    IEnumerator FadeOutText(TextMeshProUGUI textComponent, float duration)
    {
        Color startColor = textComponent.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            textComponent.color = Color.Lerp(startColor, endColor, elapsed / duration);
            yield return null;
        }
        
        textComponent.color = endColor;
        textComponent.text = "";
    }
    
    void ShowContinueButton(bool show)
    {
        Button currentButton = usingFlippedLayout ? continueButton2 : continueButton1;
        if (currentButton != null)
        {
            currentButton.gameObject.SetActive(show);
        }
    }
    
    public void OnContinuePressed()
    {
        if (waitingForInput)
        {
            waitingForInput = false;
        }
    }
    
    IEnumerator TransitionToCharacter(int classIndex)
    {
        isTransitioning = true;
        
        if (currentClassIndex == -1)
        {
            yield return FadeCanvasGroup(openingCanvasGroup, 1f, 0f, fadeDuration);
            openingScene.SetActive(false);
        }
        else
        {
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
        
        currentClassIndex = classIndex;
        usingFlippedLayout = classes[classIndex].useFlippedLayout;
        SetupCharacterDisplay(classes[classIndex], usingFlippedLayout);
        
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
        TextMeshProUGUI title = flipped ? characterTitle2 : characterTitle1;
        TextMeshProUGUI subtitle = flipped ? characterSubtitle2 : characterSubtitle1;
        Image sprite = flipped ? characterSpriteImage2 : characterSpriteImage1;
        TextMeshProUGUI background = flipped ? backgroundText2 : backgroundText1;
        Transform abilitiesParent = flipped ? abilitiesContainer2 : abilitiesContainer1;
        
        title.text = charClass.className.ToUpper();
        title.color = charClass.themeColor;
        subtitle.text = $"\"{charClass.subtitle}\"";
        
        sprite.sprite = charClass.characterSprite;
        sprite.color = Color.white;
        
        background.text = charClass.background;
        
        foreach (Transform child in abilitiesParent)
        {
            Destroy(child.gameObject);
        }
        
        if (abilityPrefab != null)
        {
            foreach (Ability ability in charClass.abilities)
            {
                GameObject abilityObj = Instantiate(abilityPrefab, abilitiesParent);
                
                TextMeshProUGUI abilityName = abilityObj.transform.Find("AbilityName").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI abilityDesc = abilityObj.transform.Find("AbilityDescription").GetComponent<TextMeshProUGUI>();
                
                if (abilityName != null)
                {
                    abilityName.text = $"{ability.abilityIcon} {ability.abilityName}";
                    abilityName.color = charClass.themeColor;
                }
                
                if (abilityDesc != null)
                {
                    abilityDesc.text = ability.description;
                }
            }
        }
    }
    
    IEnumerator SkipToNext()
    {
        isTransitioning = true;
        
        if (currentClassIndex == -1)
        {
            yield return TransitionToCharacter(0);
        }
        else if (currentClassIndex < classes.Length - 1)
        {
            yield return TransitionToCharacter(currentClassIndex + 1);
        }
        else
        {
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
        if (useSceneTransition)
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            if (selectionCanvas != null)
            {
                openingScene.SetActive(false);
                character1Scene.SetActive(false);
                character2Scene.SetActive(false);
                
                selectionCanvas.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Selection Canvas not assigned!");
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}