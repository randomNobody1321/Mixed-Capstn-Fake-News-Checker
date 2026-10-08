using UnityEngine;
using UnityEngine.EventSystems;

public class CenterScreenClickPass : MonoBehaviour, IPointerClickHandler
{
    public Transform articleContentContainer;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (articleContentContainer == null) return;

        // Clean out highlight tracking marks across text paragraphs
        foreach (Transform child in articleContentContainer)
        {
            TextHighlightSelector selector = child.GetComponent<TextHighlightSelector>();
            if (selector != null) selector.RemoveHighlight();
        }
    }
}
