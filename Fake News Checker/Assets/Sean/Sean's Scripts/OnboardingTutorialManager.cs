using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class OnboardingTutorialManager : MonoBehaviour
{
    [System.Serializable]
    public struct TutorialSlide
    {
        public Sprite slideImage;
        [TextArea(3, 6)]
        public string slideText;
    }

    [Header("UI References")]
    public Image leftDisplayImage;       // Image on the left
    public TMP_Text rightDialogueText;   // Text on the right
    public TMP_Text nextButtonLabel;     // Optional: Text inside the bottom-right button
    public GameObject appWindowPanelObject; // Drag your 'AppWindowPanel' here
    public GameObject grandmaProfilePanelObject; // Drag 'GrandmaProfilePanel' here
    public GameObject helperLifelineWidgetObject; // Drag 'HelperLifelineWidget' here
    public GameObject batteryHudObject;          // Drag your Battery UI parent GameObject here
    public Button helperActionButton;            // Drag 'HelperActionButton' here (ensures sub-canvas button is locked)

    [Header("Presentation Slides")]
    public List<TutorialSlide> slides = new List<TutorialSlide>();

    private int currentSlideIndex = 0;

    private void OnEnable()
    {
        // Pause the battery timer whenever the onboarding screen opens
        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.SetTimerPaused(true);
        }
        if (batteryHudObject != null)
        {
            batteryHudObject.SetActive(false);
        }

        // 2. Keep AppWindowPanel hidden, show FullPostModePopup in the background, and block clicks on it
        //if (appWindowPanelObject != null) appWindowPanelObject.SetActive(false);

        // 2. Show AppWindowPanel behind Onboarding, but block all clicks/scrolling on it
        if (appWindowPanelObject != null)
        {
            appWindowPanelObject.SetActive(true);
            SetPanelInteractable(appWindowPanelObject, false);
        }

        if (grandmaProfilePanelObject != null)
        {
            grandmaProfilePanelObject.SetActive(true);
            SetPanelInteractable(grandmaProfilePanelObject, false);
        }

        if (helperLifelineWidgetObject != null)
        {
            helperLifelineWidgetObject.SetActive(true);
            SetPanelInteractable(helperLifelineWidgetObject, false);
        }

        // Explicitly disable HelperActionButton in case its sub-Canvas overrides parent CanvasGroup
        if (helperActionButton != null)
        {
            helperActionButton.interactable = false;
        }

        currentSlideIndex = 0;
        ShowCurrentSlide();
    }

    private void OnDisable()
    {
        // 1. Re-enable battery UI and launch a fresh 7-bar timer
        if (batteryHudObject != null)
        {
            batteryHudObject.SetActive(true);
        }
        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.LaunchTimerEngine();
        }

        // 2. Re-enable interaction on AppWindowPanel and show HUD widgets
        if (appWindowPanelObject != null)
        {
            appWindowPanelObject.SetActive(true);
            SetPanelInteractable(appWindowPanelObject, true);
        }

        if (grandmaProfilePanelObject != null)
        {
            grandmaProfilePanelObject.SetActive(true);
            SetPanelInteractable(grandmaProfilePanelObject, true);
        }

        if (helperLifelineWidgetObject != null)
        {
            helperLifelineWidgetObject.SetActive(true);
            SetPanelInteractable(helperLifelineWidgetObject, true);
        }

        if (helperActionButton != null)
        {
            helperActionButton.interactable = true;
        }

        if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(true);
        if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(true);

        //// Resume the battery timer automatically when Onboarding is closed / SetActive(false)
        //if (BatteryTimeManager.Instance != null)
        //{
        //    BatteryTimeManager.Instance.SetTimerPaused(false);
        //}
        //// Turn on AppWindowPanel once Onboarding closes
        //if (appWindowPanelObject != null)
        //{
        //    //appWindowPanelObject.SetActive(true);
        //    // Turn on AppWindowPanel, Grandma, and Helper once Onboarding closes
        //    if (appWindowPanelObject != null) appWindowPanelObject.SetActive(true);
        //    if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(true);
        //    if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(true);
        //}
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
        cg.alpha = isInteractable ? 1f : 0.5f; // Uniformly fades ALL child images, texts, and buttons!
    }

    // Wire this to your bottom-right "Next" button's OnClick() event
    public void OnNextSlideButtonClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayOnboardingClick();
        }

        currentSlideIndex++;

        if (currentSlideIndex < slides.Count)
        {
            ShowCurrentSlide();
        }
        else
        {
            // All slides finished -> close onboarding (this automatically triggers OnDisable and starts the timer!)
            gameObject.SetActive(false);
        }
    }

    private void ShowCurrentSlide()
    {
        if (slides == null || slides.Count == 0) return;

        TutorialSlide current = slides[currentSlideIndex];

        if (rightDialogueText != null)
        {
            rightDialogueText.text = current.slideText;
        }

        // Only swap the sprite if one is assigned for this slide; otherwise keep the previous sprite
        if (leftDisplayImage != null && current.slideImage != null)
        {
            leftDisplayImage.sprite = current.slideImage;
        }

        // Optional: Change button label on the final slide
        if (nextButtonLabel != null)
        {
            nextButtonLabel.text = (currentSlideIndex == slides.Count - 1) ? "Start" : "Next";
        }
    }
}
