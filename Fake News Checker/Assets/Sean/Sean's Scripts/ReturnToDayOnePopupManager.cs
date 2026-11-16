using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReturnToDayOnePopupManager : MonoBehaviour
{
    [Header("Popup Text & Visuals")]
    public GameObject gameOverHeaderObject;   // Drag the red "GAME OVER" TMP_Text GameObject here
    public TMP_Text promptQuestionText;       // Drag "Would you like to return to day 1 ?" TMP_Text here

    [Header("Background & Screen References")]
    public GameObject desktopBackgroundPanel; // Drag 'DesktopBackgroundPanel' here
    public GameObject appWindowPanelObject;   // Drag 'AppWindowPanel' here
    public GameObject grandmaProfilePanel;    // Drag 'GrandmaProfilePanel' here
    public GameObject helperLifelineWidget;   // Drag 'HelperLifelineWidget' here
    public Button helperActionButton;         // Drag 'HelperActionButton' here
    public GameObject batteryHudObject;       // Drag your Battery UI parent GameObject here

    public void OpenPopup(bool isZeroReliabilityGameOver)
    {
        // Show "GAME OVER" ONLY for Slide 29 (0 reliability loss); hide it for Slide 30 (post-Day 3 endings)
        if (gameOverHeaderObject != null)
        {
            gameOverHeaderObject.SetActive(isZeroReliabilityGameOver);
        }

        if (promptQuestionText != null)
        {
            promptQuestionText.text = "Would you like to return to day 1 ?";
        }

        // Pause and hide battery while the decision popup is open
        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.SetTimerPaused(true);
        }
        if (batteryHudObject != null)
        {
            batteryHudObject.SetActive(false);
        }

        // For Slide 29 (mid-game loss), show the feed & HUD behind the popup but lock all clicks
        if (isZeroReliabilityGameOver)
        {
            if (appWindowPanelObject != null)
            {
                appWindowPanelObject.SetActive(true);
                SetPanelInteractable(appWindowPanelObject, false);
            }
            if (grandmaProfilePanel != null)
            {
                grandmaProfilePanel.SetActive(true);
                SetPanelInteractable(grandmaProfilePanel, false);
            }
            if (helperLifelineWidget != null)
            {
                helperLifelineWidget.SetActive(true);
                SetPanelInteractable(helperLifelineWidget, false);
            }
            if (helperActionButton != null)
            {
                helperActionButton.interactable = false;
            }
        }

        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

    private void OnEnable()
    {
        Debug.Log($"[POPUP TRACE] ReturnToDayOnePopup ENABLED! StackTrace:\n{System.Environment.StackTrace}");
    }

    private void OnDisable()
    {
        Debug.Log("[POPUP TRACE] ReturnToDayOnePopup DISABLED!");
    }


    public void OnYesButtonClicked()
    {
        Debug.Log($"[POPUP] YES clicked on: {gameObject.name} (Parent: {transform.parent.name})");
        UnlockBackgroundPanels();
        gameObject.SetActive(false);

        if (GameScrollManager.Instance != null)
        {
            GameScrollManager.Instance.RestartRunToOnboarding();
        }
    }

    public void OnNoButtonClicked()
    {
        Debug.Log($"[POPUP] NO clicked on: {gameObject.name} (Parent: {transform.parent.name})");
        UnlockBackgroundPanels();
        gameObject.SetActive(false);

        if (GameScrollManager.Instance != null)
        {
            GameScrollManager.Instance.RestartRunToDesktop();
        }
    }

    private void UnlockBackgroundPanels()
    {
        SetPanelInteractable(appWindowPanelObject, true);
        SetPanelInteractable(grandmaProfilePanel, true);
        SetPanelInteractable(helperLifelineWidget, true);
        if (helperActionButton != null) helperActionButton.interactable = true;
    }

    private void SetPanelInteractable(GameObject targetPanel, bool isInteractable)
    {
        if (targetPanel == null) return;

        CanvasGroup cg = targetPanel.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = targetPanel.AddComponent<CanvasGroup>();
        }

        cg.interactable = isInteractable;
        cg.blocksRaycasts = isInteractable;
    }
}
