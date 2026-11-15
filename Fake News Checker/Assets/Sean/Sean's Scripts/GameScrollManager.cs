////using UnityEngine;
////using UnityEngine.UI;
////using System.Collections.Generic;
////using TMPro;

////public class GameScrollManager : MonoBehaviour
////{
////    public static GameScrollManager Instance;

////    [Header("Current Run Progression")]
////    public int currentDay = 1; // Tracks Day 1 (10 posts), Day 2 (15 posts), Day 3 (20 posts)
////    private int currentFriendScore = 2; // Starts at 2

////    [Header("Grandma Profile Hooks")]
////    public Image grandmaAvatarDisplay;
////    public Sprite neutralFaceSprite;
////    public Sprite happyFaceSprite;
////    public Sprite sadFaceSprite;
////    private float reactionTimer = 0f;

////    [Header("UI Feed Layout Containers")]
////    public Transform doomScrollFeedContainer; // Vertical layout group for main dashboard list
////    public GameObject postListItemPrefab;    // List row button wrapper
////    public RectTransform fullPostPopupPanel;  // Main full screen app popup (90% Screen)

////    [Header("Full Post Display Nodes")]
////    public TMP_Text fullPostTitleText;
////    public TMP_Text fullPostContentBody;
////    public GameObject linkClickableButton;    // External link hook layout if applicable
////    public GameObject externalWebsitePopup;  // Simulated popup for website redirects

////    [Header("Direct Interaction Buttons")]
////    public Image shareButtonBg;  // Background box graphic that glows
////    public Image reportButtonBg; // Background box graphic that glows

////    [Header("Helper System")]
////    public TMP_Text helperUsesCounterText;
////    public Button helperActionButton;
////    private int helperUsesRemaining = 3;

////    [Header("Active Article Data State")]
////    private ArticlePageData openArticleContext;
////    private GameObject currentActiveFeedRowInstance;

////    [Header("Content Asset Libraries")]
////    public List<ArticlePageData> day1ArticlesPool;
////    public List<ArticlePageData> day2ArticlesPool;
////    public List<ArticlePageData> day3ArticlesPool;

////    [Header("External Website Component Targets")]
////    public TMP_Text websiteContentBodyText; // <-- Add this component slot field tracker here
////    public TMP_Text externalLinkButtonLabel; // <-- Add this component slot to trace link text changes

////    // ... inside your GameScrollManager class variables area ...
////    [Header("Comments List Component Hooks")]
////    public Transform commentsScrollContentContainer; // Target Viewport/Content box
////    public GameObject commentRowTextParagraphPrefab;  // Simple text row component blueprint asset


////    // Update this method within your script file
////    public void OpenFullPostMode(ArticlePageData data, GameObject sourceRowInstance)
////    {
////        openArticleContext = data;
////        currentActiveFeedRowInstance = sourceRowInstance;

////        fullPostTitleText.text = data.title;
////        fullPostContentBody.text = data.bodyText;

////        // Reset layout highlighter tints dynamically
////        shareButtonBg.color = Color.white;
////        reportButtonBg.color = Color.white;

////        // Point 4: Automatically apply custom website link labels if they exist
////        if (data.hasExternalLink)
////        {
////            linkClickableButton.SetActive(true);
////            if (externalLinkButtonLabel != null)
////            {
////                // Changes the text of your button directly to the web address parameter string
////                externalLinkButtonLabel.text = $"https://tellagram.com{data.articleID.ToLower()}";
////            }
////        }
////        else
////        {
////            linkClickableButton.SetActive(false);
////        }

////        fullPostPopupPanel.gameObject.SetActive(true);
////    }

////    // Point 3: Instantly populates the external webpage frame when targeted
////    public void ClickExternalLink()
////    {
////        if (externalWebsitePopup != null && openArticleContext != null)
////        {
////            if (websiteContentBodyText != null)
////            {
////                // Feeds your body paragraphs directly into your mock browser view window!
////                websiteContentBodyText.text = $"[BROWSING SECURE CONNECTION]\n\nSOURCE FEED URL: https://tellagram.com\n\n{openArticleContext.bodyText}";
////            }
////            externalWebsitePopup.SetActive(true);
////        }
////    }

////    private void Awake()
////    {
////        if (Instance == null) Instance = this;
////        else Destroy(gameObject);
////    }

////    private void Start()
////    {
////        InitializeDayLoadout(currentDay);
////        UpdateScoreboardDisplay();
////    }

////    private void Update()
////    {
////        if (reactionTimer > 0f)
////        {
////            reactionTimer -= Time.deltaTime;
////            if (reactionTimer <= 0f && grandmaAvatarDisplay != null)
////            {
////                grandmaAvatarDisplay.sprite = neutralFaceSprite;
////            }
////        }
////    }

////    public void InitializeDayLoadout(int dayNumber)
////    {
////        currentDay = dayNumber;
////        helperUsesRemaining = 3; // Reset helpers daily
////        UpdateHelperUI();

////        // Wipe active list cleanly
////        foreach (Transform child in doomScrollFeedContainer) Destroy(child.gameObject);

////        List<ArticlePageData> activePool = (dayNumber == 1) ? day1ArticlesPool :
////                                           (dayNumber == 2) ? day2ArticlesPool : day3ArticlesPool;

////        foreach (var article in activePool)
////        {
////            GameObject row = Instantiate(postListItemPrefab, doomScrollFeedContainer);
////            row.GetComponentInChildren<TMP_Text>().text = article.title;

////            // Map list selection actions directly
////            Button btn = row.GetComponent<Button>();
////            btn.onClick.AddListener(() => OpenFullPostMode(article, row));
////        }

////        LayoutRebuilder.ForceRebuildLayoutImmediate(doomScrollFeedContainer.GetComponent<RectTransform>());
////    }

////    //public void OpenFullPostMode(ArticlePageData data, GameObject sourceRowInstance)
////    //{
////    //    openArticleContext = data;
////    //    currentActiveFeedRowInstance = sourceRowInstance;

////    //    fullPostTitleText.text = data.title;
////    //    fullPostContentBody.text = data.bodyText;

////    //    // Reset highlight states dynamically
////    //    shareButtonBg.color = Color.white;
////    //    reportButtonBg.color = Color.white;

////    //    // Verify if banner link exists in specific article metadata
////    //    bool hasExternalLink = !string.IsNullOrEmpty(data.articleID) && (data.articleID.Contains("LINK") || data.articleID.Contains("PUPPY"));
////    //    linkClickableButton.SetActive(hasExternalLink);

////    //    fullPostPopupPanel.gameObject.SetActive(true);
////    //}

////    //public void ClickExternalLink()
////    //{
////    //    if (externalWebsitePopup != null)
////    //    {
////    //        externalWebsitePopup.SetActive(true);
////    //    }
////    //}

////    public void UseHelperLifeline()
////    {
////        if (helperUsesRemaining <= 0 || openArticleContext == null) return;

////        helperUsesRemaining--;
////        UpdateHelperUI();

////        // True means Legit (Share), False means Fake (Report)
////        bool isLegit = openArticleContext.cheatSheet.approveAndPublish;

////        if (isLegit) shareButtonBg.color = Color.green;
////        else reportButtonBg.color = Color.red;
////    }

////    public void ProcessPlayerVerdict(bool choseShare)
////    {
////        if (openArticleContext == null) return;

////        // Core validation logic: sheet.approveAndPublish true = legit/share, false = fake/report
////        bool isArticleLegit = openArticleContext.cheatSheet.approveAndPublish;
////        bool isCorrect = (choseShare == isArticleLegit);

////        if (isCorrect)
////        {
////            currentFriendScore += 5;
////            TriggerGrandmaExpression(happyFaceSprite);

////            // Gain extra helper reward for every 3 right answers milestone tracker
////            if (currentFriendScore % 15 == 0)
////            {
////                helperUsesRemaining++;
////                UpdateHelperUI();
////            }
////        }
////        else
////        {
////            currentFriendScore = Mathf.Max(0, currentFriendScore - 3);
////            TriggerGrandmaExpression(sadFaceSprite);
////        }

////        UpdateScoreboardDisplay();

////        // Auto-close post window, purge completed entry, and slide the remaining layout up
////        Invoke(nameof(CloseAndAnimateFeedShift), 1.5f);
////    }

////    private void CloseAndAnimateFeedShift()
////    {
////        fullPostPopupPanel.gameObject.SetActive(false);

////        if (currentActiveFeedRowInstance != null)
////        {
////            Destroy(currentActiveFeedRowInstance);
////        }

////        openArticleContext = null;
////        currentActiveFeedRowInstance = null;

////        // Force UI rendering stack recalculation instantly
////        Canvas.ForceUpdateCanvases();
////        LayoutRebuilder.ForceRebuildLayoutImmediate(doomScrollFeedContainer.GetComponent<RectTransform>());

////        // Win Condition check for empty feed before timeout
////        if (doomScrollFeedContainer.childCount == 0)
////        {
////            BatteryTimeManager.Instance.EvaluateRunCompletion();
////        }
////    }

////    private void TriggerGrandmaExpression(Sprite moodSprite)
////    {
////        if (grandmaAvatarDisplay == null) return;
////        grandmaAvatarDisplay.sprite = moodSprite;
////        reactionTimer = 1.5f;
////    }

////    private void UpdateScoreboardDisplay()
////    {
////        // Broadcast updates to the scoreboard UI
////        BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
////    }

////    private void UpdateHelperUI()
////    {
////        if (helperUsesCounterText != null)
////            helperUsesCounterText.text = $"x{helperUsesRemaining}";
////    }
////}


//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections.Generic;
//using TMPro;

//public class GameScrollManager : MonoBehaviour
//{
//    public static GameScrollManager Instance;

//    [Header("Current Run Progression")]
//    public int currentDay = 1; // Tracks Day 1 (10 posts), Day 2 (15 posts), Day 3 (20 posts)
//    private int currentFriendScore = 2; // Starts at 2

//    [Header("Grandma Profile Hooks")]
//    public Image grandmaAvatarDisplay;
//    public Sprite neutralFaceSprite;
//    public Sprite happyFaceSprite;
//    public Sprite sadFaceSprite;
//    private float reactionTimer = 0f;

//    [Header("UI Feed Layout Containers")]
//    public Transform doomScrollFeedContainer; // Viewport/Content inside FeedScrollView
//    public GameObject postListItemPrefab;    // List row button template asset
//    public RectTransform fullPostPopupPanel;  // Main full screen app popup (90% Screen)

//    [Header("Full Post Display Nodes")]
//    public TMP_Text fullPostTitleText;
//    public TMP_Text fullPostContentBody;
//    public GameObject linkClickableButton;    // External link button wrapper object

//    //[Header("Full Post Display Nodes")]
//    // --- ADD THIS NEW REFERENCE NODE SLOT ---
//    public TMP_Text fullPostHeadlinerText;


//    [Header("External Website Component Targets")]
//    public GameObject externalWebsitePopup;
//    public TMP_Text websiteHeadlinerText; // <-- ADD THIS NEW HOOK HERE! ; this is for adding the headliner text that is for bold as it is place above picture of the article
//    public TMP_Text websiteContentBodyText;   // Populates copy text to mock browser
//    public TMP_Text externalLinkButtonLabel;  // Overrides button text to show the custom URL

//    [Header("Comments List Component Hooks")]
//    public Transform commentsScrollContentContainer; // Viewport/Content inside CommentsSectionScrollView
//    public GameObject commentRowTextParagraphPrefab;  // Simple Text (TMP) row prefab asset

//    [Header("Direct Verdict Buttons")]
//    public Image shareButtonBg;  // Background box graphic that glows
//    public Image reportButtonBg; // Background box graphic that glows

//    [Header("Helper System")]
//    public TMP_Text helperUsesCounterText;
//    public Button helperActionButton;
//    private int helperUsesRemaining = 3;

//    [Header("Active Article Data State")]
//    private ArticlePageData openArticleContext;
//    private GameObject currentActiveFeedRowInstance;

//    [Header("Content Asset Libraries")]
//    public List<ArticlePageData> day1ArticlesPool;
//    public List<ArticlePageData> day2ArticlesPool;
//    public List<ArticlePageData> day3ArticlesPool;

//    private bool helperUsedOnCurrentArticle = false;

//    [Header("Image Asset Displays")]
//    public Image fullPostArticleImageComponent;
//    public Image externalWebsiteImageComponent;

//    [Header("Desktop System Startup Links")]
//    public GameObject appWindowPanelObject; // Drag your 'AppWindowPanel' here

//    [Header("HUD Overlay Toggles")]
//    public GameObject grandmaProfilePanelObject; // Drag GrandmaProfilePanel here
//    public GameObject helperLifelineWidgetObject; // Drag HelperLifelineWidget here

//    private bool canProcessVerdict = true;
//    // Prevents button spamming as it helps prevent a bug that resets the players progress by closing AppWindowPanel

//    [Header("Helper System Expression Toggles")]
//    public Image helperIconDisplay;       // Drag the Image component of your Helper UI widget here
//    public Sprite helperNormalSprite;     // Your helper's default, passive expression artwork
//    public Sprite helperActionSprite;     // The special expression artwork displayed for 1.5 seconds


//    public void LaunchApplicationFromDesktop()
//    {
//        if (appWindowPanelObject != null)
//        {
//            // 1. Reveal your main 90% application window workspace
//            appWindowPanelObject.SetActive(true);

//            // 2. Dynamically reveal HUD widgets only when the app boots up
//            if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(true);
//            if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(true);

//            // 3. Start your daily articles pool loading system
//            InitializeDayLoadout(1);

//            // 4. Kick off your 7-bar segmented countdown timer clock loop
//            if (BatteryTimeManager.Instance != null)
//            {
//                BatteryTimeManager.Instance.LaunchTimerEngine();
//            }
//        }
//    }
//        //public void LaunchApplicationFromDesktop()
//        //{
//        //    if (appWindowPanelObject != null)
//        //    {
//        //        // 1. Reveal your main 90% application window
//        //        appWindowPanelObject.SetActive(true);

//        //        // 2. Start your daily articles pool loading system
//        //        InitializeDayLoadout(1);

//        //        // 3. Kick off your 7-bar segmented countdown timer clock loop
//        //        if (BatteryTimeManager.Instance != null)
//        //        {
//        //            BatteryTimeManager.Instance.LaunchTimerEngine();
//        //            Debug.Log("[DESKTOP] Application started successfully. Battery timer is ticking!");
//        //        }
//        //    }
//        //}

//        private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    private void Start()
//    {
//        // Clean out memory leaks across all three asset pool tables on level startup
//        ResetAllArticleMemoryStates(day1ArticlesPool);
//        ResetAllArticleMemoryStates(day2ArticlesPool);
//        ResetAllArticleMemoryStates(day3ArticlesPool);

//        InitializeDayLoadout(currentDay);
//        UpdateScoreboardDisplay();

//        InitializeDayLoadout(currentDay);
//        UpdateScoreboardDisplay();
//    }

//    private void ResetAllArticleMemoryStates(List<ArticlePageData> articlePoolToClean)
//    {
//        if (articlePoolToClean == null) return;
//        foreach (var article in articlePoolToClean)
//        {
//            if (article != null)
//            {
//                article.playerLikedThisTemporaryState = false;
//                article.playerDislikedThisTemporaryState = false;

//                // Add this clean reset line here!
//                article.helperWasUsedOnThisArticleState = false;
//            }
//        }
//    }

//    private void Update()
//    {
//        if (reactionTimer > 0f)
//        {
//            reactionTimer -= Time.deltaTime;
//            if (reactionTimer <= 0f && grandmaAvatarDisplay != null)
//            {
//                grandmaAvatarDisplay.sprite = neutralFaceSprite;
//            }
//        }
//    }

//    public void InitializeDayLoadout(int dayNumber)
//    {
//        currentDay = dayNumber;
//        helperUsesRemaining = 3;
//        UpdateHelperUI();

//        foreach (Transform child in doomScrollFeedContainer) Destroy(child.gameObject);

//        List<ArticlePageData> activePool = (dayNumber == 1) ? day1ArticlesPool :
//                                           (dayNumber == 2) ? day2ArticlesPool : day3ArticlesPool;

//        foreach (var article in activePool)
//        {
//            GameObject row = Instantiate(postListItemPrefab, doomScrollFeedContainer);
//            row.GetComponentInChildren<TMP_Text>().text = article.title;

//            Button btn = row.GetComponent<Button>();
//            btn.onClick.AddListener(() => OpenFullPostMode(article, row));
//        }

//        LayoutRebuilder.ForceRebuildLayoutImmediate(doomScrollFeedContainer.GetComponent<RectTransform>());
//    }

//    // --- STEP 3: THE BACK BUTTON LOOPHOLE INTERCEPTOR ---
//    // 3. BACK BUTTON FROM FULL POST MODE
//    // Locate or create your BackButton trigger method
//    public void CloseFullPostAndReturnToDesktop()
//    {
//        // --- CRUCIAL BUG FIX ---
//        // If canProcessVerdict is FALSE, it means a verdict is actively processing 
//        // inside that 1.5-second animation delay window right now. We block the back button!
//        if (!canProcessVerdict)
//        {
//            Debug.LogWarning("[CHEAT SHIELD] Back Button blocked! You cannot escape a verdict animation loop.");
//            return;
//        }

//        // --- LAYER TOGGLES FOR RULE #3 ---
//        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false); // Hide the post view
//        if (appWindowPanelObject != null) appWindowPanelObject.SetActive(true); // Reveal the main case file app view again
//                                                                                // ---------------------------------

//        openArticleContext = null;
//        currentActiveFeedRowInstance = null;
//    }

//    // 4. CLOSING THE MOCK EXTERNAL WEBSITE BROWSER
//    // Locate or create your CloseWindowButton trigger method
//    public void CloseExternalWebsiteAndReturnToPost()
//    {
//        // --- LAYER TOGGLES FOR RULE #4 ---
//        if (externalWebsitePopup != null) externalWebsitePopup.SetActive(false); // Hide browser frame
//        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(true); // Bring full article layout back into view
//        // ---------------------------------
//    }

//    public void OpenFullPostMode(ArticlePageData data, GameObject sourceRowInstance)
//    {
//        canProcessVerdict = true; // Unlock the safety gates for this new article file!
//        //based from private bool canProcessVerdict = true; scroll up and you'd see it

//        openArticleContext = data;
//        currentActiveFeedRowInstance = sourceRowInstance;
//        helperUsedOnCurrentArticle = false;

//        // --- ADD THIS POPULATION LINE ---
//        if (fullPostHeadlinerText != null)
//        {
//            fullPostHeadlinerText.text = data.headliner;
//        }
//        // ---------------------------------

//        // --- PASTE THESE TWO EXTRA SAFETIES DIRECTLY IN THIS METHOD LOOP ---
//        // Safely forces the current state markers back to clean default settings every time you choose a new row header
//        if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
//        if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
//        // -------------------------------------------------------------------

//        fullPostTitleText.text = data.title;
//        fullPostContentBody.text = data.bodyText;

//        //appWindowPanelObject.gameObject.SetActive(false); //turns appwindow off

//        // Continue running your existing code downwards from here...
//        fullPostPopupPanel.gameObject.SetActive(true);
//        LoadCommentsToView(data);

//        //commented out since they made share and report button background white I forgot to remove this earlier
//        shareButtonBg.color = Color.purple;
//        reportButtonBg.color = Color.purple;

//        if (data.hasExternalLink)
//        {
//            linkClickableButton.SetActive(true);
//            externalLinkButtonLabel.text = $"https://www.{data.articleID.ToLower().Trim()}.org";
//        }
//        else
//        {
//            linkClickableButton.SetActive(false);
//        }

//        if (appWindowPanelObject != null) appWindowPanelObject.SetActive(false);
//        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(true);

//        // --- NEW RE-RENDER LOGIC FOR PERSISTENT HIGHLIGHTS ---
//        // Clear buttons to default white color first
//        //if (shareButtonBg != null) shareButtonBg.color = Color.white;
//        //if (reportButtonBg != null) reportButtonBg.color = Color.white;

//        // Check if this specific article already had a helper charge spent on it previously
//        if (data != null && data.helperWasUsedOnThisArticleState)
//        {
//            bool isLegit = data.cheatSheet.approveAndPublish;
//            if (isLegit && shareButtonBg != null) shareButtonBg.color = Color.green;
//            else if (!isLegit && reportButtonBg != null) reportButtonBg.color = Color.red;

//            Debug.Log("[HELPER DISPLAY] Restored saved highlight state for this specific article view.");
//        }
//        // -----------------------------------------------------


//        // Automatically updates the image thumbnail if data exists inside your asset arrays
//        if (data.images != null && data.images.Length > 0 && data.images[0].imageSprite != null)
//        {
//            fullPostArticleImageComponent.gameObject.SetActive(true);
//            fullPostArticleImageComponent.sprite = data.images[0].imageSprite;
//        }
//        else
//        {
//            fullPostArticleImageComponent.gameObject.SetActive(false); // Hide if no image assigned
//        }


//        //// ... your existing code setting text, titles, etc ...

//        //fullPostPopupPanel.gameObject.SetActive(true);
//        //LoadCommentsToView(data);

//        // Add this line inside your OpenFullPostMode method
//        helperUsedOnCurrentArticle = false;

//        // Locate this exact section inside your existing OpenFullPostMode method
//        if (data.hasExternalLink)
//        {
//            // 1. Force the parent button gameobject active instantly on screen
//            linkClickableButton.SetActive(true);
//            if (externalLinkButtonLabel != null)
//            {
//                //externalLinkButtonLabel.text = $"https://tellagram.com{data.articleID.ToLower()}";

//                // 2. Derive the unique domain path from your current open article metadata
//                string customDomain = $"https://www.{data.articleID.ToLower().Trim()}.org/feed-post";
//                //externalLinkButtonLabel.text = customDomain;

//                externalLinkButtonLabel.text = $"https://www.{data.articleID.ToLower().Trim()}.org";

//            }
//        }
//        else
//        {
//            linkClickableButton.SetActive(false);
//        }

//        //fullPostPopupPanel.gameObject.SetActive(true);

//        // --- LAYER TOGGLES FOR RULE #1 ---
//        if (appWindowPanelObject != null) appWindowPanelObject.SetActive(false); // Hide main desktop app view
//        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(true); // Open the full post overlay
//        // ---------------------------------


//        // Populate comments when full post opens
//        LoadCommentsToView(data);

//        // --- PASTE THIS EXACT LINE AT THE ABSOLUTE BOTTOM OF THIS METHOD ---
//        RefreshThumbsVisualsFromArticleData();
//    }

//    //public void ClickExternalLink()
//    //{
//    //    if (externalWebsitePopup != null && openArticleContext != null)
//    //    {
//    //        if (websiteContentBodyText != null)
//    //        {
//    //            websiteContentBodyText.text = $"[SECURE WEBSITE FEED]\n\nURL: https://tellagram.com\n\n{openArticleContext.bodyText}";
//    //        }
//    //        externalWebsitePopup.SetActive(true);
//    //    }
//    //}

//    //public void ClickExternalLink()
//    //{
//    //    if (externalWebsitePopup != null && openArticleContext != null)
//    //    {
//    //        if (websiteContentBodyText != null)
//    //        {
//    //            // Clean out raw systemic brackets/hyperlink strings from displaying inside the viewer
//    //            string cleanBody = openArticleContext.bodyText.Replace("[LINK]", "").Replace("[PUPPY]", "");

//    //            websiteContentBodyText.text = $"[SECURE BROWSER FEED]\n\n{cleanBody}";
//    //        }
//    //        externalWebsitePopup.SetActive(true);
//    //    }
//    //}

//    public void ClickExternalLink()
//    {
//        if (externalWebsitePopup != null && openArticleContext != null)
//        {
//            if (websiteContentBodyText != null)
//            {
//                // 1. Pass the unique Headliner text to the new browser headline component
//                if (websiteHeadlinerText != null)
//                {
//                    // Using <b> tags ensures it displays with strong bold emphasis inside the browser!
//                    websiteHeadlinerText.text = $"<b>{openArticleContext.headliner}</b>";
//                }
//                else
//                {
//                    // Fallback: If you don't use a separate headliner box, 
//                    // it prints a helpful log instead of crashing your game layer
//                    Debug.LogWarning("[BROWSER WARNING] WebsiteHeadlinerText slot is currently empty. Skipping text assignment.");
//                }
//                // 2. Clear out formatting brackets and pass the core story text
//                if (websiteContentBodyText != null)
//                {
//                    string cleanBody = openArticleContext.bodyText.Replace("[LINK]", "").Replace("[PUPPY]", "");
//                    websiteContentBodyText.text = cleanBody;

//                    // --- MANUAL CODE PIXEL OFFSET ---
//                    // This manually creates an intentional 150-pixel margin gap at the top of the body paragraph box.
//                    // This perfectly carves out a clean empty window space for your child image display to sit 
//                    // right below the headliner without overlapping any sentences!
//                    websiteContentBodyText.margin = new Vector4(0f, 90f, 0f, 0f);
//                }

//                // Simply strip away systemic brackets text clean
//                //string cleanBody = openArticleContext.bodyText.Replace("[LINK]", "").Replace("[PUPPY]", "");

//                // Outputs ONLY the raw copy text of the article context directly into the window frame
//                //websiteContentBodyText.text = cleanBody;

//                //// Wipe out raw code strings from displaying inside the reader window frame
//                ////string cleanBody = openArticleContext.bodyText.Replace("[LINK]", "").Replace("[PUPPY]", "");

//                //// Dynamic URL constructor string using your Article Asset IDs directly!
//                ////string customGeneratedURL = $"www.{openArticleContext.articleID.ToLower().Trim()}.com/news/feed-post-01";

//                ////// Compiles your link address string dynamically as the absolute first line of text visible inside your browser view
//                ////websiteContentBodyText.text = $"🌐 BROWSER LINK: http://{customGeneratedURL}\n" +
//                ////                              $"🔒 CONNECTION SECURE & ENCRYPTED\n" +
//                ////                              $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
//                ////                              $"{cleanBody}";
//            }

//            // 3. Populate your working nested artwork sprite
//            if (openArticleContext.images != null && openArticleContext.images.Length > 0 && openArticleContext.images[0].imageSprite != null)
//            {
//                fullPostPopupPanel.gameObject.SetActive(false);

//                if (externalWebsiteImageComponent != null)
//                {
//                    externalWebsiteImageComponent.gameObject.SetActive(true);
//                    externalWebsiteImageComponent.sprite = openArticleContext.images[0].imageSprite;

//                    // --- AUTOMATIC ANCHOR RE-ALIGNMENT ---
//                    // Instead of manual Y values, we lock the image to the top-center of the body text box.
//                    // As the body text moves dynamically based on the headliner length, the image moves with it!
//                    RectTransform imageRect = externalWebsiteImageComponent.GetComponent<RectTransform>();
//                    if (imageRect != null)
//                    {
//                        imageRect.anchorMin = new Vector2(0.5f, 1f); // Center-Top Anchor point
//                        imageRect.anchorMax = new Vector2(0.5f, 1f);
//                        imageRect.pivot = new Vector2(0.5f, 1f);     // Pivot point at the top edge of the sprite

//                        // Places it right inside the 90px margin window we carved out above
//                        imageRect.anchoredPosition = new Vector2(0f, -10f);
//                    }
//                    //// Manually force your image coordinate to anchor safely inside the text's margin window gap
//                    //RectTransform imageRect = externalWebsiteImageComponent.GetComponent<RectTransform>();
//                    //if (imageRect != null)
//                    //{
//                    //    //imageRect.anchoredPosition = new Vector2(0f, -40f); // Shifts it right into the carved out open space
//                    //}
//                }
//            }
//            else
//            {
//                if (externalWebsiteImageComponent != null) externalWebsiteImageComponent.gameObject.SetActive(false);
//            }
//            //// 3. Keep your image component setup exactly as it was
//            //if (openArticleContext.images != null && openArticleContext.images.Length > 0 && openArticleContext.images[0].imageSprite != null)
//            //{
//            //    fullPostPopupPanel.gameObject.SetActive(false); //turn it off

//            //    externalWebsiteImageComponent.gameObject.SetActive(true);
//            //    externalWebsiteImageComponent.sprite = openArticleContext.images[0].imageSprite;
//            //}
//            //else
//            //{
//            //    externalWebsiteImageComponent.gameObject.SetActive(false);
//            //}
//            //externalWebsitePopup.SetActive(true);

//            // --- LAYER TOGGLES FOR RULE #2 ---
//            //if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false); // Hide full post layout frame
//            externalWebsitePopup.SetActive(true); // Bring up the external browser popup
//            // ---------------------------------
//        }
//    }


//    private void LoadCommentsToView(ArticlePageData data)
//    {
//        if (commentsScrollContentContainer == null || commentRowTextParagraphPrefab == null) return;

//        foreach (Transform child in commentsScrollContentContainer) Destroy(child.gameObject);

//        if (data.customComments != null && data.customComments.Count > 0)
//        {
//            foreach (string commentString in data.customComments)
//            {
//                GameObject commentRow = Instantiate(commentRowTextParagraphPrefab, commentsScrollContentContainer);

//                // 1. Target the text component exactly where it is right now
//                TMP_Text targetText = commentRow.GetComponentInChildren<TMP_Text>();
//                if (targetText != null)
//                {
//                    targetText.text = commentString;

//                    // 2. FORCE CODE-DRIVEN MARGIN OFFSET
//                    // This leaves your layout groups alone and manually pushes the text characters 
//                    // 60 pixels to the right, safely clearing room for your profile icon!
//                    targetText.margin = new Vector4(95f, 0f, 0f, 0f);
//                }
//            }
//        }
//        else
//        {
//            GameObject emptyNoticeRow = Instantiate(commentRowTextParagraphPrefab, commentsScrollContentContainer);
//            TMP_Text targetText = emptyNoticeRow.GetComponentInChildren<TMP_Text>();
//            if (targetText != null)
//            {
//                targetText.text = "<i>No comments on this post.</i>";
//                targetText.margin = new Vector4(95f, 0f, 0f, 0f);
//            }
//        }

//        //if (data.customComments != null && data.customComments.Count > 0)
//        //{
//        //    foreach (string commentString in data.customComments)
//        //    {
//        //        GameObject commentRow = Instantiate(commentRowTextParagraphPrefab, commentsScrollContentContainer);
//        //        commentRow.GetComponentInChildren<TMP_Text>().text = commentString;
//        //    }
//        //}
//        //else
//        //{
//        //    GameObject emptyNoticeRow = Instantiate(commentRowTextParagraphPrefab, commentsScrollContentContainer);
//        //    emptyNoticeRow.GetComponentInChildren<TMP_Text>().text = "<i>No comments on this post.</i>";
//        //}

//        Canvas.ForceUpdateCanvases();
//        LayoutRebuilder.ForceRebuildLayoutImmediate(commentsScrollContentContainer.GetComponent<RectTransform>());
//    }

//    public void UseHelperLifeline()
//    {
//        //if (helperUsesRemaining <= 0 || openArticleContext == null) return;

//        //helperUsesRemaining--;
//        //UpdateHelperUI();

//        //bool isLegit = openArticleContext.cheatSheet.approveAndPublish;

//        //if (isLegit) shareButtonBg.color = Color.green;
//        //else reportButtonBg.color = Color.red;

//        // Safety Gate: block usage if helper was already used on this article once
//        //if (helperUsesRemaining <= 0 || openArticleContext == null || helperUsedOnCurrentArticle) return;
//        //note: last one

//        //helperUsedOnCurrentArticle = true; // Lock it out immediately
//        //note: last one

//        //if(fullPostPopupPanel == true)
//        //{
//        //    Debug.Log("helper used");

//        //    helperUsesRemaining--;
//        //    UpdateHelperUI();

//        //    bool isLegit = openArticleContext.cheatSheet.approveAndPublish;
//        //    if (isLegit) shareButtonBg.color = Color.green;
//        //    else reportButtonBg.color = Color.red;
//        //}

//        //helperUsesRemaining--;
//        //UpdateHelperUI();

//        //bool isLegit = openArticleContext.cheatSheet.approveAndPublish;
//        //if (isLegit) shareButtonBg.color = Color.green;
//        //else reportButtonBg.color = Color.red;

//        //// --- SAFETY GATE #1: Check if the full post panel is active on screen ---
//        //if (fullPostPopupPanel == null || !fullPostPopupPanel.gameObject.activeSelf)
//        //{
//        //    Debug.Log("[HELPER INTERCEPT] Click blocked because FullPostPopupPanel is currently closed or hidden.");
//        //    return;
//        //}

//        //// --- DIAGNOSTIC LOGS ---
//        //Debug.Log($"[HELPER CHECK] Checking stats... Uses remaining: {helperUsesRemaining} | Context assigned: {openArticleContext != null} | Already used on this article: {helperUsedOnCurrentArticle}");

//        //// --- SAFETY GATE #2: Split up checks to see which one is failing ---
//        //if (openArticleContext == null)
//        //{
//        //    Debug.LogWarning("[HELPER INTERCEPT] Click blocked: openArticleContext is NULL.");
//        //    return;
//        //}

//        //if (helperUsesRemaining <= 0)
//        //{
//        //    Debug.LogWarning("[HELPER INTERCEPT] Click blocked: No helper charges remaining!");
//        //    return;
//        //}

//        //if (helperUsedOnCurrentArticle)
//        //{
//        //    Debug.LogWarning("[HELPER INTERCEPT] Click blocked: Helper was ALREADY used on this specific article file!");
//        //    return;
//        //}

//        //// 3. If it passes all gates, consume a charge and activate the helper hint
//        //Debug.Log("helper used");

//        //helperUsedOnCurrentArticle = true; // Locks out duplicate clicks on this article
//        //helperUsesRemaining--;
//        //UpdateHelperUI();

//        if (fullPostPopupPanel == null || !fullPostPopupPanel.gameObject.activeSelf)
//        {
//            Debug.Log("[HELPER INTERCEPT] Click blocked because FullPostPopupPanel is currently closed or hidden.");
//            return;
//        }

//        if (openArticleContext == null) return;

//        // --- UPDATED CONDITIONS USING PER-ARTICLE STATE ---
//        // Blocks click if this specific article already has the helper activated on it
//        if (openArticleContext.helperWasUsedOnThisArticleState)
//        {
//            Debug.LogWarning("[HELPER INTERCEPT] Click blocked: Helper was ALREADY used on this specific article data profile!");
//            return;
//        }

//        if (helperUsesRemaining <= 0)
//        {
//            Debug.LogWarning("[HELPER INTERCEPT] Click blocked: No helper charges remaining!");
//            return;
//        }

//        // 3. Mark the state onto the asset data profile directly
//        Debug.Log("helper used");

//        openArticleContext.helperWasUsedOnThisArticleState = true; // Save it to the article!
//        helperUsesRemaining--;
//        UpdateHelperUI();

//        // Trigger the automatic 1.5-second image swap sequence
//        if (helperIconDisplay != null && helperActionSprite != null)
//        {
//            StartCoroutine(HelperExpressionRoutine());
//        }

//        bool isLegit = openArticleContext.cheatSheet.approveAndPublish;
//        if (isLegit && shareButtonBg != null) shareButtonBg.color = Color.green;
//        else if (!isLegit && reportButtonBg != null) reportButtonBg.color = Color.red;


//        //bool isLegit = openArticleContext.cheatSheet.approveAndPublish;
//        //if (isLegit) shareButtonBg.color = Color.green;
//        //else reportButtonBg.color = Color.red;

//    }

//    /// <summary>
//    /// Swaps the helper's avatar look to its active action graphic, 
//    /// waits for exactly 1.5 seconds, and cleanly snaps it back to normal.
//    /// </summary>
//    private System.Collections.IEnumerator HelperExpressionRoutine()
//    {
//        helperIconDisplay.sprite = helperActionSprite;

//        // Wait safely in real-time execution for exactly 1.5 seconds
//        yield return new WaitForSeconds(1.5f);

//        if (helperIconDisplay != null && helperNormalSprite != null)
//        {
//            helperIconDisplay.sprite = helperNormalSprite;
//        }
//    }

//    // --- STEP 2: PROCESS PLAYER VERDICT (THE TRIGGER POINT) ---
//    public void ProcessPlayerVerdict(bool choseShare)
//    {
//        // ANTI-SPAM SAFETY GATE: Instantly drop the call if a verdict is already running
//        // --- ANTI-SPAM SAFETY GATE ---
//        if (!canProcessVerdict || openArticleContext == null) return;
//        canProcessVerdict = false; // Instantly lock out further clicks until next frame/post
//                                   // ------------------------------

//        //if (openArticleContext == null) return; //commented this out to prevent a issue I forgot what

//        appWindowPanelObject.SetActive(true);

//        bool isArticleLegit = openArticleContext.cheatSheet.approveAndPublish;
//        bool isCorrect = (choseShare == isArticleLegit);

//        if (isCorrect)
//        {
//            currentFriendScore += 5;
//            TriggerGrandmaExpression(happyFaceSprite);

//            if (currentFriendScore % 15 == 0)
//            {
//                helperUsesRemaining++;
//                UpdateHelperUI();
//            }
//        }
//        else
//        {
//            currentFriendScore = Mathf.Max(0, currentFriendScore - 3);
//            TriggerGrandmaExpression(sadFaceSprite);
//        }

//        UpdateScoreboardDisplay();

//        // --- INSTANT GAME OVER CHECK ---
//        // If reliability drops to 0 or below, trigger an immediate failure state
//        if (currentFriendScore <= 0)
//        {
//            Debug.LogWarning("[GAME OVER] Player reliability hit 0 or lower! Triggering terminal game over canvas view.");

//            // 1. First, queue the normal interface cleanup script to run in 1.5 seconds
//            Invoke(nameof(CloseAndAnimateFeedShift), 1.5f);

//            // 2. Simultaneously queue the final Game Over screen to drop down right after it (at 1.55s)
//            // This leaves the entire preview layout open on screen for Grandma's full reaction timer!
//            Invoke(nameof(TriggerFinalGameOverState), 1.50f);
//            return;

//            //// 1. Run the normal cleanup sequence right now so the screen closes neatly
//            //CloseAndAnimateFeedShift();

//            //// 2. Instead of triggering Game Over instantly, we queue it up with a 1.5 second delay!
//            //Invoke(nameof(TriggerFinalGameOverState), 1.5f);
//            //return;

//            // Cancel any pending animations to prevent background code execution leaks
//            //CancelInvoke(nameof(CloseAndAnimateFeedShift));

//            // Execute your pre-built game over panels and HUD wipe sequence
//            //TriggerFinalGameOverState();
//            //return; // Exit out of the method immediately so it doesn't queue up normal shifts
//        }

//        // If score is safe (> 0), continue the regular 1.5-second daily progression loop

//        Invoke(nameof(CloseAndAnimateFeedShift), 1.5f);
//    }

//    // --- STEP 4: CLEAN DISPATCH ON SHIFT ---
//    private void CloseAndAnimateFeedShift()
//    {
//        // Cancel any pending invokes just in case to eliminate Bug #2 forever
//        CancelInvoke(nameof(CloseAndAnimateFeedShift));

//        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);

//        //fullPostPopupPanel.gameObject.SetActive(false);

//        if (currentActiveFeedRowInstance != null)
//        {
//            Destroy(currentActiveFeedRowInstance);
//        }

//        openArticleContext = null;
//        currentActiveFeedRowInstance = null;

//        Canvas.ForceUpdateCanvases();
//        LayoutRebuilder.ForceRebuildLayoutImmediate(doomScrollFeedContainer.GetComponent<RectTransform>());

//        //if (doomScrollFeedContainer.childCount == 0)
//        //{
//        //    BatteryTimeManager.Instance.EvaluateRunCompletion();
//        //}

//        // --- UPDATED SAFETY CHECK ---
//        // Since the destroyed child doesn't detach until the frame ends, 
//        // checking for <= 1 ensures we catch the true "last item" condition.
//        if (doomScrollFeedContainer.childCount <= 1)
//        {
//            Debug.Log("[FEED MANAGER] All daily cases cleared! Transferring execution to Summary Engine...");
//            if (BatteryTimeManager.Instance != null)
//            {
//                BatteryTimeManager.Instance.EvaluateRunCompletion();
//            }
//        }

//        //// Inside CloseAndAnimateFeedShift() under GameScrollManager.cs
//        //if (doomScrollFeedContainer.childCount == 0)
//        //{
//        //    if (BatteryTimeManager.Instance != null)
//        //    {
//        //        BatteryTimeManager.Instance.EvaluateRunCompletion();
//        //    }
//        //}
//    }

//    private void TriggerGrandmaExpression(Sprite moodSprite)
//    {
//        if (grandmaAvatarDisplay == null) return;
//        grandmaAvatarDisplay.sprite = moodSprite;
//        reactionTimer = 1.5f;
//    }   

//    private void UpdateScoreboardDisplay()
//    {
//        BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
//    }

//    private void UpdateHelperUI()
//    {
//        if (helperUsesCounterText != null)
//            helperUsesCounterText.text = $"Counter: {helperUsesRemaining}";
//    }


//    //thumbs up and down UI interactable aka likes and dislikes

//    [Header("Thumbs Interaction Custom Elements")]
//    public Image thumbsUpIconImage;   // Drag the Image component of your Thumbs Up button here
//    public Image thumbsDownIconImage; // Drag the Image component of your Thumbs Down button here

//    private bool isLiked = false;
//    private bool isDisliked = false;

//    //// Call this via the OnClick() event on your Thumbs Up UI Button component
//    //public void ToggleThumbsUpInteraction()
//    //{
//    //    isLiked = !isLiked;
//    //    if (isLiked)
//    //    {
//    //        thumbsUpIconImage.color = new Color(0.2f, 0.6f, 1f, 1f); // Vibrant Blue Highlight
//    //        isDisliked = false; // Turn off dislike state automatically
//    //        if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
//    //    }
//    //    else
//    //    {
//    //        thumbsUpIconImage.color = Color.white; // Clear back to standard tint
//    //    }
//    //}

//    //// Call this via the OnClick() event on your Thumbs Down UI Button component
//    //public void ToggleThumbsDownInteraction()
//    //{
//    //    isDisliked = !isDisliked;
//    //    if (isDisliked)
//    //    {
//    //        thumbsDownIconImage.color = new Color(1f, 0.2f, 0.2f, 1f); // Vibrant Red Highlight
//    //        isLiked = false; // Turn off like state automatically
//    //        if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
//    //    }
//    //    else
//    //    {
//    //        thumbsDownIconImage.color = Color.white; // Clear back to standard tint
//    //    }
//    //}

//    //// --- PASTE THIS EXACT RESET BLOCK INSIDE YOUR EXISTING OpenFullPostMode() METHOD ---
//    //// Place this at the bottom inside OpenFullPostMode to clear old highlights when swapping articles:
//    //private void ResetThumbsLayoutState()
//    //{
//    //    isLiked = false;
//    //    isDisliked = false;
//    //    if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
//    //    if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
//    //}

//    public void ToggleThumbsUpInteraction()
//    {
//        if (openArticleContext == null) return;

//        openArticleContext.playerLikedThisTemporaryState = !openArticleContext.playerLikedThisTemporaryState;

//        if (openArticleContext.playerLikedThisTemporaryState)
//        {
//            openArticleContext.playerDislikedThisTemporaryState = false; // Turn off dislike automatically
//        }

//        RefreshThumbsVisualsFromArticleData();
//    }

//    public void ToggleThumbsDownInteraction()
//    {
//        if (openArticleContext == null) return;

//        openArticleContext.playerDislikedThisTemporaryState = !openArticleContext.playerDislikedThisTemporaryState;

//        if (openArticleContext.playerDislikedThisTemporaryState)
//        {
//            openArticleContext.playerLikedThisTemporaryState = false; // Turn off like automatically
//        }

//        RefreshThumbsVisualsFromArticleData();
//    }

//    //// Call this inside your existing OpenFullPostMode method right at the bottom!
//    //public void RefreshThumbsVisualsFromArticleData()
//    //{
//    //    if (openArticleContext == null) return;

//    //    // Apply authentic tint highlights depending on the current stored data state of the article
//    //    if (openArticleContext.playerLikedThisTemporaryState)
//    //    {
//    //        if (thumbsUpIconImage != null) thumbsUpIconImage.color = new Color(0.2f, 0.6f, 1f, 1f); // Vibrant Blue
//    //        if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
//    //    }
//    //    else if (openArticleContext.playerDislikedThisTemporaryState)
//    //    {
//    //        if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
//    //        if (thumbsDownIconImage != null) thumbsDownIconImage.color = new Color(1f, 0.2f, 0.2f, 1f); // Vibrant Red
//    //    }
//    //    else
//    //    {
//    //        // Reset to clean default white states if nothing is currently selected
//    //        if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
//    //        if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
//    //    }
//    //}

//    public void RefreshThumbsVisualsFromArticleData()
//    {
//        // Safety check: if no article context is assigned, force a fallback state immediately
//        if (openArticleContext == null)
//        {
//            if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
//            if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
//            return;
//        }

//        // Explicitly check for verified True boolean flags saved inside the specific article
//        if (openArticleContext.playerLikedThisTemporaryState)
//        {
//            if (thumbsUpIconImage != null) thumbsUpIconImage.color = new Color(0.2f, 0.6f, 1f, 1f); // Blue
//            if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
//        }
//        else if (openArticleContext.playerDislikedThisTemporaryState)
//        {
//            if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
//            if (thumbsDownIconImage != null) thumbsDownIconImage.color = new Color(1f, 0.2f, 0.2f, 1f); // Red
//        }
//        else
//        {
//            // FORCE DEFAULT CLEAR IF BOTH CONTEXTS ARE FALSE FOR THIS ARTICLE
//            if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
//            if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
//        }
//    }



//    [Header("Next Day Transition Targets")]
//    public GameObject postGameSummaryCanvasView; // Drag PostGameSummaryCanvasView here

//    [Header("End Game Screens")]
//    public GameObject finalGameOverCanvasView; // Drag your brand new GameOver screen panel here!


//    /// <summary>
//    /// Closes completion screens, shuts off temporary website views, advances the calendar, 
//    /// and populates the feed with the next day's catalog.
//    /// </summary>
//    public void ProceedToNextDay()
//    {
//        // --- ADD THIS 3-DAY CHECK AT THE ABSOLUTE TOP OF THE METHOD ---
//        if (currentDay >= 3)
//        {
//            TriggerFinalGameOverState();
//            return;
//        }
//        // --------------------------------------------------------------

//        Debug.Log("[PROGRESSION] Advancing calendar schedule and resetting runtime states...");

//        // 1. Shut off the summary screens and popups completely
//        if (postGameSummaryCanvasView != null) postGameSummaryCanvasView.SetActive(false);
//        if (externalWebsitePopup != null) externalWebsitePopup.SetActive(false);

//        // 2. Force the App Window Workspace active
//        if (appWindowPanelObject != null) appWindowPanelObject.SetActive(true);

//        //// 3. Clear existing artifacts and shift the primary presentation view forward
//        if (fullPostPopupPanel != null)
//        {
//            fullPostPopupPanel.gameObject.SetActive(false);
//            Debug.Log("[PROGRESSION] FullPostModePopup set active.");
//        }

//        // 4. Advance day count tracker variable 
//        currentDay++;
//        Debug.Log($"[PROGRESSION] Internal tracking advanced to Day: {currentDay}");

//        // 5. Reinitialize the visual article scroll row nodes for the new pool loadout
//        InitializeDayLoadout(currentDay);

//        // 6. Automatically kick off the countdown tracking clocks again for the new level loop
//        if (BatteryTimeManager.Instance != null)
//        {
//            BatteryTimeManager.Instance.LaunchTimerEngine();
//        }


//    }

//    private void TriggerFinalGameOverState()
//    {
//        Debug.Log("[END GAME] All 3 days complete. Shutting down systems and showing Game Over screen.");

//        // 1. Terminate the battery/time engine countdown loops
//        if (BatteryTimeManager.Instance != null)
//        {
//            // Forcibly halts the Update countdown tickers
//            BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
//        }

//        // 2. Shut off all temporary working gameplay windows
//        if (postGameSummaryCanvasView != null) postGameSummaryCanvasView.SetActive(false);
//        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);
//        if (appWindowPanelObject != null) appWindowPanelObject.SetActive(false);
//        if (externalWebsitePopup != null) externalWebsitePopup.SetActive(false);

//        // --- NEW HUD OVERLAY TOGGLES ---
//        // 3. Clear HUD layout elements completely off-screen for a clean Game Over view
//        if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(false);
//        if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(false);
//        // --------------------------------

//        // 4. Bring up the definitive final game over canvas display
//        if (finalGameOverCanvasView != null)
//        {
//            finalGameOverCanvasView.SetActive(true);
//        }
//        else
//        {
//            Debug.LogError("[END GAME] Final GameOver Canvas View slot is empty in the Inspector!");
//        }

//    }

//}





























































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

    [Header("End Game Screens")]
    public GameObject finalGameOverCanvasView; // Drag your GameOver screen panel here!

    [Header("Thumbs Interaction Custom Elements")]
    public Image thumbsUpIconImage;   // Drag the Image component of your Thumbs Up button here
    public Image thumbsDownIconImage; // Drag the Image component of your Thumbs Down button here

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

                //if (onboardingPanelObject != null)
                //{
                //    // Show AppWindowPanel in the background; Onboarding's OnEnable() locks interaction
                //    appWindowPanelObject.SetActive(true);
                //    onboardingPanelObject.SetActive(true);
                //    onboardingPanelObject.transform.SetAsLastSibling();
                //}
                //else
                //{
                //    if (BatteryTimeManager.Instance != null)
                //    {
                //        BatteryTimeManager.Instance.LaunchTimerEngine();
                //    }
                //    appWindowPanelObject.SetActive(true);
                //    if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(true);
                //    if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(true);
                //}
            }
        

            //// Open onboarding on Day 1 (this automatically pauses the battery timer until closed)
            //if (onboardingPanelObject != null)
            //{
            //    onboardingPanelObject.SetActive(true);
            //}
            //else
            //{


            //    // Fallback in case no onboarding panel is assigned
            //    appWindowPanelObject.SetActive(true);
            //    if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(true);
            //    if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(true);
            //}
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

        if (shareButtonBg != null)
        {
            //shareButtonBg.color = Color.purple;
            if (shareUpSprite != null) shareButtonBg.sprite = shareUpSprite;
        }
        if (reportButtonBg != null)
        {
            //reportButtonBg.color = Color.purple;
            if (reportUpSprite != null) reportButtonBg.sprite = reportUpSprite;
        }

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
        currentBaseSequenceLength++; // Add penalty number onto difficulty progression tracker
        StartCoroutine(PlaySequencePatternRoutine(currentBaseSequenceLength));
    }
    private void ForceBypassOpenExternalWebsite()
    {
        if (externalWebsitePopup != null && openArticleContext != null)
        {
            bool imageOnlyMode = openArticleContext.cheatSheet != null && openArticleContext.cheatSheet.onlyDisplayImage;
            bool hasValidImage = openArticleContext.images != null && openArticleContext.images.Length > 0 && openArticleContext.images[0].imageSprite != null;

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

            // 3. Image display
            if (hasValidImage)
            {
                if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);

                if (externalWebsiteImageComponent != null)
                {
                    externalWebsiteImageComponent.gameObject.SetActive(true);
                    externalWebsiteImageComponent.sprite = openArticleContext.images[0].imageSprite;
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

            //if (externalWebsitePopup != null && openArticleContext != null)
            //{
            //    bool imageOnlyMode = openArticleContext.cheatSheet != null && openArticleContext.cheatSheet.onlyDisplayImage;
            //    bool hasValidImage = openArticleContext.images != null && openArticleContext.images.Length > 0 && openArticleContext.images[0].imageSprite != null;

            //    if (imageOnlyMode)
            //    {
            //        // Hide headline and external body text when Only Display Image is checked
            //        if (websiteHeadlinerText != null)
            //        {
            //            websiteHeadlinerText.gameObject.SetActive(false);
            //        }

            //        if (websiteContentBodyText != null)
            //        {
            //            websiteContentBodyText.gameObject.SetActive(false);
            //        }
            //    }
            //    else
            //    {
            //        // Normal external link display: show headline and externalLinkBodyText
            //        if (websiteHeadlinerText != null)
            //        {
            //            websiteHeadlinerText.gameObject.SetActive(true);
            //            websiteHeadlinerText.text = $"{openArticleContext.headliner}";
            //        }

            //        if (websiteContentBodyText != null)
            //        {
            //            websiteContentBodyText.gameObject.SetActive(true);
            //            string rawExternalBody = string.IsNullOrEmpty(openArticleContext.externalLinkBodyText)
            //                ? ""
            //                : openArticleContext.externalLinkBodyText;
            //            string cleanBody = rawExternalBody.Replace("[LINK]", "").Replace("[PUPPY]", "");
            //            websiteContentBodyText.text = cleanBody;
            //            //websiteContentBodyText.margin = new Vector4(0f, 90f, 0f, 0f); //makes too much space between external link text body and the image
            //        }
            //    }

            //    // Handle image display
            //    if (hasValidImage)
            //    {
            //        if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);

            //        if (externalWebsiteImageComponent != null)
            //        {
            //            externalWebsiteImageComponent.gameObject.SetActive(true);
            //            externalWebsiteImageComponent.sprite = openArticleContext.images[0].imageSprite;

            //            RectTransform imageRect = externalWebsiteImageComponent.GetComponent<RectTransform>();
            //            if (imageRect != null)
            //            {
            //                if (imageOnlyMode)
            //                {
            //                    // Center the 404 image in the popup when no text or headline is shown
            //                    imageRect.anchorMin = new Vector2(0.5f, 0.5f);
            //                    imageRect.anchorMax = new Vector2(0.5f, 0.5f);
            //                    imageRect.pivot = new Vector2(0.5f, 0.5f);
            //                    imageRect.anchoredPosition = Vector2.zero;
            //                }
            //                else
            //                {
            //                    // Normal top-center alignment above body text
            //                    imageRect.anchorMin = new Vector2(0.5f, 1f);
            //                    imageRect.anchorMax = new Vector2(0.5f, 1f);
            //                    imageRect.pivot = new Vector2(0.5f, 1f);
            //                    imageRect.anchoredPosition = new Vector2(0f, -10f);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (externalWebsiteImageComponent != null) externalWebsiteImageComponent.gameObject.SetActive(false);
            //    }

            //if (websiteHeadlinerText != null)
            //{
            //    websiteHeadlinerText.text = $"{openArticleContext.headliner}";
            //}
            //if (websiteContentBodyText != null)
            //{
            //    string cleanBody = openArticleContext.bodyText.Replace("[LINK]", "").Replace("[PUPPY]", "");
            //    websiteContentBodyText.text = cleanBody;
            //    websiteContentBodyText.margin = new Vector4(0f, 90f, 0f, 0f); // 90px automatic margin window
            //}
            //if (openArticleContext.images != null && openArticleContext.images.Length > 0 && openArticleContext.images[0].imageSprite != null)
            //{
            //    if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);
            //    if (externalWebsiteImageComponent != null)
            //    {
            //        externalWebsiteImageComponent.gameObject.SetActive(true);
            //        externalWebsiteImageComponent.sprite = openArticleContext.images[0].imageSprite;
            //        RectTransform imageRect = externalWebsiteImageComponent.GetComponent<RectTransform>();
            //        if (imageRect != null)
            //        {
            //            imageRect.anchorMin = new Vector2(0.5f, 1f); // Pin to top center of body copy boundaries
            //            imageRect.anchorMax = new Vector2(0.5f, 1f);
            //            imageRect.pivot = new Vector2(0.5f, 1f);
            //            imageRect.anchoredPosition = new Vector2(0f, -10f); // Align into the margin clearance gap
            //        }
            //    }
            //}
            //else
            //{
            //    if (externalWebsiteImageComponent != null) externalWebsiteImageComponent.gameObject.SetActive(false);
            //}
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
        else if (!isLegit && reportButtonBg != null) reportButtonBg.color = Color.red;
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
        if (!canProcessVerdict || openArticleContext == null) return;
        canProcessVerdict = false;

        // Swap pressed verdict button to Down-State sprite
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
            TriggerGrandmaExpression(happyFaceSprite);
            if (currentFriendScore % 15 == 0)
            {
                helperUsesRemaining++;
                UpdateHelperUI();
            }
        }
        else
        {
            currentFriendScore = Mathf.Max(0, currentFriendScore - 3);
            TriggerGrandmaExpression(sadFaceSprite);
        }
        UpdateScoreboardDisplay();
        if (currentFriendScore <= 0)
        {
            Debug.LogWarning("[GAME OVER QUEUED] Score hit 0 or lower. Keeping popup open for 1.5s...");
            Invoke(nameof(CloseAndAnimateFeedShift), 1.5f);
            //Invoke(nameof(TriggerFinalGameOverState), 1.55f);
            Invoke(nameof(EvaluateAndTriggerEnding), 1.55f);
            return;
        }
        Invoke(nameof(CloseAndAnimateFeedShift), 1.5f);
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
            if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
            if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
            return;
        }
        if (openArticleContext.playerLikedThisTemporaryState)
        {
            if (thumbsUpIconImage != null) thumbsUpIconImage.color = new Color(0.2f, 0.6f, 1f, 1f);
            if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
        }
        else if (openArticleContext.playerDislikedThisTemporaryState)
        {
            if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
            if (thumbsDownIconImage != null) thumbsDownIconImage.color = new Color(1f, 0.2f, 0.2f, 1f);
        }
        else
        {
            if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
            if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
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
        if (currentFriendScore <= 2) // Bad Ending: 0 to 50
        {
            Debug.Log("[END GAME] Triggering BAD ENDING (0 - 50)");
            //if (GameOverCanvasView != null) GameOverCanvasView.SetActive(true);
            if (badEndingCanvasView != null) badEndingCanvasView.SetActive(true);
            else Debug.LogError("[END GAME] GameOverCanvasView (Bad Ending) slot is empty in Inspector!");
        }
        else if (currentFriendScore <= 10) // Neutral Ending: 51 to 100
        {
            Debug.Log("[END GAME] Triggering NEUTRAL ENDING (51 - 100)");
            if (neutralEndingCanvasView != null) neutralEndingCanvasView.SetActive(true);
            else Debug.LogError("[END GAME] neutralEndingCanvasView slot is empty in Inspector!");
        }
        else // Good Ending: 101 to 152+
        {
            Debug.Log("[END GAME] Triggering GOOD ENDING (101 - 152)");
            if (goodEndingCanvasView != null) goodEndingCanvasView.SetActive(true);
            else Debug.LogError("[END GAME] goodEndingCanvasView slot is empty in Inspector!");
        }
    }
    //private void TriggerNeutralEnding()//Neutral Ending
    //{
    //    Debug.Log("Player has 51-100 reliability remaining");
    //    if (BatteryTimeManager.Instance != null)
    //    {
    //        BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
    //    }
    //    if (neutralEndingCanvasView != null) neutralEndingCanvasView.SetActive(false);
    //    if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);
    //    if (appWindowPanelObject != null) appWindowPanelObject.SetActive(false);
    //    if (externalWebsitePopup != null) externalWebsitePopup.SetActive(false);
    //    if (captchaPanelObject != null) captchaPanelObject.SetActive(false); // Safety wipe
    //    if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(false);
    //    if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(false);
    //    if (GameOverCanvasView != null)
    //    {
    //        GameOverCanvasView.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("[END GAME] Final GameOver Canvas View slot is empty in the Inspector!");
    //    }
    //}

    //private void TriggerGoodEnding()//Good Ending
    //{
    //    Debug.Log("Player has 101-152 reliability remaining");
    //    if (BatteryTimeManager.Instance != null)
    //    {
    //        BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
    //    }
    //    if (goodEndingCanvasView != null) goodEndingCanvasView.SetActive(false);
    //    if (fullPostPopupPanel != null) fullPostPopupPanel.gameObject.SetActive(false);
    //    if (appWindowPanelObject != null) appWindowPanelObject.SetActive(false);
    //    if (externalWebsitePopup != null) externalWebsitePopup.SetActive(false);
    //    if (captchaPanelObject != null) captchaPanelObject.SetActive(false); // Safety wipe
    //    if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(false);
    //    if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(false);
    //    if (GameOverCanvasView != null)
    //    {
    //        GameOverCanvasView.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("[END GAME] Final GameOver Canvas View slot is empty in the Inspector!");
    //    }
    //}

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
    }

    private Sprite GetReliabilityTierSprite(int score)
    {
        if (reliabilityBarSprites == null || reliabilityBarSprites.Length < 5) return null;

        if (score <= 5) return reliabilityBarSprites[0];       // 1 bar: 0-37
        if (score <= 15 && score >= 6) return reliabilityBarSprites[1];       // 2 bars: 38-75
        if (score <= 25 && score >= 16) return reliabilityBarSprites[2];      // 3 bars: 76-150
        if (score <= 36 && score >= 26) return reliabilityBarSprites[3];      // 4 bars: 151-188
        return reliabilityBarSprites[4];                        // 5 bars: 189-227+
        //if (score <= 37) return reliabilityBarSprites[0];       // 1 bar: 0-37
        //if (score <= 75 && score >= 38) return reliabilityBarSprites[1];       // 2 bars: 38-75
        //if (score <= 150 && score >= 76) return reliabilityBarSprites[2];      // 3 bars: 76-150
        //if (score <= 188 &&score >= 151) return reliabilityBarSprites[3];      // 4 bars: 151-188
        //return reliabilityBarSprites[4];                        // 5 bars: 189-227+
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

