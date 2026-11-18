using System.Collections.Generic;
using UnityEngine;

public enum SectorType { LocalNews, PopCulture, Finance, HealthScience, BreakingNews }

[System.Serializable]
public class ArticleCheatSheet
{
    public SectorType correctSector;
    public bool isFlawed;
    public bool approveAndPublish; // true = Approve, false = Reject

    [Header("Display Overrides")]
    [Tooltip("When checked, the external link popup hides the headline and body text and ONLY displays the image (e.g. for a 404 image).")]
    public bool onlyDisplayImage;

    [Header("Hidden Error Tags")]
    public bool unverifiedAuthor;
    public bool falsifiedCredentials;
    public bool imageManipulation;
    public bool outOfContextMedia;
    public bool inaccurateStats;
    public bool falseTimeline;
    public bool uncitedClaims;
    public bool typos;

    
}

// Keep all your original code at the top exactly the same...
[CreateAssetMenu(fileName = "NewArticlePage", menuName = "Case System/Article Page")]
public class ArticlePageData : ScriptableObject
{
    [HideInInspector] public bool helperWasUsedOnThisArticleState = false;

    [Header("ID Configuration")]
    public string articleID;
    public string title;

    [TextArea(2, 4)] public string headliner;

    [Header("Body Text Separation")]
    [Tooltip("Text shown ONLY inside the Full Post popup.")]
    [TextArea(5, 12)] public string bodyText;

    [Tooltip("Text shown inside the Full Post popup right before the clickable external URL button.")]
    [TextArea(2, 5)] public string textBeforeUrl;

    [Tooltip("Text shown ONLY inside the External Link website popup.")]
    [TextArea(5, 12)] public string externalLinkBodyText;

    [Header("New Feed Additions")]
    [Tooltip("Check this if this specific post features an external website link simulation")]
    public bool hasExternalLink;

    [TextArea(3, 5)]
    [Tooltip("Add comments for this post here. Leave empty if this post has no comments.")]
    public List<string> customComments;

    [Header("Verification Cheat Sheet")]
    public ArticleCheatSheet cheatSheet;

    [HideInInspector] public bool playerLikedThisTemporaryState = false;
    [HideInInspector] public bool playerDislikedThisTemporaryState = false;

    [System.Serializable]
    public class EmbeddedImage
    {
        public Sprite imageSprite;
        //public Sprite imageSprite2;
        [Tooltip("The text block index after which this image should appear.")]
        public int insertAfterParagraphIndex;
    }

    [Header("Image Separation")]
    [Tooltip("Images shown ONLY inside the Full Post popup.")]
    public EmbeddedImage[] images;

    [Tooltip("Images shown ONLY inside the External Link website popup.")]
    public EmbeddedImage[] externalLinkImages;
}
