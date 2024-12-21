using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class UIHandler : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    //public GameObject settingsPanel;
    public GameObject gameOverPanel;

    [Header("HUD")]
    public Slider mineralBar;
    public float currentTimer;
    public TextMeshProUGUI timerText;

    [Header("UI Buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;

    public InputHandler playerInput;

    private void Start()
    {
        // Initialize button listeners
        //playButton.onClick.AddListener(OnPlayButtonClicked);
        //settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        //quitButton.onClick.AddListener(OnQuitButtonClicked);
        // Initialize PlayerInput
        
        // Show the main menu at the start
        //ShowMainMenu();
    }
    private void Update()
    {
        // Decrease time as the game runs
        if (currentTimer > 0)
        {
            currentTimer += Time.deltaTime;
            currentTimer = Mathf.Max(currentTimer, 0); // Clamp to avoid negative time
        }

        // Update the timer text
        timerText.text = FormatTime(currentTimer);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60); // Calculate minutes
        int seconds = Mathf.FloorToInt(time % 60); // Calculate seconds
        return string.Format("{0:00}:{1:00}", minutes, seconds); // Format as MM:SS
    }
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        //settingsPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    //public void ShowSettings()
    //{
    //    mainMenuPanel.SetActive(false);
    //    settingsPanel.SetActive(true);
    //    gameOverPanel.SetActive(false);
    //}

    public void ShowGameOver()
    {
        mainMenuPanel.SetActive(false);
        //settingsPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    private void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked!");
        // Start the game logic here
    }

    //private void OnSettingsButtonClicked()
    //{
    //    Debug.Log("Settings button clicked!");
    //    ShowSettings();
    //}

    //private void OnQuitButtonClicked()
    //{
    //    Debug.Log("Quit button clicked!");
    //    Application.Quit();
    //}

    public void FadeOutCanvasGroupOnClick(CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeOutOnClickCoroutine(canvasGroup));
    }

    private IEnumerator FadeOutOnClickCoroutine(CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        while (!playerInput.MouseClick()) // Wait for UI click action
        {
            yield return null;
        }

        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;
        float duration = 0.5f; // Optional fade-out duration for smooth transition

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
