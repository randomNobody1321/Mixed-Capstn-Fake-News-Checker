using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatteryTimeManager : MonoBehaviour
{
    public static BatteryTimeManager Instance;

    [Header("Timer Properties")]
    private float totalTimeElapsed = 0f;
    private float targetMaxRoundTime = 420f; // 7 total bars * 60 seconds = 7 Minutes total clock duration
    private bool trackingActive = false;

    [Header("Battery Segment UI Outputs")]
    public Image[] batteryBars; // Wire up exactly 7 sequential bar images inside your UI overlay
    public Color highPowerColor = Color.green;
    public Color deadPowerColor = Color.clear;

    [Header("Persistent HUD Hooks")]
    public TMP_Text liveFriendsScoreHUD;

    [Header("Game Wrap-up Terminal Panel")]
    public GameObject postGameSummaryCanvasView;
    public TMP_Text summaryFriendTallyLabel;
    public TMP_Text evaluationPerformanceTierLabel;

    private int lastCachedScore = 2;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void LaunchTimerEngine()
    {
        totalTimeElapsed = 0f;
        trackingActive = true;
        UpdateBatteryVisuals(7);
    }

    private void Update()
    {
        if (!trackingActive) return;

        totalTimeElapsed += Time.deltaTime;

        // Calculate dynamic bar reduction intervals every 60 seconds
        int activeBarsRemaining = 7 - Mathf.FloorToInt(totalTimeElapsed / 60f);
        UpdateBatteryVisuals(activeBarsRemaining);

        if (totalTimeElapsed >= targetMaxRoundTime)
        {
            trackingActive = false;
            EvaluateRunCompletion();
        }
    }

    private void UpdateBatteryVisuals(int barsToKeepActive)
    {
        for (int i = 0; i < batteryBars.Length; i++)
        {
            if (i < barsToKeepActive)
                batteryBars[i].color = highPowerColor;
            else
                batteryBars[i].color = deadPowerColor; // Empties visual slot
        }
    }

    public void UpdateFriendsScoreUI(int updatedScoreValue)
    {
        lastCachedScore = updatedScoreValue;
        if (liveFriendsScoreHUD != null)
        {
            liveFriendsScoreHUD.text = $"{updatedScoreValue}";
        }
    }

    public void EvaluateRunCompletion()
    {
        trackingActive = false;
        if (postGameSummaryCanvasView != null)
        {
            postGameSummaryCanvasView.SetActive(true);
            summaryFriendTallyLabel.text = $"{lastCachedScore}";

            // Grade feedback system
            if (lastCachedScore >= 50) evaluationPerformanceTierLabel.text = "Viral Sensation! Internet Savvy Grandma!";
            else if (lastCachedScore >= 15) evaluationPerformanceTierLabel.text = "Responsible Citizen Netizen";
            else evaluationPerformanceTierLabel.text = "Scammed & Out of the Loop!";
        }
    }
}
