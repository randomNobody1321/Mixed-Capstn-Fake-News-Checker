//using UnityEngine;

//[CreateAssetMenu(fileName = "NewArticlePage", menuName = "Case System/Article Page")]
//public class ArticlePageData : ScriptableObject
//{
//    public string title;
//    [TextArea(15, 30)] public string bodyText;

//    [System.Serializable]
//    public class EmbeddedImage
//    {
//        public Sprite imageSprite;
//        [Tooltip("The text block index after which this image should appear.")]
//        public int insertAfterParagraphIndex;
//    }
//    public EmbeddedImage[] images;
//}

using System.Collections.Generic;
using UnityEngine;

public enum SectorType { LocalNews, PopCulture, Finance, HealthScience, BreakingNews }

[System.Serializable]
public class ArticleCheatSheet
{
    public SectorType correctSector;
    public bool isFlawed;
    public bool approveAndPublish; // true = Approve, false = Reject

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

//[CreateAssetMenu(fileName = "NewArticlePage", menuName = "Case System/Article Page")]
//public class ArticlePageData : ScriptableObject
//{
//    [Header("ID Configuration")]
//    [Tooltip("Format: DESK_DAY_NUMBER (e.g. LOC_D1_01)")]
//    public string articleID;

//    public string title;
//    [TextArea(15, 30)] public string bodyText;

//    [Header("Verification Cheat Sheet")]
//    public ArticleCheatSheet cheatSheet;

//    [System.Serializable]
//    public class EmbeddedImage
//    {
//        public Sprite imageSprite;
//        [Tooltip("The text block index after which this image should appear.")]
//        public int insertAfterParagraphIndex;
//    }
//    public EmbeddedImage[] images;
//}

// Keep all your original code at the top exactly the same...
[CreateAssetMenu(fileName = "NewArticlePage", menuName = "Case System/Article Page")]
public class ArticlePageData : ScriptableObject
{
    [HideInInspector] public bool helperWasUsedOnThisArticleState = false;

    [Header("ID Configuration")]
    public string articleID;
    public string title;
    //[TextArea(15, 30)] public string bodyText;
    [TextArea(5, 12)] public string bodyText;

    //[Header("ID Configuration")]
    //public string articleID;

    // --- ADD THIS NEW LINE RIGHT HERE ---
    [TextArea(2, 4)] public string headliner;

    //[TextArea(5, 12)] public string bodyText;

    // --- ADD THESE TWO NEW FIELDS HERE ---
    [Header("New Feed Additions")]
    [Tooltip("Check this if this specific post features an external website link simulation")]
    public bool hasExternalLink;

    [TextArea(3, 5)]
    [Tooltip("Add comments for this post here. Leave empty if this post has no comments.")]
    public List<string> customComments;
    // -------------------------------------

    [Header("Verification Cheat Sheet")]
    public ArticleCheatSheet cheatSheet;

    [HideInInspector] public bool playerLikedThisTemporaryState = false;
    [HideInInspector] public bool playerDislikedThisTemporaryState = false;


    // Leave the rest of your original EmbeddedImage arrays exactly as they were...

    //    [Header("ID Configuration")]
    //    [Tooltip("Format: DESK_DAY_NUMBER (e.g. LOC_D1_01)")]
    //    public string articleID;

    //    public string title;
    //    [TextArea(15, 30)] public string bodyText;

    //    [Header("Verification Cheat Sheet")]
    //    public ArticleCheatSheet cheatSheet;

    [System.Serializable]
    public class EmbeddedImage
    {
        public Sprite imageSprite;
        [Tooltip("The text block index after which this image should appear.")]
        public int insertAfterParagraphIndex;
    }
    public EmbeddedImage[] images;
}
