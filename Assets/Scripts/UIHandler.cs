using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    [Header("UI Panels")]
    public CanvasGroup mainMenuPanel;
    public CanvasGroup HUDPanel;
    //public GameObject settingsPanel;
    public CanvasGroup gameOverPanel;
    public CanvasGroup blackScreen;
    [Header("HUD")]
    public Slider mineralBar;
    public float currentTimer;
    public TextMeshProUGUI timerText;

    [Header("UI Buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;

    private InputHandler playerInput;
    private GameManager gameManager;
    private void Start()
    {
        // Initialize button listeners
        if (playButton != null)
        {
            playButton.onClick.AddListener(delegate { ChangeUI("OnPlayButtonClicked", 0f); });
            playButton.onClick.AddListener(ConfirmFade);
        }
        else
        {
            gameManager = GameManager.Instance;
            gameManager.UIHandler = this;
            UpdateResourceBar(gameManager.currentResourcesNeeded, gameManager.resources);
            StartCoroutine(FadeOutBlack());
        }
        playerInput = InputHandler.Instance;
    }
    private void Update()
    {
        // Increase time as the game runs
        if (SceneManager.GetActiveScene().name == "Test")
        {
            if (playerInput.MainMenu())
                ChangeUI("ShowMainMenu", 0f);
            currentTimer += Time.deltaTime;
            currentTimer = Mathf.Max(currentTimer, 0); // Clamp to avoid negative time
            timerText.text = FormatTime(currentTimer);
        }
    }
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60); // Calculate minutes
        int seconds = Mathf.FloorToInt(time % 60); // Calculate seconds
        return string.Format("{0:00}:{1:00}", minutes, seconds); // Format as MM:SS
    }

    public void ChangeUI(string method, float time)
    {
        Invoke(method, time);
    }
    public void ShowMainMenu()
    {
        Time.timeScale = 0f;
        StartCoroutine(FadeInBlack("MainMenu")); ;
    }
    public void ShowGameOver()
    {
        StartCoroutine(FadeInBlack("GameOver"));
    }

    private void OnPlayButtonClicked()
    {
        StartCoroutine(FadeInBlack("Test"));
    }

    public void LoadSceneWithDelay(string sceneString)
    {
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

    public void UpdateResourceBar(int max, int current)
    {
        mineralBar.maxValue = max;
        mineralBar.value = current;
    }


    bool fadeCheck;
    public void FadeOutCanvasGroupOnClick(CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeOutOnClickCoroutine(canvasGroup));
    }

    private IEnumerator FadeOutOnClickCoroutine(CanvasGroup canvasGroup)
    {

        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;
        float duration = 0.5f; // Optional fade-out duration for smooth transition

        if (startAlpha == 0f)
        {
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, elapsed / duration);
                yield return null;
            }


            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            while (!fadeCheck) // Wait for UI click action
            {
                yield return null;
            }
        }

        startAlpha = canvasGroup.alpha;
        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        fadeCheck = false;
        Time.timeScale = 1;
    }
    private IEnumerator FadeInBlack(string sceneString)
    {
        blackScreen.interactable = true;
        blackScreen.blocksRaycasts = true;

        float elapsed = 0f;
        float duration = 0.5f; // Optional fade-out duration for smooth transition

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            blackScreen.alpha = Mathf.Lerp(0, 1, elapsed / duration);
            yield return null;
        }


        SceneManager.LoadScene(sceneString);
    }
    private IEnumerator FadeOutBlack()
    {

        Time.timeScale = 0;
        float elapsed = 0f;
        float duration = 0.5f; // Optional fade-out duration for smooth transition

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            blackScreen.alpha = Mathf.Lerp(1, 0, elapsed / duration);
            yield return null;
        }


        blackScreen.interactable = true;
        blackScreen.blocksRaycasts = true;
        Time.timeScale = 1;
    }

    #region Upgrade UI
    [Header("Upgrade Menu")]

    public CanvasGroup upgradeMenu;
    public List<UpgradeCard> upgradeCards = new List<UpgradeCard>();
    public List<UpgradeInfo> upgradeUIInfomation = new List<UpgradeInfo>();

    public void UpgradeMenu()
    {
        Time.timeScale = 0;
        FadeOutCanvasGroupOnClick(upgradeMenu);
        List<UpgradeMethod> currentMethods = Enum.GetValues(typeof(UpgradeMethod)).Cast<UpgradeMethod>().ToList();
        foreach (UpgradeCard card in upgradeCards)
        {
            UpgradeMethod methodToUse = currentMethods[UnityEngine.Random.Range(0, currentMethods.Count)];
            currentMethods.Remove(methodToUse);


            UpgradeInfo infoToUse = new UpgradeInfo();
            foreach (UpgradeInfo info in upgradeUIInfomation)
            {
                if (methodToUse == info.methodKey)
                {
                    infoToUse = info;
                    break;
                }
            }

            card.UpdateUpgradeCard(methodToUse, infoToUse);
        }
    }
    public void ConfirmFade()
    {
        fadeCheck = true;
    }
    #endregion
}

[System.Serializable]
public class UpgradeInfo
{
    public UpgradeMethod methodKey;
    [TextArea]
    public string info;
    public Sprite icon;

}
