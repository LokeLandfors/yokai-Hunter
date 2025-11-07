using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;
    public Text hoverText;

    [Header("Buttons")]
    public Button button1; // Start
    public Button button2; // Quit
    public Button button3; // Stats
    public Button button4; // Skills

    private void Start()
    {
        if (hoverText != null)
            hoverText.gameObject.SetActive(false);

        
        if (button1 != null)
            button1.onClick.AddListener(ResumeGame);

        if (button2 != null)
            button2.onClick.AddListener(QuitToMainMenu);

        // Add hover effects for Button 3 and 4
        AddHoverListeners(button3, "Coming Soon");
        AddHoverListeners(button4, "Coming Soon");
    }

    private void ResumeGame()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
    }

    private void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void AddHoverListeners(Button button, string message)
    {
        if (button == null || hoverText == null)
            return;

        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        trigger.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();

        // Pointer Enter
        EventTrigger.Entry entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((data) => ShowHoverText(message, true));
        trigger.triggers.Add(entryEnter);

        // Pointer Exit
        EventTrigger.Entry entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((data) => ShowHoverText(message, false));
        trigger.triggers.Add(entryExit);
    }

    private void ShowHoverText(string message, bool show)
    {
        hoverText.text = message;
        hoverText.gameObject.SetActive(show);
    }
}
