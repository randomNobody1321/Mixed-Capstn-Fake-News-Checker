using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextHighlightSelector : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text textComponent;
    private bool isHighlighted = false;
    private string runtimeOriginalText;

    void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        if (textComponent != null) runtimeOriginalText = textComponent.text;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (textComponent == null) return;

        // If clicking inside the text container but not targeting highlighted words, clear state
        if (isHighlighted)
        {
            RemoveHighlight();
            return;
        }

        int wordIndex = TMP_TextUtilities.FindIntersectingWord(textComponent, eventData.position, eventData.pressEventCamera);
        if (wordIndex != -1)
        {
            string cleanWord = textComponent.textInfo.wordInfo[wordIndex].GetWord();

            // To prevent accidental system conflicts, toggle highlight states
            isHighlighted = true;
            textComponent.text = runtimeOriginalText.Replace(cleanWord, $"<mark=#FFFF00AA><color=black>{cleanWord}</color></mark>");
        }
    }

    public void RemoveHighlight()
    {
        if (textComponent == null) return;
        isHighlighted = false;
        textComponent.text = runtimeOriginalText;
    }
}
