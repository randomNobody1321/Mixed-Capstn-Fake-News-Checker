using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class BatteryTimeManager : MonoBehaviour
{
    public static BatteryTimeManager Instance;

    [Header("Timer Properties")]
    private float totalTimeElapsed = 0f;
    private float targetMaxRoundTime = 70f; //420 seconds to 16 ; 7 total bars * 60 seconds = 7 Minutes total clock duration
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

    [Header("Parent Container Links")]
    public GameObject centerMonitorPopupUIContainer; // Drag the parent container here!

    [Header("Script Links")]
    public GameScrollManager gameScrollManager; // Drag 'GameObject' (with GameScrollManager) here in the Inspector!


    //private void Awake()
    //{
    //    if (Instance == null) Instance = this;
    //    else Destroy(gameObject);


    //}

    // --- CHANGE YOUR AWAKE METHOD TO LOOK EXACTLY LIKE THIS ---
    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            // Prevent this manager node from being wiped if scene layers reload
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

        // Mathf.Clamp keeps the active bars safely between 0 and 7 so it never goes negative
        int activeBarsRemaining = Mathf.Clamp(7 - Mathf.FloorToInt(totalTimeElapsed / 10f), 0, 7);

        // Calculate dynamic bar reduction intervals every 60 seconds
        //int activeBarsRemaining = 7 - Mathf.FloorToInt(totalTimeElapsed / 2f);
        UpdateBatteryVisuals(activeBarsRemaining);

        if (totalTimeElapsed >= targetMaxRoundTime)
        {
            trackingActive = false;
            EvaluateRunCompletion();
        }
    }

    public void SetTimerPaused(bool isPaused)
    {
        trackingActive = !isPaused;
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
            liveFriendsScoreHUD.text = $"Reliability: {updatedScoreValue}";
        }
    }


    //public void EvaluateRunCompletion()
    //{
    //    trackingActive = false;
    //    if (postGameSummaryCanvasView != null)
    //    {
    //        postGameSummaryCanvasView.SetActive(true);
    //        summaryFriendTallyLabel.text = $"{lastCachedScore}";

    //        // Grade feedback system
    //        if (lastCachedScore >= 50) evaluationPerformanceTierLabel.text = "Viral Sensation! Internet Savvy Grandma!";
    //        else if (lastCachedScore >= 15) evaluationPerformanceTierLabel.text = "Responsible Citizen Netizen";
    //        else evaluationPerformanceTierLabel.text = "Scammed & Out of the Loop!";
    //    }
    //}

    //public void EvaluateRunCompletion()
    //{
    //    trackingActive = false;
    //    Debug.Log("[BATTERY SYSTEM] 7-Bar Round Time Limit reached! Halting game cycle threads.");

    //    if (postGameSummaryCanvasView != null)
    //    {
    //        // 1. Force your canvas layout panel active instantly on screen
    //        postGameSummaryCanvasView.SetActive(true);

    //        // 2. Drive the game window layout block straight to the front layer of your rendering stack
    //        postGameSummaryCanvasView.transform.SetAsLastSibling();

    //        if (summaryFriendTallyLabel != null)
    //        {
    //            summaryFriendTallyLabel.text = $"Total Friends Secured: {lastCachedScore}";
    //        }

    //        // Performance review threshold parameters
    //        if (evaluationPerformanceTierLabel != null)
    //        {
    //            if (lastCachedScore >= 50) evaluationPerformanceTierLabel.text = "RANK: Internet Savvy Genius! Grandma Is Unstoppable!";
    //            else if (lastCachedScore >= 15) evaluationPerformanceTierLabel.text = "RANK: Responsible Citizen Netizen";
    //            else evaluationPerformanceTierLabel.text = "RANK: Completely Scammed & Lost on the Feed!";
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("[CRITICAL OVERLAP ERROR] PostGameSummaryCanvasView reference slot is empty inside BatteryTimeManager script component!");
    //    }
    //}

    // Open BatteryTimeManager.cs and completely replace your EvaluateRunCompletion method
    public void EvaluateRunCompletion()
    {

        trackingActive = false;
        Debug.Log("[BATTERY] Game match completed. Activating End-of-Day Summary...");

        // 1. Activate the parent container layout if assigned
        if (centerMonitorPopupUIContainer != null)
        {
            centerMonitorPopupUIContainer.SetActive(true);

            Transform canvasCheck = centerMonitorPopupUIContainer.transform.parent;
            if (canvasCheck != null)
            {
                canvasCheck.gameObject.SetActive(true);
            }
        }

        // 2. Update the tally text on the summary panel if assigned
        if (summaryFriendTallyLabel != null)
        {
            summaryFriendTallyLabel.text = $"Total Friends Secured: {lastCachedScore}";
        }

        // 3. Hand off ONLY to TriggerEndOfDaySummary (which opens postGameSummaryCanvasView)
        GameScrollManager manager = gameScrollManager != null
            ? gameScrollManager
            : FindFirstObjectByType<GameScrollManager>();

        if (manager != null)
        {
            manager.TriggerEndOfDaySummary();
        }
        else
        {
            Debug.LogError("[BATTERY] Could not find GameScrollManager in the scene!");
        }

        //trackingActive = false;
        //Debug.Log("[BATTERY] Game match completed. Activating summary panels directly via Inspector slots...");

        //// 1. Activate the parent container layout if assigned
        //if (centerMonitorPopupUIContainer != null)
        //{
        //    centerMonitorPopupUIContainer.SetActive(true);

        //    // Automatically grab and wake up the canvas layer sitting right above the container
        //    Transform canvasCheck = centerMonitorPopupUIContainer.transform.parent;
        //    if (canvasCheck != null)
        //    {
        //        canvasCheck.gameObject.SetActive(true);
        //    }
        //}

        //// 2. Instantly activate the summary screen panel via its assigned slot
        //if (postGameSummaryCanvasView != null)
        //{
        //    postGameSummaryCanvasView.SetActive(true);
        //    postGameSummaryCanvasView.transform.SetAsLastSibling(); // Pushes it to the front of the screen layout stack

        //    // 3. Update the text objects safely via your direct Inspector field variable
        //    if (summaryFriendTallyLabel != null)
        //    {
        //        summaryFriendTallyLabel.text = $"Total Friends Secured: {lastCachedScore}";
        //    }

        //    Debug.Log("[SUMMARY ENGINE] Direct assignment override successful. Summary screen open.");
        //}
        //else
        //{
        //    Debug.LogError("[SUMMARY ENGINE] Critical Failure: PostGameSummaryCanvasView assignment missing in the Inspector slot!");
        //}

        //if (gameScrollManager != null)
        //{
        //    gameScrollManager.EvaluateAndTriggerEnding();
        //}
        //else
        //{
        //    // Fallback just in case the Inspector slot wasn't assigned
        //    GameScrollManager foundManager = FindFirstObjectByType<GameScrollManager>();
        //    if (foundManager != null)
        //    {
        //        foundManager.EvaluateAndTriggerEnding();
        //    }
        //    else
        //    {
        //        Debug.LogError("[BATTERY] Could not find GameScrollManager in the scene!");
        //    }
        //}

        //// Update the tally text on the summary panel if assigned
        //if (summaryFriendTallyLabel != null)
        //{
        //    summaryFriendTallyLabel.text = $"Total Friends Secured: {lastCachedScore}";
        //}

        //GameScrollManager manager = gameScrollManager != null
        //    ? gameScrollManager
        //    : FindFirstObjectByType<GameScrollManager>();

        //if (manager != null)
        //{
        //    manager.TriggerEndOfDaySummary();
        //}
        //else
        //{
        //    Debug.LogError("[BATTERY] Could not find GameScrollManager in the scene!");
        //}

        //trackingActive = false;
        //Debug.Log("[BATTERY] Game match completed. Triggering summary screen overlay panels...");

        //// 1. Hard-override lookup for every hidden or visible transform in the scene database
        //Transform[] allObjectsInScene = Resources.FindObjectsOfTypeAll<Transform>();

        //// 2. Force the parent monitor canvases to open first so children can render safely
        //foreach (Transform t in allObjectsInScene)
        //{
        //    if (t.name == "Center Monitor Pop up ui Canvas" || t.name == "Center Monitor Pop up UI Container")
        //    {
        //        t.gameObject.SetActive(true);
        //    }
        //}

        //// 3. Drive the performance scoreboard directly to the active screen layer
        //foreach (Transform t in allObjectsInScene)
        //{
        //    if (t.name == "PostGameSummaryCanvasView")
        //    {
        //        t.gameObject.SetActive(true);
        //        t.SetAsLastSibling(); // Move to top of render layer stack

        //        // Re-render score details
        //        TMP_Text finalScoreLabel = t.GetComponentInChildren<TMP_Text>();
        //        if (finalScoreLabel != null)
        //        {
        //            finalScoreLabel.text = $"Total Friends Secured: {lastCachedScore}";
        //        }

        //        Debug.Log("[SUMMARY ENGINE] Hard override successful. Summary screen forced open.");
        //        return;
        //    }
        //}

        //===============











        //// Direct brute-force path: Force the parent canvas container object ON first
        //if (centerMonitorPopupUIContainer != null)
        //{
        //    centerMonitorPopupUIContainer.SetActive(true);
        //}

        //// Force the summary panel completely active instantly on top of everything
        //if (postGameSummaryCanvasView != null)
        //{
        //    postGameSummaryCanvasView.SetActive(true);
        //    postGameSummaryCanvasView.transform.SetAsLastSibling();

        //    if (summaryFriendTallyLabel != null)
        //    {
        //        summaryFriendTallyLabel.text = $"{lastCachedScore}";
        //    }
        //    Debug.Log("[SUMMARY SCREEN] Successfully forced open!");
        //}

        //// 1. Force the root UI Canvas system to rebuild its sorting order stack instantly
        //Canvas.ForceUpdateCanvases();

        //// 2. Locate and turn ON your overall workspace window folder panel frame first
        //GameObject overallParentFolder = GameObject.Find("Center Monitor Pop up ui Canvas");
        //if (overallParentFolder != null)
        //{
        //    overallParentFolder.SetActive(true);
        //}

        //// 3. Brute force activate the summary screen regardless of scene hierarchy nesting
        //Transform[] everyObjectInScene = Resources.FindObjectsOfTypeAll<Transform>();
        //foreach (Transform t in everyObjectInScene)
        //{
        //    if (t.name == "PostGameSummaryCanvasView")
        //    {
        //        // Force the object active
        //        t.gameObject.SetActive(true);

        //        // Push its rendering row to the front layer of your rendering system stack
        //        t.SetAsLastSibling();

        //        Debug.Log("[BATTERY] PostGameSummaryCanvasView forced active successfully.");
        //        return;
        //    }
        //}

        // 1. Manually find the parent pop-up canvas container by searching the whole scene layout tree
        //Transform[] allTransformsInScene = Resources.FindObjectsOfTypeAll<Transform>();
        //foreach (Transform t in allTransformsInScene)
        //{
        //    if (t.hideFlags == HideFlags.None && t.name == "Center Monitor Pop up UI Container")
        //    {
        //        t.gameObject.SetActive(true);
        //        break;
        //    }
        //}

        //// 2. Safely force the post game performance panel active and push it to the front layer
        //if (postGameSummaryCanvasView != null)
        //{
        //    postGameSummaryCanvasView.SetActive(true);
        //    postGameSummaryCanvasView.transform.SetAsLastSibling();

        //    if (summaryFriendTallyLabel != null)
        //    {
        //        summaryFriendTallyLabel.text = $"{lastCachedScore}";
        //    }
        //    Debug.Log("[SUMMARY SCREEN] Successfully forced open on front layout layer!");
        //}
        //else
        //{
        //    // Absolute fallback search step if your assignment slot got cleared inside the inspector layout
        //    foreach (Transform t in allTransformsInScene)
        //    {
        //        if (t.hideFlags == HideFlags.None && t.name == "PostGameSummaryCanvasView")
        //        {
        //            t.gameObject.SetActive(true);
        //            t.transform.SetAsLastSibling();
        //            break;
        //        }
        //    }
        //}

        //// Turn on the overall workspace folder container safely first
        //if (centerMonitorPopupUIContainer != null)
        //{
        //    centerMonitorPopupUIContainer.SetActive(true);
        //}

        //// Turn on the summary performance window inside it
        //if (postGameSummaryCanvasView != null)
        //{
        //    postGameSummaryCanvasView.SetActive(true);
        //    postGameSummaryCanvasView.transform.SetAsLastSibling();
        //}

        //// 1. Locate and turn ON your overall parent workspace folder canvas view first
        //GameObject parentContainerContainer = GameObject.Find("Center Monitor Pop up UI Container");
        //if (parentContainerContainer != null)
        //{
        //    parentContainerContainer.SetActive(true);
        //}

        //// 2. Now safely activate the summary sub-panel inside it
        //if (postGameSummaryCanvasView != null)
        //{
        //    postGameSummaryCanvasView.SetActive(true);
        //    postGameSummaryCanvasView.transform.SetAsLastSibling();
        //    Debug.Log("[SUMMARY SCREEN] Successfully forced open on front layout layer!");
        //}

        //if (postGameSummaryCanvasView != null)
        //{
        //    // 1. Force the summary panel completely active instantly
        //    postGameSummaryCanvasView.SetActive(true);

        //    // 2. Drive it directly to the front layer of the rendering system so nothing hides it
        //    postGameSummaryCanvasView.transform.SetAsLastSibling();

        //    //if (summaryFriendTallyLabel != null)
        //    //{
        //    //    summaryFriendTallyLabel.text = $"{lastCachedScore}";
        //    //}
        //}
        //else
        //{
        //    // Safe execution fallback: print warning directly to window log
        //    Debug.LogWarning("PostGameSummaryCanvasView slot assignment is missing on BatteryTimeManager component inside _GameManagers scene object!");


        //    GameObject dynamicLookup = GameObject.Find("PostGameSummaryCanvasView");
        //    if (dynamicLookup != null) { dynamicLookup.SetActive(true); dynamicLookup.transform.SetAsLastSibling(); }

        //    //// Fallback lookup: Search your hierarchy directly if the inspector reference broke
        //    //GameObject dynamicLookup = GameObject.Find("PostGameSummaryCanvasView");
        //    //if (dynamicLookup != null)
        //    //{
        //    //    dynamicLookup.SetActive(true);
        //    //    dynamicLookup.transform.SetAsLastSibling();
        //    //}
        //    //else
        //    //{
        //    //    Debug.LogError("[CRITICAL UI ERROR] Could not locate PostGameSummaryCanvasView panel anywhere in scene!");
        //    //}
        //}
    }


}
