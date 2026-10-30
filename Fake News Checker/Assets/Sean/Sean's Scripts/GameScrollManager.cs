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
//    public Transform doomScrollFeedContainer; // Vertical layout group for main dashboard list
//    public GameObject postListItemPrefab;    // List row button wrapper
//    public RectTransform fullPostPopupPanel;  // Main full screen app popup (90% Screen)

//    [Header("Full Post Display Nodes")]
//    public TMP_Text fullPostTitleText;
//    public TMP_Text fullPostContentBody;
//    public GameObject linkClickableButton;    // External link hook layout if applicable
//    public GameObject externalWebsitePopup;  // Simulated popup for website redirects

//    [Header("Direct Interaction Buttons")]
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

//    [Header("External Website Component Targets")]
//    public TMP_Text websiteContentBodyText; // <-- Add this component slot field tracker here
//    public TMP_Text externalLinkButtonLabel; // <-- Add this component slot to trace link text changes

//    // ... inside your GameScrollManager class variables area ...
//    [Header("Comments List Component Hooks")]
//    public Transform commentsScrollContentContainer; // Target Viewport/Content box
//    public GameObject commentRowTextParagraphPrefab;  // Simple text row component blueprint asset


//    // Update this method within your script file
//    public void OpenFullPostMode(ArticlePageData data, GameObject sourceRowInstance)
//    {
//        openArticleContext = data;
//        currentActiveFeedRowInstance = sourceRowInstance;

//        fullPostTitleText.text = data.title;
//        fullPostContentBody.text = data.bodyText;

//        // Reset layout highlighter tints dynamically
//        shareButtonBg.color = Color.white;
//        reportButtonBg.color = Color.white;

//        // Point 4: Automatically apply custom website link labels if they exist
//        if (data.hasExternalLink)
//        {
//            linkClickableButton.SetActive(true);
//            if (externalLinkButtonLabel != null)
//            {
//                // Changes the text of your button directly to the web address parameter string
//                externalLinkButtonLabel.text = $"https://tellagram.com{data.articleID.ToLower()}";
//            }
//        }
//        else
//        {
//            linkClickableButton.SetActive(false);
//        }

//        fullPostPopupPanel.gameObject.SetActive(true);
//    }

//    // Point 3: Instantly populates the external webpage frame when targeted
//    public void ClickExternalLink()
//    {
//        if (externalWebsitePopup != null && openArticleContext != null)
//        {
//            if (websiteContentBodyText != null)
//            {
//                // Feeds your body paragraphs directly into your mock browser view window!
//                websiteContentBodyText.text = $"[BROWSING SECURE CONNECTION]\n\nSOURCE FEED URL: https://tellagram.com\n\n{openArticleContext.bodyText}";
//            }
//            externalWebsitePopup.SetActive(true);
//        }
//    }

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    private void Start()
//    {
//        InitializeDayLoadout(currentDay);
//        UpdateScoreboardDisplay();
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
//        helperUsesRemaining = 3; // Reset helpers daily
//        UpdateHelperUI();

//        // Wipe active list cleanly
//        foreach (Transform child in doomScrollFeedContainer) Destroy(child.gameObject);

//        List<ArticlePageData> activePool = (dayNumber == 1) ? day1ArticlesPool :
//                                           (dayNumber == 2) ? day2ArticlesPool : day3ArticlesPool;

//        foreach (var article in activePool)
//        {
//            GameObject row = Instantiate(postListItemPrefab, doomScrollFeedContainer);
//            row.GetComponentInChildren<TMP_Text>().text = article.title;

//            // Map list selection actions directly
//            Button btn = row.GetComponent<Button>();
//            btn.onClick.AddListener(() => OpenFullPostMode(article, row));
//        }

//        LayoutRebuilder.ForceRebuildLayoutImmediate(doomScrollFeedContainer.GetComponent<RectTransform>());
//    }

//    //public void OpenFullPostMode(ArticlePageData data, GameObject sourceRowInstance)
//    //{
//    //    openArticleContext = data;
//    //    currentActiveFeedRowInstance = sourceRowInstance;

//    //    fullPostTitleText.text = data.title;
//    //    fullPostContentBody.text = data.bodyText;

//    //    // Reset highlight states dynamically
//    //    shareButtonBg.color = Color.white;
//    //    reportButtonBg.color = Color.white;

//    //    // Verify if banner link exists in specific article metadata
//    //    bool hasExternalLink = !string.IsNullOrEmpty(data.articleID) && (data.articleID.Contains("LINK") || data.articleID.Contains("PUPPY"));
//    //    linkClickableButton.SetActive(hasExternalLink);

//    //    fullPostPopupPanel.gameObject.SetActive(true);
//    //}

//    //public void ClickExternalLink()
//    //{
//    //    if (externalWebsitePopup != null)
//    //    {
//    //        externalWebsitePopup.SetActive(true);
//    //    }
//    //}

//    public void UseHelperLifeline()
//    {
//        if (helperUsesRemaining <= 0 || openArticleContext == null) return;

//        helperUsesRemaining--;
//        UpdateHelperUI();

//        // True means Legit (Share), False means Fake (Report)
//        bool isLegit = openArticleContext.cheatSheet.approveAndPublish;

//        if (isLegit) shareButtonBg.color = Color.green;
//        else reportButtonBg.color = Color.red;
//    }

//    public void ProcessPlayerVerdict(bool choseShare)
//    {
//        if (openArticleContext == null) return;

//        // Core validation logic: sheet.approveAndPublish true = legit/share, false = fake/report
//        bool isArticleLegit = openArticleContext.cheatSheet.approveAndPublish;
//        bool isCorrect = (choseShare == isArticleLegit);

//        if (isCorrect)
//        {
//            currentFriendScore += 5;
//            TriggerGrandmaExpression(happyFaceSprite);

//            // Gain extra helper reward for every 3 right answers milestone tracker
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

//        // Auto-close post window, purge completed entry, and slide the remaining layout up
//        Invoke(nameof(CloseAndAnimateFeedShift), 1.5f);
//    }

//    private void CloseAndAnimateFeedShift()
//    {
//        fullPostPopupPanel.gameObject.SetActive(false);

//        if (currentActiveFeedRowInstance != null)
//        {
//            Destroy(currentActiveFeedRowInstance);
//        }

//        openArticleContext = null;
//        currentActiveFeedRowInstance = null;

//        // Force UI rendering stack recalculation instantly
//        Canvas.ForceUpdateCanvases();
//        LayoutRebuilder.ForceRebuildLayoutImmediate(doomScrollFeedContainer.GetComponent<RectTransform>());

//        // Win Condition check for empty feed before timeout
//        if (doomScrollFeedContainer.childCount == 0)
//        {
//            BatteryTimeManager.Instance.EvaluateRunCompletion();
//        }
//    }

//    private void TriggerGrandmaExpression(Sprite moodSprite)
//    {
//        if (grandmaAvatarDisplay == null) return;
//        grandmaAvatarDisplay.sprite = moodSprite;
//        reactionTimer = 1.5f;
//    }

//    private void UpdateScoreboardDisplay()
//    {
//        // Broadcast updates to the scoreboard UI
//        BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
//    }

//    private void UpdateHelperUI()
//    {
//        if (helperUsesCounterText != null)
//            helperUsesCounterText.text = $"x{helperUsesRemaining}";
//    }
//}


using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameScrollManager : MonoBehaviour
{
    public static GameScrollManager Instance;

    [Header("Current Run Progression")]
    public int currentDay = 1; // Tracks Day 1 (10 posts), Day 2 (15 posts), Day 3 (20 posts)
    private int currentFriendScore = 2; // Starts at 2

    [Header("Grandma Profile Hooks")]
    public Image grandmaAvatarDisplay;
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
    public GameObject linkClickableButton;    // External link button wrapper object

    [Header("External Website Component Targets")]
    public GameObject externalWebsitePopup;
    public TMP_Text websiteContentBodyText;   // Populates copy text to mock browser
    public TMP_Text externalLinkButtonLabel;  // Overrides button text to show the custom URL

    [Header("Comments List Component Hooks")]
    public Transform commentsScrollContentContainer; // Viewport/Content inside CommentsSectionScrollView
    public GameObject commentRowTextParagraphPrefab;  // Simple Text (TMP) row prefab asset

    [Header("Direct Verdict Buttons")]
    public Image shareButtonBg;  // Background box graphic that glows
    public Image reportButtonBg; // Background box graphic that glows

    [Header("Helper System")]
    public TMP_Text helperUsesCounterText;
    public Button helperActionButton;
    private int helperUsesRemaining = 3;

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


    public void LaunchApplicationFromDesktop()
    {
        if (appWindowPanelObject != null)
        {
            // 1. Reveal your main 90% application window workspace
            appWindowPanelObject.SetActive(true);

            // 2. Dynamically reveal HUD widgets only when the app boots up
            if (grandmaProfilePanelObject != null) grandmaProfilePanelObject.SetActive(true);
            if (helperLifelineWidgetObject != null) helperLifelineWidgetObject.SetActive(true);

            // 3. Start your daily articles pool loading system
            InitializeDayLoadout(1);

            // 4. Kick off your 7-bar segmented countdown timer clock loop
            if (BatteryTimeManager.Instance != null)
            {
                BatteryTimeManager.Instance.LaunchTimerEngine();
            }
        }
    }
        //public void LaunchApplicationFromDesktop()
        //{
        //    if (appWindowPanelObject != null)
        //    {
        //        // 1. Reveal your main 90% application window
        //        appWindowPanelObject.SetActive(true);

        //        // 2. Start your daily articles pool loading system
        //        InitializeDayLoadout(1);

        //        // 3. Kick off your 7-bar segmented countdown timer clock loop
        //        if (BatteryTimeManager.Instance != null)
        //        {
        //            BatteryTimeManager.Instance.LaunchTimerEngine();
        //            Debug.Log("[DESKTOP] Application started successfully. Battery timer is ticking!");
        //        }
        //    }
        //}

        private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Clean out memory leaks across all three asset pool tables on level startup
        ResetAllArticleMemoryStates(day1ArticlesPool);
        ResetAllArticleMemoryStates(day2ArticlesPool);
        ResetAllArticleMemoryStates(day3ArticlesPool);

        InitializeDayLoadout(currentDay);
        UpdateScoreboardDisplay();

        InitializeDayLoadout(currentDay);
        UpdateScoreboardDisplay();
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
            }
        }
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

    public void InitializeDayLoadout(int dayNumber)
    {
        currentDay = dayNumber;
        helperUsesRemaining = 3;
        UpdateHelperUI();

        foreach (Transform child in doomScrollFeedContainer) Destroy(child.gameObject);

        List<ArticlePageData> activePool = (dayNumber == 1) ? day1ArticlesPool :
                                           (dayNumber == 2) ? day2ArticlesPool : day3ArticlesPool;

        foreach (var article in activePool)
        {
            GameObject row = Instantiate(postListItemPrefab, doomScrollFeedContainer);
            row.GetComponentInChildren<TMP_Text>().text = article.title;

            Button btn = row.GetComponent<Button>();
            btn.onClick.AddListener(() => OpenFullPostMode(article, row));
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(doomScrollFeedContainer.GetComponent<RectTransform>());
    }

    public void OpenFullPostMode(ArticlePageData data, GameObject sourceRowInstance)
    {
        openArticleContext = data;
        currentActiveFeedRowInstance = sourceRowInstance;
        helperUsedOnCurrentArticle = false;

        // --- PASTE THESE TWO EXTRA SAFETIES DIRECTLY IN THIS METHOD LOOP ---
        // Safely forces the current state markers back to clean default settings every time you choose a new row header
        if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
        if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
        // -------------------------------------------------------------------

        fullPostTitleText.text = data.title;
        fullPostContentBody.text = data.bodyText;

        //appWindowPanelObject.gameObject.SetActive(false); //turns appwindow off

        // Continue running your existing code downwards from here...
        fullPostPopupPanel.gameObject.SetActive(true);
        LoadCommentsToView(data);

        shareButtonBg.color = Color.white;
        reportButtonBg.color = Color.white;

        // Automatically updates the image thumbnail if data exists inside your asset arrays
        if (data.images != null && data.images.Length > 0 && data.images[0].imageSprite != null)
        {
            fullPostArticleImageComponent.gameObject.SetActive(true);
            fullPostArticleImageComponent.sprite = data.images[0].imageSprite;
        }
        else
        {
            fullPostArticleImageComponent.gameObject.SetActive(false); // Hide if no image assigned
        }

        //// ... your existing code setting text, titles, etc ...

        //fullPostPopupPanel.gameObject.SetActive(true);
        //LoadCommentsToView(data);

        // Add this line inside your OpenFullPostMode method
        helperUsedOnCurrentArticle = false;

        // Locate this exact section inside your existing OpenFullPostMode method
        if (data.hasExternalLink)
        {
            // 1. Force the parent button gameobject active instantly on screen
            linkClickableButton.SetActive(true);
            if (externalLinkButtonLabel != null)
            {
                //externalLinkButtonLabel.text = $"https://tellagram.com{data.articleID.ToLower()}";

                // 2. Derive the unique domain path from your current open article metadata
                string customDomain = $"https://www.{data.articleID.ToLower().Trim()}.org/feed-post";
                //externalLinkButtonLabel.text = customDomain;

                externalLinkButtonLabel.text = $"https://www.{data.articleID.ToLower().Trim()}.org";

            }
        }
        else
        {
            linkClickableButton.SetActive(false);
        }

        fullPostPopupPanel.gameObject.SetActive(true);

        // Populate comments when full post opens
        LoadCommentsToView(data);

        // --- PASTE THIS EXACT LINE AT THE ABSOLUTE BOTTOM OF THIS METHOD ---
        RefreshThumbsVisualsFromArticleData();
    }

    //public void ClickExternalLink()
    //{
    //    if (externalWebsitePopup != null && openArticleContext != null)
    //    {
    //        if (websiteContentBodyText != null)
    //        {
    //            websiteContentBodyText.text = $"[SECURE WEBSITE FEED]\n\nURL: https://tellagram.com\n\n{openArticleContext.bodyText}";
    //        }
    //        externalWebsitePopup.SetActive(true);
    //    }
    //}

    //public void ClickExternalLink()
    //{
    //    if (externalWebsitePopup != null && openArticleContext != null)
    //    {
    //        if (websiteContentBodyText != null)
    //        {
    //            // Clean out raw systemic brackets/hyperlink strings from displaying inside the viewer
    //            string cleanBody = openArticleContext.bodyText.Replace("[LINK]", "").Replace("[PUPPY]", "");

    //            websiteContentBodyText.text = $"[SECURE BROWSER FEED]\n\n{cleanBody}";
    //        }
    //        externalWebsitePopup.SetActive(true);
    //    }
    //}

    public void ClickExternalLink()
    {
        if (externalWebsitePopup != null && openArticleContext != null)
        {
            if (websiteContentBodyText != null)
            {
                // Simply strip away systemic brackets text clean
                string cleanBody = openArticleContext.bodyText.Replace("[LINK]", "").Replace("[PUPPY]", "");

                // Outputs ONLY the raw copy text of the article context directly into the window frame
                websiteContentBodyText.text = cleanBody;

                // Wipe out raw code strings from displaying inside the reader window frame
                //string cleanBody = openArticleContext.bodyText.Replace("[LINK]", "").Replace("[PUPPY]", "");

                // Dynamic URL constructor string using your Article Asset IDs directly!
                //string customGeneratedURL = $"www.{openArticleContext.articleID.ToLower().Trim()}.com/news/feed-post-01";

                //// Compiles your link address string dynamically as the absolute first line of text visible inside your browser view
                //websiteContentBodyText.text = $"🌐 BROWSER LINK: http://{customGeneratedURL}\n" +
                //                              $"🔒 CONNECTION SECURE & ENCRYPTED\n" +
                //                              $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                //                              $"{cleanBody}";
            }

            if (openArticleContext.images != null && openArticleContext.images.Length > 0 && openArticleContext.images[0].imageSprite != null)
            {
                fullPostPopupPanel.gameObject.SetActive(false); //turn it off

                externalWebsiteImageComponent.gameObject.SetActive(true);
                externalWebsiteImageComponent.sprite = openArticleContext.images[0].imageSprite;
            }
            else
            {
                externalWebsiteImageComponent.gameObject.SetActive(false);
            }
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
                commentRow.GetComponentInChildren<TMP_Text>().text = commentString;
            }
        }
        else
        {
            GameObject emptyNoticeRow = Instantiate(commentRowTextParagraphPrefab, commentsScrollContentContainer);
            emptyNoticeRow.GetComponentInChildren<TMP_Text>().text = "<i>No comments on this post.</i>";
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(commentsScrollContentContainer.GetComponent<RectTransform>());
    }

    public void UseHelperLifeline()
    {
        //if (helperUsesRemaining <= 0 || openArticleContext == null) return;

        //helperUsesRemaining--;
        //UpdateHelperUI();

        //bool isLegit = openArticleContext.cheatSheet.approveAndPublish;

        //if (isLegit) shareButtonBg.color = Color.green;
        //else reportButtonBg.color = Color.red;

        // Safety Gate: block usage if helper was already used on this article once
        if (helperUsesRemaining <= 0 || openArticleContext == null || helperUsedOnCurrentArticle) return;

        helperUsedOnCurrentArticle = true; // Lock it out immediately
        helperUsesRemaining--;
        UpdateHelperUI();

        bool isLegit = openArticleContext.cheatSheet.approveAndPublish;
        if (isLegit) shareButtonBg.color = Color.green;
        else reportButtonBg.color = Color.red;
    }

    public void ProcessPlayerVerdict(bool choseShare)
    {
        if (openArticleContext == null) return;

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
        Invoke(nameof(CloseAndAnimateFeedShift), 1.5f);
    }

    private void CloseAndAnimateFeedShift()
    {
        fullPostPopupPanel.gameObject.SetActive(false);

        if (currentActiveFeedRowInstance != null)
        {
            Destroy(currentActiveFeedRowInstance);
        }

        openArticleContext = null;
        currentActiveFeedRowInstance = null;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(doomScrollFeedContainer.GetComponent<RectTransform>());

        //if (doomScrollFeedContainer.childCount == 0)
        //{
        //    BatteryTimeManager.Instance.EvaluateRunCompletion();
        //}

        // Inside CloseAndAnimateFeedShift() under GameScrollManager.cs
        if (doomScrollFeedContainer.childCount == 0)
        {
            if (BatteryTimeManager.Instance != null)
            {
                BatteryTimeManager.Instance.EvaluateRunCompletion();
            }
        }
    }

    private void TriggerGrandmaExpression(Sprite moodSprite)
    {
        if (grandmaAvatarDisplay == null) return;
        grandmaAvatarDisplay.sprite = moodSprite;
        reactionTimer = 1.5f;
    }   

    private void UpdateScoreboardDisplay()
    {
        BatteryTimeManager.Instance.UpdateFriendsScoreUI(currentFriendScore);
    }

    private void UpdateHelperUI()
    {
        if (helperUsesCounterText != null)
            helperUsesCounterText.text = $"x{helperUsesRemaining}";
    }


    //thumbs up and down UI interactable aka likes and dislikes

    [Header("Thumbs Interaction Custom Elements")]
    public Image thumbsUpIconImage;   // Drag the Image component of your Thumbs Up button here
    public Image thumbsDownIconImage; // Drag the Image component of your Thumbs Down button here

    private bool isLiked = false;
    private bool isDisliked = false;

    //// Call this via the OnClick() event on your Thumbs Up UI Button component
    //public void ToggleThumbsUpInteraction()
    //{
    //    isLiked = !isLiked;
    //    if (isLiked)
    //    {
    //        thumbsUpIconImage.color = new Color(0.2f, 0.6f, 1f, 1f); // Vibrant Blue Highlight
    //        isDisliked = false; // Turn off dislike state automatically
    //        if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
    //    }
    //    else
    //    {
    //        thumbsUpIconImage.color = Color.white; // Clear back to standard tint
    //    }
    //}

    //// Call this via the OnClick() event on your Thumbs Down UI Button component
    //public void ToggleThumbsDownInteraction()
    //{
    //    isDisliked = !isDisliked;
    //    if (isDisliked)
    //    {
    //        thumbsDownIconImage.color = new Color(1f, 0.2f, 0.2f, 1f); // Vibrant Red Highlight
    //        isLiked = false; // Turn off like state automatically
    //        if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
    //    }
    //    else
    //    {
    //        thumbsDownIconImage.color = Color.white; // Clear back to standard tint
    //    }
    //}

    //// --- PASTE THIS EXACT RESET BLOCK INSIDE YOUR EXISTING OpenFullPostMode() METHOD ---
    //// Place this at the bottom inside OpenFullPostMode to clear old highlights when swapping articles:
    //private void ResetThumbsLayoutState()
    //{
    //    isLiked = false;
    //    isDisliked = false;
    //    if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
    //    if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
    //}

    public void ToggleThumbsUpInteraction()
    {
        if (openArticleContext == null) return;

        openArticleContext.playerLikedThisTemporaryState = !openArticleContext.playerLikedThisTemporaryState;

        if (openArticleContext.playerLikedThisTemporaryState)
        {
            openArticleContext.playerDislikedThisTemporaryState = false; // Turn off dislike automatically
        }

        RefreshThumbsVisualsFromArticleData();
    }

    public void ToggleThumbsDownInteraction()
    {
        if (openArticleContext == null) return;

        openArticleContext.playerDislikedThisTemporaryState = !openArticleContext.playerDislikedThisTemporaryState;

        if (openArticleContext.playerDislikedThisTemporaryState)
        {
            openArticleContext.playerLikedThisTemporaryState = false; // Turn off like automatically
        }

        RefreshThumbsVisualsFromArticleData();
    }

    //// Call this inside your existing OpenFullPostMode method right at the bottom!
    //public void RefreshThumbsVisualsFromArticleData()
    //{
    //    if (openArticleContext == null) return;

    //    // Apply authentic tint highlights depending on the current stored data state of the article
    //    if (openArticleContext.playerLikedThisTemporaryState)
    //    {
    //        if (thumbsUpIconImage != null) thumbsUpIconImage.color = new Color(0.2f, 0.6f, 1f, 1f); // Vibrant Blue
    //        if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
    //    }
    //    else if (openArticleContext.playerDislikedThisTemporaryState)
    //    {
    //        if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
    //        if (thumbsDownIconImage != null) thumbsDownIconImage.color = new Color(1f, 0.2f, 0.2f, 1f); // Vibrant Red
    //    }
    //    else
    //    {
    //        // Reset to clean default white states if nothing is currently selected
    //        if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
    //        if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
    //    }
    //}

    public void RefreshThumbsVisualsFromArticleData()
    {
        // Safety check: if no article context is assigned, force a fallback state immediately
        if (openArticleContext == null)
        {
            if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
            if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
            return;
        }

        // Explicitly check for verified True boolean flags saved inside the specific article
        if (openArticleContext.playerLikedThisTemporaryState)
        {
            if (thumbsUpIconImage != null) thumbsUpIconImage.color = new Color(0.2f, 0.6f, 1f, 1f); // Blue
            if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
        }
        else if (openArticleContext.playerDislikedThisTemporaryState)
        {
            if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
            if (thumbsDownIconImage != null) thumbsDownIconImage.color = new Color(1f, 0.2f, 0.2f, 1f); // Red
        }
        else
        {
            // FORCE DEFAULT CLEAR IF BOTH CONTEXTS ARE FALSE FOR THIS ARTICLE
            if (thumbsUpIconImage != null) thumbsUpIconImage.color = Color.white;
            if (thumbsDownIconImage != null) thumbsDownIconImage.color = Color.white;
        }
    }

}

