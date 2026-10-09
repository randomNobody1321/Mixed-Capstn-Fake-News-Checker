using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShiftTimeManager : MonoBehaviour
{
    [Header("Timer Parameters")]
    [Tooltip("Total shift duration in seconds (e.g., 180 seconds = 3 minutes)")]
    public float shiftDurationSeconds = 180f;
    private float timeRemaining;
    private bool isTimerRunning = false;

    [Header("UI Visual Outputs")]
    public TMP_Text timerTextDisplay; // Drag your countdown clock TMP_Text here!
    public GameObject endOfDayStatusPanel; // Drag your End of Day Pop-up Panel window here!

    [Header("Scoreboard Text Elements")]
    public TMP_Text finalTrustScoreText; // Large text layout for the overall trust score
    public Transform logsScrollContentContainer; // 'Content' under your summary scroll view
    public GameObject logMessagePrefab; // A basic UI text prefab to spawn log lines into the list

    [Header("Dependencies")]
    public VerificationReportForm reportForm; // Reference to capture early submissions

    void Start()
    {
        timeRemaining = shiftDurationSeconds;
        isTimerRunning = true;

        if (endOfDayStatusPanel != null)
            endOfDayStatusPanel.SetActive(false);
    }

    void Update()
    {
        if (!isTimerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            timeRemaining = 0;
            UpdateTimerText();
            TriggerEndOfDay();
        }
    }

    void UpdateTimerText()
    {
        if (timerTextDisplay == null) return;

        // Formats seconds into a standard digital clock string (MM:SS)
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerTextDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Called automatically when the clock runs out or manually when ending early
    public void TriggerEndOfDay()
    {
        if (!isTimerRunning) return; // Prevent double execution
        isTimerRunning = false;

        Debug.Log("[SHIFT SYSTEM] Shift finished! Loading End of Day Status...");

        if (endOfDayStatusPanel != null)
        {
            endOfDayStatusPanel.SetActive(true);

            // Force the end of day popup panel to sit at the absolute top of the visual stack
            endOfDayStatusPanel.transform.SetAsLastSibling();

            // --- CHRONICLE LOG CALCULATIONS ON THE SCOREBOARD ---
            CompileFinalScoreboardData();
        }
    }

    void CompileFinalScoreboardData()
    {
        if (reportForm == null) return;

        // 1. Fetch and print the global score tally directly from your Verification Form metrics
        if (finalTrustScoreText != null)
        {
            finalTrustScoreText.text = $"Final Bureau Trust: {reportForm.GetCurrentTrust()}/100";
        }

        // 2. Clear out any sample mock text assets in the container lane cleanly
        if (logsScrollContentContainer != null)
        {
            foreach (Transform child in logsScrollContentContainer)
            {
                Destroy(child.gameObject);
            }

            // 3. Dynamically instantiate a text row packet for every article logged during this shift
            List<string> logsList = reportForm.GetShiftLogReport();
            foreach (string logEntry in logsList)
            {
                GameObject newLogObj = Instantiate(logMessagePrefab, logsScrollContentContainer);
                TMP_Text logText = newLogObj.GetComponentInChildren<TMP_Text>();
                if (logText != null)
                {
                    logText.text = logEntry;
                }
            }
        }
    }
}