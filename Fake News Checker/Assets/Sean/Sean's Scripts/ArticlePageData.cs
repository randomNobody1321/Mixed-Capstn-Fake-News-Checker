using UnityEngine;

[CreateAssetMenu(fileName = "NewArticlePage", menuName = "Case System/Article Page")]
public class ArticlePageData : ScriptableObject
{
    public string title;
    [TextArea(15, 30)] public string bodyText;

    [System.Serializable]
    public class EmbeddedImage
    {
        public Sprite imageSprite;
        [Tooltip("The text block index after which this image should appear.")]
        public int insertAfterParagraphIndex;
    }
    public EmbeddedImage[] images;
}
