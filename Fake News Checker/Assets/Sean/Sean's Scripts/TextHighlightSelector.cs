using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class TextHighlightSelector : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text textComponent;
    private string[] originalWords;
    private HashSet<int> highlightedWordIndices = new HashSet<int>();

    void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    // Add this helper method so CenterScreenClickPass can call it without errors!
    public void RemoveHighlight()
    {
        RemoveAllHighlights();
    }


    void Start()
    {
        if (textComponent != null)
        {
            // Force a clean generation pass to capture baseline text parameters
            textComponent.ForceMeshUpdate();

            // Split the raw text string into a clean, permanent word registry
            string cleanText = textComponent.text;
            originalWords = cleanText.Split(new char[] { ' ', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (textComponent == null || originalWords == null || originalWords.Length == 0) return;

        // Sync mesh data before calculating screen pixel overlap coordinates
        textComponent.ForceMeshUpdate();

        // 2. Fix the offset: Convert screen pixels accurately into local text box coordinates
        RectTransform rectTransform = textComponent.rectTransform;

        // --- CHANGED THIS LINE FROM Vector3 TO Vector2 TO FIX COMPILER ERROR CS1503 ---
        Vector2 localPoint;
        // -----------------------------------------------------------------------------

        // This translates the mouse position perfectly, matching any screen scale or parent layout constraints
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
        {

            // 3. Track intersecting character nodes directly via local canvas position coordinates
            int wordIndex = TMP_TextUtilities.FindIntersectingWord(textComponent, eventData.position, eventData.pressEventCamera);

            if (wordIndex != -1 && wordIndex < originalWords.Length)
            {
                if (highlightedWordIndices.Contains(wordIndex))
                {
                    highlightedWordIndices.Remove(wordIndex);
                }
                else
                {
                    highlightedWordIndices.Add(wordIndex);
                }

                RebuildHighlightedText();
            }
        }

        //int wordIndex = TMP_TextUtilities.FindIntersectingWord(textComponent, eventData.position, eventData.pressEventCamera);

        //if (wordIndex != -1 && wordIndex < originalWords.Length)
        //{
        //    if (highlightedWordIndices.Contains(wordIndex))
        //    {
        //        highlightedWordIndices.Remove(wordIndex);
        //    }
        //    else
        //    {
        //        highlightedWordIndices.Add(wordIndex);
        //    }

        //    RebuildHighlightedText();
        //}
    }

    void RebuildHighlightedText()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int i = 0; i < originalWords.Length; i++)
        {
            if (highlightedWordIndices.Contains(i))
            {
                // Wrap the original clean word without corrupting baseline string tracking lengths
                sb.Append($"<mark=#FFFF00AA><color=black>{originalWords[i]}</color></mark> ");
            }
            else
            {
                sb.Append(originalWords[i] + " ");
            }
        }

        textComponent.text = sb.ToString().TrimEnd();
    }

    public void RemoveAllHighlights()
    {
        if (textComponent == null || originalWords == null) return;
        highlightedWordIndices.Clear();
        textComponent.text = string.Join(" ", originalWords);
    }
}


//using UnityEngine;
//using UnityEngine.EventSystems;
//using TMPro;
//using System.Collections.Generic;

//public class TextHighlightSelector : MonoBehaviour, IPointerClickHandler
//{
//    private TMP_Text textComponent;
//    //private bool isHighlighted = false;
//    //private string runtimeOriginalText;

//    private string baseCleanText;
//    private HashSet<int> highlightedWordIndices = new HashSet<int>();

//    //void Awake()
//    //{
//    //    textComponent = GetComponent<TMP_Text>();
//    //    if (textComponent != null) runtimeOriginalText = textComponent.text;
//    //}

//    void Awake()
//    {
//        textComponent = GetComponent<TMP_Text>();
//        if (textComponent != null) baseCleanText = textComponent.text;
//    }

//    public void OnPointerClick(PointerEventData eventData)
//    {
//        if (textComponent == null) return;

//        // --- ADD THIS SAFETY LINE TO GENERATE MESH DATA BEFORE DETECTING CLICKS ---
//        textComponent.ForceMeshUpdate();
//        // --------------------------------------------------------------------------

//        if (textComponent.textInfo == null || textComponent.textInfo.wordInfo == null) return;

//        int wordIndex = TMP_TextUtilities.FindIntersectingWord(textComponent, eventData.position, eventData.pressEventCamera);

//        // Safety check to ensure the clicked index is actually inside our text block array length
//        if (wordIndex != -1 && wordIndex < textComponent.textInfo.wordInfo.Length)
//        {
//            if (highlightedWordIndices.Contains(wordIndex))
//            {
//                highlightedWordIndices.Remove(wordIndex);
//            }
//            else
//            {
//                highlightedWordIndices.Add(wordIndex);
//            }

//            RebuildHighlightedText();
//        }



//    //public void OnPointerClick(PointerEventData eventData)
//    //{
//    //    if (textComponent == null) return;

//    //    // Locate exactly what word was clicked
//    //    int wordIndex = TMP_TextUtilities.FindIntersectingWord(textComponent, eventData.position, eventData.pressEventCamera);

//    //    if (wordIndex != -1)
//    //    {
//    //        // If it's already highlighted, remove it from our collection. Otherwise, add it!
//    //        if (highlightedWordIndices.Contains(wordIndex))
//    //        {
//    //            highlightedWordIndices.Remove(wordIndex);
//    //        }
//    //        else
//    //        {
//    //            highlightedWordIndices.Add(wordIndex);
//    //        }

//    //        RebuildHighlightedText();
//    //    }


//        // If clicking inside the text container but not targeting highlighted words, clear state
//        //if (isHighlighted)
//        //{
//        //    RemoveHighlight();
//        //    return;
//        //}

//        //int wordIndex = TMP_TextUtilities.FindIntersectingWord(textComponent, eventData.position, eventData.pressEventCamera);
//        //if (wordIndex != -1)
//        //{
//        //    string cleanWord = textComponent.textInfo.wordInfo[wordIndex].GetWord();

//        //    // To prevent accidental system conflicts, toggle highlight states
//        //    isHighlighted = true;
//        //    textComponent.text = runtimeOriginalText.Replace(cleanWord, $"<mark=#FFFF00AA><color=black>{cleanWord}</color></mark>");
//        //}
//    }

//    void RebuildHighlightedText()
//    {
//        // Reconstruct the text block applying Rich Text mark styling tags strictly around selected indices
//        string renderedResult = "";
//        for (int i = 0; i < textComponent.textInfo.wordInfo.Length; i++)
//        {
//            string word = textComponent.textInfo.wordInfo[i].GetWord();

//            if (highlightedWordIndices.Contains(i))
//            {
//                renderedResult += $"<mark=#FFFF00AA><color=black>{word}</color></mark> ";
//            }
//            else
//            {
//                renderedResult += word + " ";
//            }
//        }
//        textComponent.text = renderedResult.TrimEnd();
//    }



//    public void RemoveHighlight()
//    {
//        if (textComponent == null) return;
//        highlightedWordIndices.Clear();
//        textComponent.text = baseCleanText;
//        //isHighlighted = false;
//        //textComponent.text = runtimeOriginalText;
//    }
//}
