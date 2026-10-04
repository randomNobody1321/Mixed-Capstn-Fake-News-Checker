using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CenterDocumentViewer : MonoBehaviour
{
    [Header("Scroll Container")]
    public Transform articleContentContainer; // 'Content' under Center Scroll View (Vertical Layout Group)
    public ScrollRect articleScrollRect;

    [Header("Prefab Templates")]
    public GameObject textParagraphPrefab; // UI GameObject containing just a TextMeshPro component
    public GameObject imageEmbedPrefab;    // UI GameObject containing just an Image component

    public void DisplayArticle(ArticlePageData data)
    {
        if (data == null || articleContentContainer == null) return;

        // 1. Wipe out the previous document layout cleanly
        foreach (Transform child in articleContentContainer)
        {
            Destroy(child.gameObject);
        }

        // Split text body by paragraphs (double newlines)
        string[] paragraphs = data.bodyText.Split(new string[] { "\n\n", "\r\n\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        // 2. Loop through every paragraph line and weave images in elegantly
        for (int i = 0; i < paragraphs.Length; i++)
        {
            // Spawn Text
            GameObject txtObj = Instantiate(textParagraphPrefab, articleContentContainer);
            TMP_Text pText = txtObj.GetComponent<TMP_Text>();
            if (pText != null) pText.text = paragraphs[i];

            // Check if any image data packet matches this precise index step
            foreach (var imgData in data.images)
            {
                if (imgData.insertAfterParagraphIndex == i && imgData.imageSprite != null)
                {
                    // Spawn Image Box right below that block line
                    GameObject imgObj = Instantiate(imageEmbedPrefab, articleContentContainer);
                    Image imageWidget = imgObj.GetComponent<Image>();
                    if (imageWidget != null)
                    {
                        imageWidget.sprite = imgData.imageSprite;
                        imageWidget.preserveAspect = true;
                    }
                }
            }
        }

        // 3. Reset document scroll focus lane cleanly back to the roof line
        Canvas.ForceUpdateCanvases();
        if (articleScrollRect != null) articleScrollRect.verticalNormalizedPosition = 1f;
    }
}
