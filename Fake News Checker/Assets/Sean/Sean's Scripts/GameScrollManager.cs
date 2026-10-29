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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitializeDayLoadout(currentDay);
        UpdateScoreboardDisplay();
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

        fullPostTitleText.text = data.title;
        fullPostContentBody.text = data.bodyText;

        shareButtonBg.color = Color.white;
        reportButtonBg.color = Color.white;

        if (data.hasExternalLink)
        {
            linkClickableButton.SetActive(true);
            if (externalLinkButtonLabel != null)
            {
                externalLinkButtonLabel.text = $"https://tellagram.com{data.articleID.ToLower()}";
            }
        }
        else
        {
            linkClickableButton.SetActive(false);
        }

        fullPostPopupPanel.gameObject.SetActive(true);

        // Populate comments when full post opens
        LoadCommentsToView(data);
    }

    public void ClickExternalLink()
    {
        if (externalWebsitePopup != null && openArticleContext != null)
        {
            if (websiteContentBodyText != null)
            {
                websiteContentBodyText.text = $"[SECURE WEBSITE FEED]\n\nURL: https://tellagram.com\n\n{openArticleContext.bodyText}";
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
        if (helperUsesRemaining <= 0 || openArticleContext == null) return;

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

        if (doomScrollFeedContainer.childCount == 0)
        {
            BatteryTimeManager.Instance.EvaluateRunCompletion();
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
}

