using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class GameScrollManager : MonoBehaviour
{
    public static GameScrollManager Instance;

    [Header("Current Run Progression")]
    public int currentDay = 1; // Tracks Day 1 (10 posts), Day 2 (15 posts), Day 3 (20 posts)
    private int currentFriendScore = 2; // Starts at 2

    [Header("Grandma Profile Hooks")]
    public Image grandmaAvatarDisplay;
    public Image grandmaReliabilityMeterImage; // Drag the Reliability Bar Image inside GrandmaProfilePanel here
    public Sprite neutralFaceSprite;
    public Sprite happyFaceSprite;
    public Sprite sadFaceSprite;
    private float reactionTimer = 0f;

    [Header("UI Feed Layout Containers")]
    public Transform doomScrollFeedContainer; // Viewport/Content inside FeedScrollView
    public GameObject postListItemPrefab;    // List row button template asset
    public RectTransform fullPostPopupPanel;  // Main full screen app popup (90% Screen)

    [Header("Full Post Display Nodes")]
    public TMP_Text fullPostTitleText;
    public TMP_Text fullPostContentBody;
    public TMP_Text fullPostTextBeforeUrl; // Drag the new TMP_Text object placed above LinkClickableButton here
    public GameObject linkClickableButton;    // External link button wrapper object
    public TMP_Text fullPostHeadlinerText;

    [Header("External Website Component Targets")]
    public GameObject externalWebsitePopup;
    public TMP_Text websiteHeadlinerText; // Hook for bold headline placed above the article image
    public TMP_Text websiteContentBodyText;   // Populates copy text to mock browser
    public TMP_Text externalLinkButtonLabel;  // Overrides button text to show the custom URL

    [Header("Comments List Component Hooks")]
    public Transform commentsScrollContentContainer; // Viewport/Content inside CommentsSectionScrollView
    public GameObject commentRowTextParagraphPrefab;  // Simple Text (TMP) row prefab asset

    [Header("Direct Verdict Buttons")]
    public Image shareButtonBg;  // Background box graphic that glows
    public Image reportButtonBg; // Background box graphic that glows
    public Sprite shareUpSprite;
    public Sprite shareDownSprite;
    public Sprite reportUpSprite;
    public Sprite reportDownSprite;

    [Header("Helper System")]
    public TMP_Text helperUsesCounterText;
    public Button helperActionButton;
    private int helperUsesRemaining = 3;
    int succeedThreeInARow = 0;

    [Header("Helper System Expression Toggles")]
    public Image helperIconDisplay;       // Image component of your Helper UI widget
    public Sprite helperNormalSprite;     // Helper's default expression artwork
    public Sprite helperActionSprite;     // Temporary 1.5-second expression artwork

    [Header("Active Article Data State")]
    private ArticlePageData openArticleContext;
    private GameObject currentActiveFeedRowInstance;

    [Header("Content Asset Libraries")]
    public List<ArticlePageData> day1ArticlesPool;
    public List<ArticlePageData> day2ArticlesPool;
    public List<ArticlePageData> day3ArticlesPool;

    private bool helperUsedOnCurrentArticle = false;

    [Header("Image Asset Displays")]
    public Image fullPostArticleImageComponent;
    public Image externalWebsiteImageComponent;

    [Header("Desktop System Startup Links")]
    public GameObject appWindowPanelObject; // Drag your 'AppWindowPanel' here

    [Header("HUD Overlay Toggles")]
    public GameObject grandmaProfilePanelObject; // Drag GrandmaProfilePanel here
    public GameObject helperLifelineWidgetObject; // Drag HelperLifelineWidget here

    [Header("Next Day Transition Targets")]
    public GameObject postGameSummaryCanvasView; // Drag PostGameSummaryCanvasView here
    public GameObject day3SummaryCanvasView;    // Drag 'day 3 summary' GameObject here
    public GameObject badEndingCanvasView;
    public GameObject neutralEndingCanvasView;
    public GameObject goodEndingCanvasView;

    [Header("End-of-Day Summary UI Hooks (Days 1 & 2)")]
    public TMP_Text dayResultsHeaderText;        // e.g., "DAY 1 RESULTS"
    public Image reliabilityMeterImage;          // Child Image under "RELIABILITY :"
    public TMP_Text friendsTallyText;            // e.g., "FRIENDS : 15"
    public GameObject topicsAddedHeaderObject;   // Parent "TOPICS ADDED :" GameObject
    public TMP_Text topicsAddedDetailText;       // Child TMP_Text under "TOPICS ADDED :"
    public TMP_Text timeLeftText;                // e.g., "TIME LEFT : 00:00:00"

    [Header("Day 3 Summary UI Hooks")]
    public TMP_Text day3ResultsHeaderText;       // "DAY 3 RESULTS"
    public Image day3ReliabilityMeterImage;      // Child Image under "RELIABILITY :" on Day 3
    public TMP_Text day3FriendsTallyText;        // e.g., "FRIENDS : 15"
    public TMP_Text day3TimeLeftText;            // e.g., "TIME LEFT : 00:00:00"

    [Header("Reliability Meter Sprites (5 Tiers)")]
    [Tooltip("Element 0 = 1 Bar (0-37), Element 1 = 2 Bars (38-75), Element 2 = 3 Bars (76-150), Element 3 = 4 Bars (151-188), Element 4 = 5 Bars (189-227+)")]
    public Sprite[] reliabilityBarSprites;       // Size = 5

    [Header("Game Over Screens")]
    public GameObject GameOverCanvasView; // this is for when the player has 0 reliability; Drag your GameOver screen panel here!

    [Header("Game Over & Return to Day 1 Popups")]
    //public GameObject GameOverCanvasView; // Bad Ending after Day 3 (score 1 to 50)
    //public GameObject finalGameOverCanvasView;
    public GameObject desktopBackgroundPanelObject; // Drag 'DesktopBackgroundPanel' here
    public GameObject batteryHudPanelObject;        // Drag your Battery UI parent GameObject here
    public ReturnToDayOnePopupManager returnToDayOnePopup; // Drag the popup with ReturnToDayOnePopupManager here

    [Header("End Game Screens")]
    public GameObject finalGameOverCanvasView; // Drag your GameOver screen panel here!

    [Header("Thumbs Interaction Custom Elements")]
    public Image thumbsUpIconImage;   // Drag the Image component of your Thumbs Up button here
    public Image thumbsDownIconImage; // Drag the Image component of your Thumbs Down button here
    public Sprite thumbsUpUnpressedSprite;
    public Sprite thumbsUpPressedSprite;
    public Sprite thumbsDownUnpressedSprite;
    public Sprite thumbsDownPressedSprite;

    [Header("Onboarding Tutorial")]
    public GameObject onboardingPanelObject; // Drag your Onboarding GameObject here

    [Header("Captcha Subsystem Elements")]
    public GameObject captchaPanelObject;     // Drag your 'CaptchaPanel' canvas wrapper here
    public TMP_Text captchaInstructionLabel;  // Displays "Are you a robot?" text strings
    public Image[] captchaNumPadButtonBgs;    // Element 0=Key1, 1=Key2 ... up to Element 8=Key9
    public Sprite[] numPadUpSprites;       // Size = 9 (Element 0=Key1 Up ... Element 8=Key9 Up)
    public Sprite[] numPadDownSprites;     // Size = 9 (Element 0=Key1 Down ... Element 8=Key9 Down)
    public Color captchaDefaultColor = Color.white;
    public Color captchaHighlightColor = new Color(0.5f, 1f, 0.5f, 1f); // Light Green
    public Color captchaFailureColor = new Color(1f, 0.3f, 0.3f, 1f);   // Red Flash

    // Captcha State Management
    private int currentBaseSequenceLength = 3;  // Starts at 3 numbers, escalates dynamically
    private List<int> currentGeneratedPattern = new List<int>();
    private List<int> playerInputPattern = new List<int>();
    private bool isPlayingCaptchaSequence = false;
    private bool canProcessVerdict = true;

    [SerializeField] private Color captchaPlayerTapColor = new Color(0.56f, 0.93f, 0.56f, 1f); // Light Green

    private bool isRestartingRun = false;
    private bool lostDueToZeroReliability = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ResetAllArticleMemoryStates(day1ArticlesPool);
        ResetAllArticleMemoryStates(day2ArticlesPool);
        ResetAllArticleMemoryStates(day3ArticlesPool);

        InitializeDayLoadout(currentDay);
        UpdateScoreboardDisplay();

        if (captchaPanelObject != null) captchaPanelObject.SetActive(false);
    }

    private void Update()
    {
        if (reactionTimer > 0f)
        {
            reactionTimer -= Time.deltaTime;
            if (reactionTimer <= 0f && grandmaAvatarDisplay != null)
            {
                grandmaAvatarDisplay.sprite = neutralFaceSprite;
            }
        }
    }

    public void LaunchApplicationFromDesktop()
    {
        if (appWindowPanelObject != null)
        {
            // Keep AppWindowPanel hidden while Onboarding is active
            appWindowPanelObject.SetActive(false);
            if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(false);
            if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(false);

            //appWindowPanelObject.SetActive(true);
            //if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(true);
            //if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(true);

            InitializeDayLoadout(1);

            if (BatteryTimeManager.Instance != null)
            {
                BatteryTimeManager.Instance.LaunchTimerEngine();
            }

            if (appWindowPanelObject != null)
            {
                //if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(false);
                //if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(false);

                InitializeDayLoadout(1);

                if (onboardingPanelObject != null)
                {
                    appWindowPanelObject.SetActive(true);
                    if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(true);
                    if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(true);

                    onboardingPanelObject.SetActive(true);
                    onboardingPanelObject.transform.SetAsLastSibling();
                }
                else
                {
                    if (BatteryTimeManager.Instance != null)
                    {
                        BatteryTimeManager.Instance.LaunchTimerEngine();
                    }
                    appWindowPanelObject.SetActive(true);
                    if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(true);
                    if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(true);
                }
            }     
        }
    }

    private void ResetAllArticleMemoryStates(List<ArticlePageData> articlePoolToClean)
    {
        if (articlePoolToClean == null) return;
        foreach (var article in articlePoolToClean)
        {
            if (article != null)
            {
                article.playerLikedThisTemporaryState = false;
                article.playerDislikedThisTemporaryState = false;
                article.helperWasUsedOnThisArticleState = false;
            }
        }
    }

    public void InitializeDayLoadout(int dayNumber)
    {

        currentDay = dayNumber;
        helperUsesRemaining = 3;
        UpdateHelperUI();

        // Clear existing rows cleanly
        for (int i = doomScrollFeedContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(doomScrollFeedContainer.GetChild(i).gameObject);
        }

        foreach (Transform child in doomScrollFeedContainer) Destroy(child.gameObject);

        List<ArticlePageData> activePool = (dayNumber == 1) ? day1ArticlesPool :
                                           (dayNumber == 2) ? day2ArticlesPool : day3ArticlesPool;

        if (activePool == null || activePool.Count == 0)
        {
            Debug.LogWarning($"[FEED SHUFFLE] Active pool for Day {dayNumber} is empty!");
            return;
        }
        // 1. Copy into a new list so your Inspector pools (day1/2/3ArticlesPool) stay untouched
        List<ArticlePageData> shuffledArticles = new List<ArticlePageData>(activePool);

        // 2. Fisher-Yates shuffle to randomize top-to-bottom order
        for (int i = shuffledArticles.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            ArticlePageData temp = shuffledArticles[i];
            shuffledArticles[i] = shuffledArticles[randomIndex];
            shuffledArticles[randomIndex] = temp;
        }
        Debug.Log($"[FEED SHUFFLE] Day {dayNumber} first post is now: {shuffledArticles[0].title}");

        // 2. Instantiate and explicitly enforce sibling order
        for (int i = 0; i < shuffledArticles.Count; i++)
        {
            ArticlePageData article = shuffledArticles[i];
            GameObject row = Instantiate(postListItemPrefab, doomScrollFeedContainer);
            row.transform.SetAsLastSibling();
            row.GetComponentInChildren<TMP_Text>().text = article.title;

            Button btn = row.GetComponent<Button>();
            btn.onClick.AddListener(() => OpenFullPostMode(article, row));

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.RegisterButtonAudio(btn);
            }

        }

        //// 3. Spawn feed rows using the randomized list
        //foreach (var article in activePool)
        //{
        //    GameObject row = Instantiate(postListItemPrefab, doomScrollFeedContainer);
        //    row.GetComponentInChildren<TMP_Text>().text = article.title;

        //    Button btn = row.GetComponent<Button>();
        //    btn.onClick.AddListener(() => OpenFullPostMode(article, row));
        //}
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(doomScrollFeedContainer.GetComponent<RectTransform>());
    }

    public void OpenFullPostMode(ArticlePageData data, GameObject sourceRowInstance)
    {
        canProcessVerdict = true;
        openArticleContext = data;
        currentActiveFeedRowInstance = sourceRowInstance;
        helperUsedOnCurrentArticle = false;

        if (captchaPanelObject != null) captchaPanelObject.SetActive(false);

        if (fullPostHeadlinerText != null) fullPostHeadlinerText.text = data.headliner;
        if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
        if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;

        if (shareButtonBg != null && shareUpSprite != null)
        {
            shareButtonBg.sprite = shareUpSprite;
        }
        if (reportButtonBg != null && reportUpSprite != null)
        {
            reportButtonBg.sprite = reportUpSprite;
        }
        //if (shareButtonBg != null)
        //{
        //    //shareButtonBg.color = Color.purple;
        //    if (shareUpSprite != null) shareButtonBg.sprite = shareUpSprite;
        //}
        //if (reportButtonBg != null)
        //{
        //    //reportButtonBg.color = Color.purple;
        //    if (reportUpSprite != null) reportButtonBg.sprite = reportUpSprite;
        //}

        fullPostTitleText.text = data.title;
        fullPostContentBody.text = data.bodyText;

        fullPostPopupPanel.gameObject.SetActive(true);
        LoadCommentsToView(data);

        //commented out since they made share and report button background white I forgot to remove this earlier
        //shareButtonBg.color = Color.purple;
        //reportButtonBg.color = Color.purple;

        if (fullPostTextBeforeUrl != null)
        {
            bool hasBeforeUrlText = !string.IsNullOrWhiteSpace(data.textBeforeUrl);
            fullPostTextBeforeUrl.gameObject.SetActive(hasBeforeUrlText);
            fullPostTextBeforeUrl.text = hasBeforeUrlText ? data.textBeforeUrl : "";
        }

        if (data.hasExternalLink)
        {
            linkClickableButton.SetActive(true);
            externalLinkButtonLabel.text = $"https://www.{data.articleID.ToLower().Trim()}.org";
        }
        else
        {
            linkClickableButton.SetActive(false);
        }

        if (appWindowPanelObject != null) appWindowPanelObject.SetActive(false);
        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(true);

        //if (shareButtonBg != null) shareButtonBg.color = Color.purple;
        //if (reportButtonBg != null) reportButtonBg.color = Color.purple;

        if (data != null && data.helperWasUsedOnThisArticleState)
        {
            bool isLegit = data.cheatSheet.approveAndPublish;
            if (isLegit && shareButtonBg != null) shareButtonBg.color = Color.green;
            else if (!isLegit && reportButtonBg != null) reportButtonBg.color = Color.red;
            Debug.Log("[HELPER DISPLAY] Restored saved highlight state.");
        }

        if (data.images != null && data.images.Length > 0 && data.images[0].imageSprite != null)
        {
            fullPostArticleImageComponent.gameObject.SetActive(true);
            fullPostArticleImageComponent.sprite = data.images[0].imageSprite;
        }
        else
        {
            fullPostArticleImageComponent.gameObject.SetActive(false);
        }

        RefreshThumbsVisualsFromArticleData();
    }

    public void CloseFullPostAndReturnToDesktop()
    {
        if (!canProcessVerdict)
        {
            Debug.LogWarning("[CHEAT SHIELD] Back Button blocked during verdict delay!");
            return;
        }

        if (captchaPanelObject != null) captchaPanelObject.SetActive(false);
        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);
        
if (appWindowPanelObject != null) appWindowPanelObject.SetActive(true);
        openArticleContext = null;
        currentActiveFeedRowInstance = null;
    }
    public void CloseExternalWebsiteAndReturnToPost()
    {
        if (externalWebsitePopup != null) externalWebsitePopup.SetActive(false);
        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(true);
    }

    public void CloseCaptchaBackButtonReturnToPost()
    {
        StopAllCoroutines();
        isPlayingCaptchaSequence = false; // Reset lock if closed mid-flash
        //fullPostPopupPanel.gameObject.SetActive(true); //turn it on
        currentBaseSequenceLength = 3;
        //currentGeneratedPattern.Clear();
        //playerInputPattern.Clear();
        //playerInputPattern.Clear();
        if (captchaPanelObject != null) captchaPanelObject.SetActive(false);
        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(true);
    }
    // --- NEW CAPTCHA INTERCEPT ENGINE (RULE #1, #5, #6) ---
    public void ClickExternalLink()
    {
        if (openArticleContext == null) return;
        // Roll 50/50 Chance Every Time link is pressed
        float randomRoll = Random.Range(0f, 100f);
        if (randomRoll <= 50f && captchaPanelObject != null)
        {
            //fullPostPopupPanel.gameObject.SetActive(false); //turn it off
            Debug.Log("[CAPTCHA TRIGGERED] 50/50 check failed! Launching sequence verification matrix...");
            LaunchCaptchaMinigame();
        }
        else
        {
            Debug.Log("[CAPTCHA SKIPPED] 50/50 check passed! Bypassing security filter...");
            ForceBypassOpenExternalWebsite();
        }
    }
    private void LaunchCaptchaMinigame()
    {
        if (captchaPanelObject == null) return;
        if (captchaInstructionLabel != null) captchaInstructionLabel.text = "Are you a robot?";
        captchaPanelObject.SetActive(true);
        // Keep preview screens safely locked in place while blocking website overlay
        //if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);
        //if (externalWebsitePopup != null) externalWebsitePopup.SetActive(false);

        //if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(false);
        //if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(false);

        // Stop any previous sequence or failure flash before starting a new one
        StopAllCoroutines();
        isPlayingCaptchaSequence = false; // Guarantees input is never stuck locked!

        // Reset all button colors to default in case a previous coroutine was interrupted mid-flash
        for (int i = 0; i < captchaNumPadButtonBgs.Length; i++)
        {
            if (captchaNumPadButtonBgs[i] != null)
            {
                captchaNumPadButtonBgs[i].color = captchaDefaultColor;
                if (numPadUpSprites != null && i < numPadUpSprites.Length && numPadUpSprites[i] != null)
                {
                    captchaNumPadButtonBgs[i].sprite = numPadUpSprites[i];
                }
            }
        }

        StartCoroutine(PlaySequencePatternRoutine(currentBaseSequenceLength));
    }
    private IEnumerator PlaySequencePatternRoutine(int length)
    {
        isPlayingCaptchaSequence = true;
        currentGeneratedPattern.Clear();
        playerInputPattern.Clear();
        // Generate dynamic random pattern indexes (Keys 1-9 correspond to array indexes 0-8)
        for (int i = 0; i < length; i++)
        {
            currentGeneratedPattern.Add(Random.Range(1, 10));
            Debug.Log("number: " + currentGeneratedPattern);
        }
        yield return new WaitForSeconds(0.4f);
        // Sequentially highlight the keys
        foreach (int keyNumber in currentGeneratedPattern)
        {
            int targetIndex = keyNumber - 1;
            if (targetIndex >= 0 && targetIndex < captchaNumPadButtonBgs.Length)
            {
                if (captchaNumPadButtonBgs[targetIndex] != null)
                    captchaNumPadButtonBgs[targetIndex].color = captchaHighlightColor;
                Debug.Log("number: " + captchaNumPadButtonBgs[targetIndex]);
                yield return new WaitForSeconds(0.5f); // 1.0s exact highlight buffer per number
                if (captchaNumPadButtonBgs[targetIndex] != null)
                    captchaNumPadButtonBgs[targetIndex].color = captchaDefaultColor;
                Debug.Log("number: " + captchaNumPadButtonBgs[targetIndex]);
                yield return new WaitForSeconds(0.2f); // Quick interval blank buffer between same numbers
            }
        }
        isPlayingCaptchaSequence = false;
        //Debug.Log("number: " + captchaNumPadButtonBgs[targetIndex]);
        Debug.Log("[CAPTCHA MATRIX] Highlighting finished. Awaiting input registration sequence...");
    }
    // Connect this to your 9 individual button clicks (Pass numbers 1 through 9 inside inspector properties)
    public void RegisterPlayerNumPadInput(int keyedValue)
    {
        Debug.Log($"[NUMPAD CLICKED] Key: {keyedValue} | isPlaying: {isPlayingCaptchaSequence} | panelNull: {captchaPanelObject == null} | panelActive: {(captchaPanelObject != null && captchaPanelObject.activeSelf)}");


        if (isPlayingCaptchaSequence || captchaPanelObject == null || !captchaPanelObject.activeSelf) return;

        // Prevent registering extra inputs while a failure animation is playing
        if (playerInputPattern.Count >= currentGeneratedPattern.Count) return;

        int tappedIndex = keyedValue - 1;
        playerInputPattern.Add(keyedValue);
        int currentInputIndex = playerInputPattern.Count - 1;

        Debug.Log($"Expected: {currentGeneratedPattern[currentInputIndex]} | Pressed: {keyedValue} (Index: {currentInputIndex})");


        //int currentInputIndex = playerInputPattern.Count;
        Debug.Log("pattern input num: " + currentGeneratedPattern[currentInputIndex]);
        Debug.Log("player input num: " + keyedValue);

        // Immediate sequence check
        if (playerInputPattern[currentInputIndex] != currentGeneratedPattern[currentInputIndex])
        {
            StartCoroutine(FlashPlayerSelectedButtonRoutine(tappedIndex));
            // Failure Loop Penalty
            StartCoroutine(FlashCaptchaFailureRoutine());
            return;
        }

        // Highlight the pressed button in light green
        StartCoroutine(FlashPlayerSelectedButtonRoutine(keyedValue - 1));

        // Win Condition
        if (playerInputPattern.Count == currentGeneratedPattern.Count)
        {
            //StopAllCoroutines();
            isPlayingCaptchaSequence = false; // Reset lock if closed mid-flash
                                              //fullPostPopupPanel.gameObject.SetActive(true); //turn it on
            currentBaseSequenceLength = 3;

            //Debug.Log("[CAPTCHA SUCCESS] Access authorized. Sequence difficulty permanently escalated +1.");
            //currentBaseSequenceLength++; // Increases baseline sequence length for subsequent captchas
            //commented out due to no sense in increasing idfficulty for right input
            playerInputPattern.Clear();

            StartCoroutine(CompleteCaptchaAfterTapRoutine());

            if (captchaPanelObject != null) captchaPanelObject.SetActive(false);
            ForceBypassOpenExternalWebsite();
        }
    }

    private IEnumerator CompleteCaptchaAfterTapRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        if (captchaPanelObject != null) captchaPanelObject.SetActive(false);
        ForceBypassOpenExternalWebsite();
    }

    private IEnumerator FlashPlayerSelectedButtonRoutine(int buttonIndex)
    {
        if (buttonIndex >= 0 && buttonIndex < captchaNumPadButtonBgs.Length && captchaNumPadButtonBgs[buttonIndex] != null)
        {
            Image btnImage = captchaNumPadButtonBgs[buttonIndex];

            if (numPadDownSprites != null && buttonIndex < numPadDownSprites.Length && numPadDownSprites[buttonIndex] != null)
            {
                btnImage.sprite = numPadDownSprites[buttonIndex];
            }

            if (!isPlayingCaptchaSequence) btnImage.color = captchaPlayerTapColor;

            yield return new WaitForSeconds(0.25f);

            if (numPadUpSprites != null && buttonIndex < numPadUpSprites.Length && numPadUpSprites[buttonIndex] != null)
            {
                btnImage.sprite = numPadUpSprites[buttonIndex];
            }

            if (!isPlayingCaptchaSequence) btnImage.color = captchaDefaultColor;

            captchaNumPadButtonBgs[buttonIndex].color = captchaPlayerTapColor;
            yield return new WaitForSeconds(0.25f);

            // Only reset to default if the failure flash hasn't taken over
            if (!isPlayingCaptchaSequence)
            {
                captchaNumPadButtonBgs[buttonIndex].color = captchaDefaultColor;
            }
        }
    }
        
    private IEnumerator FlashCaptchaFailureRoutine()
    {
        isPlayingCaptchaSequence = true; // Lock player input during red error flash
        Debug.LogWarning("[CAPTCHA FAILURE] Wrong input string entered! Resetting pattern sequence length +1.");
        foreach (Image bg in captchaNumPadButtonBgs)
        {
            if (bg != null) bg.color = captchaFailureColor;
        }
        yield return new WaitForSeconds(0.8f);
        foreach (Image bg in captchaNumPadButtonBgs)
        {
            if (bg != null) bg.color = captchaDefaultColor;
        }
        if(currentBaseSequenceLength <= 9) //this is the cap of the max number of sequences/combinations
        {//set to 9 instead of 10 as it treats it like its 1 higher due to the ++ below this line
            currentBaseSequenceLength++; // Add penalty number onto difficulty progression tracker
        }

        StartCoroutine(PlaySequencePatternRoutine(currentBaseSequenceLength));
    }
    private void ForceBypassOpenExternalWebsite()
    {
        if (externalWebsitePopup != null && openArticleContext != null)
        {
            bool imageOnlyMode = openArticleContext.cheatSheet != null && openArticleContext.cheatSheet.onlyDisplayImage;
            //bool hasValidImage = openArticleContext.images != null && openArticleContext.images.Length > 0 && openArticleContext.images[0].imageSprite != null;
            //for 1 image

            Debug.Log($"[EXTERNAL CHECK] Asset Name: '{openArticleContext.name}' | Title: '{openArticleContext.title}' | ExternalImages Count: {(openArticleContext.externalLinkImages != null ? openArticleContext.externalLinkImages.Length : -1)}");

            //for 2 images
            // Strictly check externalLinkImages because that is what gets assigned below
            bool hasValidImage = openArticleContext.externalLinkImages != null
                                 && openArticleContext.externalLinkImages.Length > 0
                                 && openArticleContext.externalLinkImages[0] != null
                                 && openArticleContext.externalLinkImages[0].imageSprite != null;

            // 1. Headline visibility & text
            if (websiteHeadlinerText != null)
            {
                websiteHeadlinerText.gameObject.SetActive(!imageOnlyMode);
                if (!imageOnlyMode)
                {
                    websiteHeadlinerText.text = $"{openArticleContext.headliner}";
                }
            }

            // 2. External Body Text visibility & text (90px top margin removed!)
            if (websiteContentBodyText != null)
            {
                websiteContentBodyText.gameObject.SetActive(!imageOnlyMode);
                if (!imageOnlyMode)
                {
                    string rawExternalBody = string.IsNullOrEmpty(openArticleContext.externalLinkBodyText)
                        ? ""
                        : openArticleContext.externalLinkBodyText;
                    string cleanBody = rawExternalBody.Replace("[LINK]", "").Replace("[PUPPY]", "");
                    websiteContentBodyText.text = cleanBody;
                    websiteContentBodyText.margin = Vector4.zero;
                }
            }

            //// 3. Image display
            //if (hasValidImage)
            //{
            //    if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);

            //    if (externalWebsiteImageComponent != null)
            //    {   
            //        externalWebsiteImageComponent.gameObject.SetActive(true);
            //        externalWebsiteImageComponent.sprite = openArticleContext.externalLinkImages[0].imageSprite;
            //    }
            //}
            //else
            //{
            //    if (externalWebsiteImageComponent != null)
            //    {
            //        externalWebsiteImageComponent.gameObject.SetActive(false);
            //    }
            //}

            // ALWAYS close FullPostPopupPanel when opening the External Website popup
            if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);

            // 3. Image display
            if (hasValidImage)
            {
                if (externalWebsiteImageComponent != null)
                {
                    externalWebsiteImageComponent.gameObject.SetActive(true);
                    externalWebsiteImageComponent.sprite = openArticleContext.externalLinkImages[0].imageSprite;
                }
            }
            else
            {
                if (externalWebsiteImageComponent != null)
                {
                    externalWebsiteImageComponent.gameObject.SetActive(false);
                }
            }

            if (captchaPanelObject != null) captchaPanelObject.SetActive(false);
            externalWebsitePopup.SetActive(true);

            // Rebuild Content layout cleanly
            if (websiteContentBodyText != null && websiteContentBodyText.transform.parent != null)
            {
                RectTransform contentRect = websiteContentBodyText.transform.parent.GetComponent<RectTransform>();
                if (contentRect != null)
                {
                    Canvas.ForceUpdateCanvases();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
                }
            }
            if (captchaPanelObject != null) captchaPanelObject.SetActive(false);
            externalWebsitePopup.SetActive(true);
        }
    }
    private void LoadCommentsToView(ArticlePageData data)
    {
        if (commentsScrollContentContainer == null || commentRowTextParagraphPrefab == null) return;
        foreach (Transform child in commentsScrollContentContainer) Destroy(child.gameObject);
        if (data.customComments != null && data.customComments.Count > 0)
        {
            foreach (string commentString in data.customComments)
            {
                GameObject commentRow = Instantiate(commentRowTextParagraphPrefab, commentsScrollContentContainer);
                TMP_Text targetText = commentRow.GetComponentInChildren<TMP_Text>();
                if (targetText != null)
                {
                    targetText.text = commentString;
                    targetText.margin = new Vector4(95f, 0f, 0f, 0f); // 95px layout margin clearance width
                }
            }
        }
        else
        {
            GameObject emptyNoticeRow = Instantiate(commentRowTextParagraphPrefab, commentsScrollContentContainer);
            TMP_Text targetText = emptyNoticeRow.GetComponentInChildren<TMP_Text>();
            if (targetText != null)
            {
                targetText.text = "No comments on this post.";
                targetText.margin = new Vector4(95f, 0f, 0f, 0f);
            }
        }
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(commentsScrollContentContainer.GetComponent<RectTransform>());
    }
    // --- EMERGENCY ESCAPE HATCH B: CAPTCHA AUTOMATICALLY BYPASSED ON HELPER SELECTION ---
    public void UseHelperLifeline()
    {
        if (fullPostPopupPanel == null || !fullPostPopupPanel.gameObject.activeSelf)
        {
            Debug.Log("[HELPER INTERCEPT] Click blocked because FullPostPopupPanel is currently closed or hidden.");
            return;
        }
        if (openArticleContext == null) return;
        if (openArticleContext.helperWasUsedOnThisArticleState)
        {
            Debug.LogWarning("[HELPER INTERCEPT] Click blocked: Helper was ALREADY used on this specific article data profile!");
            return;
        }

        if (helperUsesRemaining <= 0)
        {
            Debug.LogWarning("[HELPER INTERCEPT] Click blocked: No helper charges remaining!");
            return;
        }
        Debug.Log("helper used");
        openArticleContext.helperWasUsedOnThisArticleState = true;
        helperUsesRemaining--;
        UpdateHelperUI();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayHelperClick();
        }

        if (helperIconDisplay != null && helperActionSprite != null)
        {
            StartCoroutine(HelperExpressionRoutine());
        }
        // Emergency Escape Hatch: Shuts off the captcha loop if active
        if (captchaPanelObject != null && captchaPanelObject.activeSelf)
        {
            //fullPostPopupPanel.gameObject.SetActive(true); //turn it on
            Debug.Log("[CAPTCHA BYPASS] Emergency Escape Option triggered via Helper choice.");
            StopAllCoroutines();
            isPlayingCaptchaSequence = false;
            captchaPanelObject.SetActive(false);
        }
        bool isLegit = openArticleContext.cheatSheet.approveAndPublish;
        if (isLegit && shareButtonBg != null) shareButtonBg.color = Color.green;
        else if (!isLegit && reportButtonBg != null) reportButtonBg.color = Color.green;
    }
    private IEnumerator HelperExpressionRoutine()
    {
        helperIconDisplay.sprite = helperActionSprite;
        yield return new WaitForSeconds(1.5f);
        if (helperIconDisplay != null && helperNormalSprite != null)
        {
            helperIconDisplay.sprite = helperNormalSprite;
        }
    }
    // --- EMERGENCY ESCAPE HATCH A: CAPTCHA SHUTS OFF ON VERDICT SELECTION ---
    public void ProcessPlayerVerdict(bool choseShare)
    {
        shareButtonBg.color = Color.white;
        reportButtonBg.color = Color.white;

        if (!canProcessVerdict || openArticleContext == null) return;
        canProcessVerdict = false;

        // Swap Share or Report to its pressed sprite
        if (choseShare)
        {
            if (shareButtonBg != null && shareDownSprite != null)
                shareButtonBg.sprite = shareDownSprite;
        }
        else
        {
            if (reportButtonBg != null && reportDownSprite != null)
                reportButtonBg.sprite = reportDownSprite;
        }

        if (captchaPanelObject != null && captchaPanelObject.activeSelf)
        {
            StopAllCoroutines();
            isPlayingCaptchaSequence = false;
            captchaPanelObject.SetActive(false);
        }

        //// Swap pressed verdict button to Down-State sprite
        //if (choseShare)
        //{
        //    if (shareButtonBg != null && shareDownSprite != null)
        //        shareButtonBg.sprite = shareDownSprite;
        //}
        //else
        //{
        //    if (reportButtonBg != null && reportDownSprite != null)
        //        reportButtonBg.sprite = reportDownSprite;
        //}

        // Emergency Escape Hatch: Shuts off the captcha loop if active
        if (captchaPanelObject != null && captchaPanelObject.activeSelf)
        {
            Debug.Log("[CAPTCHA BYPASS] Emergency Escape Option triggered via Verdict choice.");
            StopAllCoroutines();
            isPlayingCaptchaSequence = false;
            captchaPanelObject.SetActive(false);
        }
        appWindowPanelObject.SetActive(true);
        bool isArticleLegit = openArticleContext.cheatSheet.approveAndPublish;
        bool isCorrect = (choseShare == isArticleLegit);
        if (isCorrect)
        {
            currentFriendScore += 5;
            succeedThreeInARow++;
            Debug.Log("+1 to succeedThreeInARow: " + succeedThreeInARow);

            if (succeedThreeInARow == 3)
            {
                helperUsesRemaining++;
                succeedThreeInARow = 0;
                UpdateHelperUI();
                Debug.LogWarning("Gained 1 helper use due to 3 success in a row!");
            }

            TriggerGrandmaExpression(happyFaceSprite);
            if (AudioManager.Instance != null) AudioManager.Instance.PlayGrannyRightAnswer();

            if (currentFriendScore % 15 == 0)
            {
                helperUsesRemaining++;
                UpdateHelperUI();

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayHelperClick();
                }

            }
        }
        else
        {
            succeedThreeInARow = 0;
            currentFriendScore = Mathf.Max(0, currentFriendScore - 3);
            TriggerGrandmaExpression(sadFaceSprite);
            if (AudioManager.Instance != null) AudioManager.Instance.PlayGrannyWrongAnswer();

        }
        UpdateScoreboardDisplay();
        if (currentFriendScore <= 0)
        {
            Invoke(nameof(CloseAndAnimateFeedShift), 1.5f);
            //Invoke(nameof(TriggerZeroReliabilityGameOverPopup), 1.55f);
            Invoke(nameof(EvaluateAndTriggerEnding), 1.55f);
            return;
        }
        Invoke(nameof(CloseAndAnimateFeedShift), 1.5f);
    }

    public void TriggerZeroReliabilityGameOverPopup()
    {
        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
        }

        HideAllGameplayAndEndingPanels();

        if (returnToDayOnePopup != null)
        {
            returnToDayOnePopup.OpenPopup(isZeroReliabilityGameOver: true);
        }
    }

    // Hook this method to the "Next" / "Continue" button on ALL 3 Ending screens (Bad, Neutral, Good)
    public void OnEndingProceedButtonClicked()
    {
        if (isRestartingRun) return;

        HideAllGameplayAndEndingPanels();

        if (returnToDayOnePopup != null)
        {
            returnToDayOnePopup.OpenPopup(isZeroReliabilityGameOver: lostDueToZeroReliability);
            //returnToDayOnePopup.OpenPopup(isZeroReliabilityGameOver: false);
        }
    }

    private void ResetCoreRunState()
    {
        isRestartingRun = false;
        lostDueToZeroReliability = false;


        CancelInvoke();
        StopAllCoroutines();
        isPlayingCaptchaSequence = false;
        canProcessVerdict = true;
        currentBaseSequenceLength = 3;
        succeedThreeInARow = 0;
        openArticleContext = null;
        currentActiveFeedRowInstance = null;

        currentDay = 1;
        currentFriendScore = 2;
        helperUsesRemaining = 3;

        ResetAllArticleMemoryStates(day1ArticlesPool);
        ResetAllArticleMemoryStates(day2ArticlesPool);
        ResetAllArticleMemoryStates(day3ArticlesPool);

        InitializeDayLoadout(1);
        UpdateScoreboardDisplay();
        UpdateHelperUI();
    }

    // Called when player clicks YES >>
    public void RestartRunToOnboarding()
    {
        isRestartingRun = true;

        HideAllGameplayAndEndingPanels();
        //if (desktopBackgroundPanelObject != null) desktopBackgroundPanelObject.SetActive(false);
        if (desktopBackgroundPanelObject != null) desktopBackgroundPanelObject.SetActive(true);

        ResetCoreRunState();

        // Immediately launch Day 1 Onboarding (saves 1 click from the desktop)
        LaunchApplicationFromDesktop();

        // Guarantee ReturnToDayOnePopup stays hidden after Onboarding opens
        if (returnToDayOnePopup != null)
        {
            returnToDayOnePopup.gameObject.SetActive(false);
        }

        // Clear the guard on the next frame after all UI events finish
        Invoke(nameof(ClearRestartGuard), 0.1f);
    }

    private void ClearRestartGuard()
    {
        isRestartingRun = false;
    }

    // Called when player clicks NO >>
    public void RestartRunToDesktop()
    {
        HideAllGameplayAndEndingPanels();
        if (onboardingPanelObject != null) onboardingPanelObject.SetActive(false);

        ResetCoreRunState();

        // Turn on DesktopBackgroundPanel and keep ONLY the Battery HUD visible at 420f (7 bars)
        if (desktopBackgroundPanelObject != null)
        {
            desktopBackgroundPanelObject.SetActive(true);
        }

        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.ResetTimerToFullPaused();
        }

        if (batteryHudPanelObject != null)
        {
            batteryHudPanelObject.SetActive(false);
            batteryHudPanelObject.transform.SetAsLastSibling();
        }
    }

    private void CloseAndAnimateFeedShift()
    {
        CancelInvoke(nameof(CloseAndAnimateFeedShift));
        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);
        if (currentActiveFeedRowInstance != null)
        {
            Destroy(currentActiveFeedRowInstance);
        }
        openArticleContext = null;
        currentActiveFeedRowInstance = null;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(doomScrollFeedContainer.GetComponent<RectTransform>());
        if (doomScrollFeedContainer.childCount <= 1)
        {
            Debug.Log("[FEED MANAGER] All daily cases cleared! Transferring execution to Summary Engine...");
            if (BatteryTimeManager.Instance != null)
            {
                BatteryTimeManager.Instance.EvaluateRunCompletion();
            }
        }
        canProcessVerdict = true;
    }
    private void TriggerGrandmaExpression(Sprite moodSprite)
    {
        if (grandmaAvatarDisplay == null) return;
        grandmaAvatarDisplay.sprite = moodSprite;
        reactionTimer = 1.5f;
    }
    private void UpdateScoreboardDisplay()
    {
        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
        }

        if (grandmaReliabilityMeterImage != null)
        {
            Sprite tierSprite = GetReliabilityTierSprite(currentFriendScore);
            if (tierSprite != null)
            {
                grandmaReliabilityMeterImage.sprite = tierSprite;
            }
        }
    }
    private void UpdateHelperUI()
    {
        if (helperUsesCounterText != null)
            helperUsesCounterText.text = $"Uses: {helperUsesRemaining}";
    }
    public void ToggleThumbsUpInteraction()
    {
        if (openArticleContext == null) return;
        openArticleContext.playerLikedThisTemporaryState = !openArticleContext.playerLikedThisTemporaryState;
        if (openArticleContext.playerLikedThisTemporaryState)
        {
            openArticleContext.playerDislikedThisTemporaryState = false;
        }
        RefreshThumbsVisualsFromArticleData();
    }
    public void ToggleThumbsDownInteraction()
    {
        if (openArticleContext == null) return;
        openArticleContext.playerDislikedThisTemporaryState = !openArticleContext.playerDislikedThisTemporaryState;
        if (openArticleContext.playerDislikedThisTemporaryState)
        {
            openArticleContext.playerLikedThisTemporaryState = false;
        }
        RefreshThumbsVisualsFromArticleData();
    }
    public void RefreshThumbsVisualsFromArticleData()
    {
        if (openArticleContext == null)
        {
            if (thumbsUpIconImage != null)
            {
                thumbsUpIconImage.color = Color.white;
                if (thumbsUpUnpressedSprite != null) thumbsUpIconImage.sprite = thumbsUpUnpressedSprite;
            }
            if (thumbsDownIconImage != null)
            {
                thumbsDownIconImage.color = Color.white;
                if (thumbsDownUnpressedSprite != null) thumbsDownIconImage.sprite = thumbsDownUnpressedSprite;
            }
            return;
        }

        if (thumbsUpIconImage != null)
        {
            thumbsUpIconImage.color = Color.white;
            Sprite targetUpSprite = openArticleContext.playerLikedThisTemporaryState
                ? thumbsUpPressedSprite
                : thumbsUpUnpressedSprite;
            if (targetUpSprite != null) thumbsUpIconImage.sprite = targetUpSprite;
        }

        if (thumbsDownIconImage != null)
        {
            thumbsDownIconImage.color = Color.white;
            Sprite targetDownSprite = openArticleContext.playerDislikedThisTemporaryState
                ? thumbsDownPressedSprite
                : thumbsDownUnpressedSprite;
            if (targetDownSprite != null) thumbsDownIconImage.sprite = targetDownSprite;
        }
    }
    public void ProceedToNextDay()
    {
        if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(true);
        if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(true);
        if (currentDay >= 3)
        {
            //TriggerFinalGameOverState();
            EvaluateAndTriggerEnding();
            return;
        }
        Debug.Log("[PROGRESSION] Advancing calendar schedule and resetting runtime states...");
        if (postGameSummaryCanvasView != null) postGameSummaryCanvasView.SetActive(false);
        if (externalWebsitePopup != null) externalWebsitePopup.SetActive(false);
        if (appWindowPanelObject != null) appWindowPanelObject.SetActive(true);
        if (fullPostPopupPanel != null)
        {
            fullPostPopupPanel.gameObject.SetActive(false);
            Debug.Log("[PROGRESSION] FullPostModePopup set active.");
        }
        currentDay++;
        Debug.Log($"[PROGRESSION] Internal tracking advanced to Day: {currentDay}");
        InitializeDayLoadout(currentDay);
        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.LaunchTimerEngine();
        }
    }

    private void TriggerGameOverState()//bad ending
    {
        Debug.Log("Player has 0 reliability remaining");
        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
        }
        if (postGameSummaryCanvasView != null) postGameSummaryCanvasView.SetActive(false);
        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);
        if (appWindowPanelObject != null) appWindowPanelObject.SetActive(false);
        if (externalWebsitePopup != null) externalWebsitePopup.SetActive(false);
        if (captchaPanelObject != null) captchaPanelObject.SetActive(false); // Safety wipe
        if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(false);
        if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(false);
        if (GameOverCanvasView != null)
        {
            GameOverCanvasView.SetActive(true);
        }
        else
        {
            Debug.LogError("[END GAME] Final GameOver Canvas View slot is empty in the Inspector!");
        }
    }

    // Make this PUBLIC so BatteryTimeManager can also call it when the 7-minute timer ends!
    public void EvaluateAndTriggerEnding()
    {
        Debug.Log($"[END GAME] Evaluating ending with final score: {currentFriendScore}");

        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
        }

        // 1. Hide all gameplay popups and HUD panels
        HideAllGameplayAndEndingPanels();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEndingAudio(currentFriendScore);
        }

        // 1. Clean up and hide all gameplay popups and ending screens first
        if (postGameSummaryCanvasView != null) postGameSummaryCanvasView.SetActive(false);
        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);
        if (appWindowPanelObject != null) appWindowPanelObject.SetActive(false);
        if (externalWebsitePopup != null) externalWebsitePopup.SetActive(false);
        if (captchaPanelObject != null) captchaPanelObject.SetActive(false);
        if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(false);
        if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(false);


        if (GameOverCanvasView != null) GameOverCanvasView.SetActive(false);
        if (badEndingCanvasView != null) badEndingCanvasView.SetActive(false);
        if (neutralEndingCanvasView != null) neutralEndingCanvasView.SetActive(false);
        if (goodEndingCanvasView != null) goodEndingCanvasView.SetActive(false);

        // 2. Check reliability score thresholds and activate the matching ending canvas
        if (currentFriendScore <= 75) // Bad Ending: 0 to 75
        {
            Debug.Log("[END GAME] Triggering BAD ENDING (0 - 75)");
            //if (GameOverCanvasView != null) GameOverCanvasView.SetActive(true);
            if (badEndingCanvasView != null) badEndingCanvasView.SetActive(true);
            else Debug.LogError("[END GAME] GameOverCanvasView (Bad Ending) slot is empty in Inspector!");
        }
        else if (currentFriendScore <= 150) // Neutral Ending: 51 to 150
        {
            Debug.Log("[END GAME] Triggering NEUTRAL ENDING (51 - 150)");
            if (neutralEndingCanvasView != null) neutralEndingCanvasView.SetActive(true);
            else Debug.LogError("[END GAME] neutralEndingCanvasView slot is empty in Inspector!");
        }
        else // Good Ending: 151 to 227+
        {
            Debug.Log("[END GAME] Triggering GOOD ENDING (151 - 127)");
            if (goodEndingCanvasView != null) goodEndingCanvasView.SetActive(true);
            else Debug.LogError("[END GAME] goodEndingCanvasView slot is empty in Inspector!");
        }
    }

    // Helper method so we don't repeat UI cleanup code
    private void HideAllGameplayAndEndingPanels()
    {
        if (postGameSummaryCanvasView != null) postGameSummaryCanvasView.SetActive(false);
        if (day3SummaryCanvasView != null) day3SummaryCanvasView.SetActive(false);
        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);
        if (appWindowPanelObject != null) appWindowPanelObject.SetActive(false);
        if (externalWebsitePopup != null) externalWebsitePopup.SetActive(false);
        if (captchaPanelObject != null) captchaPanelObject.SetActive(false);
        if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(false);
        if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(false);

        if (GameOverCanvasView != null) GameOverCanvasView.SetActive(false);
        if (neutralEndingCanvasView != null) neutralEndingCanvasView.SetActive(false);
        if (goodEndingCanvasView != null) goodEndingCanvasView.SetActive(false);

        if (returnToDayOnePopup != null) returnToDayOnePopup.gameObject.SetActive(false);
        if (finalGameOverCanvasView != null) finalGameOverCanvasView.SetActive(false);

    }

    private Sprite GetReliabilityTierSprite(int score)
    {
        if (reliabilityBarSprites == null || reliabilityBarSprites.Length < 5) return null;

        //if (score <= 5) return reliabilityBarSprites[0];       // 1 bar: 0-37
        //if (score <= 15 && score >= 6) return reliabilityBarSprites[1];       // 2 bars: 38-75
        //if (score <= 25 && score >= 16) return reliabilityBarSprites[2];      // 3 bars: 76-150
        //if (score <= 36 && score >= 26) return reliabilityBarSprites[3];      // 4 bars: 151-188
        //return reliabilityBarSprites[4];                        // 5 bars: 189-227+

        if (score <= 37) return reliabilityBarSprites[0];       // 1 bar: 0-37
        if (score <= 75 && score >= 38) return reliabilityBarSprites[1];       // 2 bars: 38-75
        if (score <= 150 && score >= 76) return reliabilityBarSprites[2];      // 3 bars: 76-150
        if (score <= 188 && score >= 151) return reliabilityBarSprites[3];      // 4 bars: 151-188
        return reliabilityBarSprites[4];                        // 5 bars: 189-227+
    }

    // 4TH RESULT: Called when a day finishes (Days 1, 2, or 3) while reliability > 0
    public void TriggerEndOfDaySummary()
    {
        // If reliability hit 0 or below, skip the daily summary and trigger Bad Ending immediately
        if (currentFriendScore <= 0)
        {
            EvaluateAndTriggerEnding();
            return;
        }

        string formattedTime = "00:00:00";
        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.SetTimerPaused(true);
            BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
            formattedTime = BatteryTimeManager.Instance.GetFormattedRemainingTime();
        }

        Debug.Log($"[DAY COMPLETE] Showing End-of-Day Summary. Score: {currentFriendScore}");

        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
        }

        HideAllGameplayAndEndingPanels();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayDayEndSound();
        }

        Sprite tierSprite = GetReliabilityTierSprite(currentFriendScore);

        if (currentDay >= 3)
        {
            // Activate Day 3 Summary panel (no Topics Added section)
            if (day3SummaryCanvasView != null)
            {
                if (day3SummaryCanvasView.transform.parent != null)
                    day3SummaryCanvasView.transform.parent.gameObject.SetActive(true);

                day3SummaryCanvasView.SetActive(true);
                day3SummaryCanvasView.transform.SetAsLastSibling();
            }

            if (day3ResultsHeaderText != null)
                day3ResultsHeaderText.text = $"DAY {currentDay} RESULTS";

            if (day3ReliabilityMeterImage != null && tierSprite != null)
                day3ReliabilityMeterImage.sprite = tierSprite;

            if (day3FriendsTallyText != null)
                day3FriendsTallyText.text = $"FRIENDS : {currentFriendScore}";

            if (day3TimeLeftText != null)
                day3TimeLeftText.text = $"TIME LEFT : {formattedTime}";
        }
        else
        {
            // Activate Day 1 & Day 2 PostGameSummaryCanvasView
            if (postGameSummaryCanvasView != null)
            {
                if (postGameSummaryCanvasView.transform.parent != null)
                    postGameSummaryCanvasView.transform.parent.gameObject.SetActive(true);

                postGameSummaryCanvasView.SetActive(true);
                postGameSummaryCanvasView.transform.SetAsLastSibling();
            }

            if (dayResultsHeaderText != null)
                dayResultsHeaderText.text = $"DAY {currentDay} RESULTS";

            if (reliabilityMeterImage != null && tierSprite != null)
                reliabilityMeterImage.sprite = tierSprite;

            if (friendsTallyText != null)
                friendsTallyText.text = $"FRIENDS : {currentFriendScore}";

            if (topicsAddedHeaderObject != null)
                topicsAddedHeaderObject.SetActive(true);

            if (topicsAddedDetailText != null)
            {
                if (currentDay == 1)
                    topicsAddedDetailText.text = "ENTERTAINMENT AND POP CULTURE NEWS";
                else if (currentDay == 2)
                    topicsAddedDetailText.text = "POLITICAL NEWS";
            }

            if (timeLeftText != null)
                timeLeftText.text = $"TIME LEFT : {formattedTime}";
        }
        //if (postGameSummaryCanvasView != null)
        //{
        //    postGameSummaryCanvasView.SetActive(true);
        //    postGameSummaryCanvasView.transform.SetAsLastSibling();
        //}
        //else
        //{
        //    Debug.LogError("[DAY COMPLETE] postGameSummaryCanvasView is missing in Inspector!");
        //}
    }

    // WIRE THIS TO YOUR "NEXT DAY / CONTINUE" BUTTON ON postGameSummaryCanvasView
    public void OnNextDayButtonClicked()
    {
        // Check if the player just finished Day 3 (replace 'currentDay' with your day tracker variable)
        if (currentDay >= 3)
        {
            //if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(false);
            //if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(false);
            Debug.Log("[DAY 3 COMPLETE] Evaluating final ending based on reliability score...");
            EvaluateAndTriggerEnding();
        }
        else
        {
            Debug.Log($"[NEXT DAY] Advancing from Day {currentDay} to Day {currentDay + 1}...");
            if (postGameSummaryCanvasView != null) postGameSummaryCanvasView.SetActive(false);

            // Re-enable the HUD panels for the new day
            if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(true);
            if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(true);

            // Call your existing method that starts the next day & resets the battery timer!
            ProceedToNextDay();
        }
    }

    private void TriggerFinalGameOverState()
    {
        Debug.Log("[END GAME] All 3 days complete. Shutting down systems and showing Game Over screen.");
        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
        }
        if (postGameSummaryCanvasView != null) postGameSummaryCanvasView.SetActive(false);
        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);
        if (appWindowPanelObject != null) appWindowPanelObject.SetActive(false);
        if (externalWebsitePopup != null) externalWebsitePopup.SetActive(false);
        if (captchaPanelObject != null) captchaPanelObject.SetActive(false); // Safety wipe
        if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(false);
        if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(false);

        //if (finalGameOverCanvasView != null)
        //{
        //    finalGameOverCanvasView.SetActive(true);
        //}
        if (finalGameOverCanvasView != null)
        {
            GameOverCanvasView.SetActive(true);
        }
        else if (finalGameOverCanvasView != null)
        {
            
            neutralEndingCanvasView.SetActive(true);
            Debug.LogError("[END GAME] Final GameOver Canvas View slot is empty in the Inspector!");
        }
        else if (finalGameOverCanvasView != null)
        {
            goodEndingCanvasView.SetActive(true);
            Debug.LogError("[END GAME] Final GameOver Canvas View slot is empty in the Inspector!");
        }
        //if (finalGameOverCanvasView != null)
        //{
        //    finalGameOverCanvasView.SetActive(true);
        //}
        //else
        //{
        //    Debug.LogError("[END GAME] Final GameOver Canvas View slot is empty in the Inspector!");
        //}
    }
}

