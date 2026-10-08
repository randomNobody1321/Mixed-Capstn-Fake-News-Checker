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

[CreateAssetMenu(fileName = "NewArticlePage", menuName = "Case System/Article Page")]
public class ArticlePageData : ScriptableObject
{
    [Header("ID Configuration")]
    [Tooltip("Format: DESK_DAY_NUMBER (e.g. LOC_D1_01)")]
    public string articleID;

    public string title;
    [TextArea(15, 30)] public string bodyText;

    [Header("Verification Cheat Sheet")]
    public ArticleCheatSheet cheatSheet;

    [System.Serializable]
    public class EmbeddedImage
    {
        public Sprite imageSprite;
        [Tooltip("The text block index after which this image should appear.")]
        public int insertAfterParagraphIndex;
    }
    public EmbeddedImage[] images;
}

