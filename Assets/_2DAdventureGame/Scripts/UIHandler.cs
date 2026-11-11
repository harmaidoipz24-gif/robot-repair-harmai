using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance { get; private set; }

    private VisualElement m_HealthBar;
    private VisualElement m_NPCDialogue;
    private Label m_DialogueText;
    private Label m_CounterLabel;
    private VisualElement m_WinScreen;
    private VisualElement m_LoseScreen;

    private float m_TimerDisplay = -1.0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;

        m_HealthBar = root.Q<VisualElement>("HealthBar");

        m_NPCDialogue = root.Q<VisualElement>("NPCDialogue");
        if (m_NPCDialogue != null)
        {
            m_DialogueText = m_NPCDialogue.Q<Label>();
            m_NPCDialogue.style.display = DisplayStyle.None;
        }

        m_CounterLabel = root.Q<Label>("CounterLabel");
        m_WinScreen = root.Q<VisualElement>("WinScreenContainer");
        m_LoseScreen = root.Q<VisualElement>("LoseScreenContainer");

        if (m_WinScreen != null) m_WinScreen.style.display = DisplayStyle.None;
        if (m_LoseScreen != null) m_LoseScreen.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        if (m_TimerDisplay > 0)
        {
            m_TimerDisplay -= Time.deltaTime;
            if (m_TimerDisplay <= 0)
            {
                HideDialogue();
            }
        }
    }

    public void SetHealthValue(float percentage)
    {
        if (m_HealthBar != null)
        {
            m_HealthBar.style.width = Length.Percent(percentage * 100);
        }
    }

    public void SetCounter(int current, int total)
    {
        if (m_CounterLabel != null)
        {
            m_CounterLabel.text = $"Fixed: {current} / {total}";
        }
    }

    // Підтримка виклику без параметрів для демо-скриптів
    public void DisplayDialogue()
    {
        if (m_NPCDialogue != null)
        {
            m_NPCDialogue.style.display = DisplayStyle.Flex;
            m_TimerDisplay = 4.0f;
        }
    }

    // Виклик із передачею тексту та часу
    public void DisplayDialogue(string text, float displayTime = 4.0f)
    {
        if (m_NPCDialogue != null)
        {
            if (m_DialogueText != null && !string.IsNullOrEmpty(text))
            {
                m_DialogueText.text = text;
            }
            m_NPCDialogue.style.display = DisplayStyle.Flex;
            m_TimerDisplay = displayTime;
        }
    }

    public void HideDialogue()
    {
        if (m_NPCDialogue != null)
        {
            m_NPCDialogue.style.display = DisplayStyle.None;
        }
    }

    public void DisplayWinScreen()
    {
        if (m_WinScreen != null)
        {
            m_WinScreen.style.display = DisplayStyle.Flex;
        }
    }

    public void DisplayLoseScreen()
    {
        if (m_LoseScreen != null)
        {
            m_LoseScreen.style.display = DisplayStyle.Flex;
        }
    }
}