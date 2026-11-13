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

        currentSlideIndex = 0;
        ShowCurrentSlide();
    }

    private void OnDisable()
    {
        // Resume the battery timer automatically when Onboarding is closed / SetActive(false)
        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.SetTimerPaused(false);
        }

        // Turn on AppWindowPanel once Onboarding closes
        if (appWindowPanelObject != null)
        {
            //appWindowPanelObject.SetActive(true);
            // Turn on AppWindowPanel, Grandma, and Helper once Onboarding closes
            if (appWindowPanelObject != null) appWindowPanelObject.SetActive(true);
            if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(true);
            if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(true);
        }
    }

    // Wire this to your bottom-right "Next" button's OnClick() event
    public void OnNextSlideButtonClicked()
    {
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
