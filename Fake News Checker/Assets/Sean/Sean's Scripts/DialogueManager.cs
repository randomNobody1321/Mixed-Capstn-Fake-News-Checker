using UnityEngine;
using UnityEngine.UI;          // Fixes ScrollRect and Button errors
using System.Collections.Generic;
using TMPro;                   // Fixes TMP_Text errors


public class DialogueManager : MonoBehaviour
{
    //public ChatUI chatUI; // old dialogue system

    private DialogueData currentDialogue;
    private int currentNodeIndex;

    [Header("UI Output Scroll Container")]
    public Transform chatContentContainer;  // 'Content' GameObject inside Center Scroll View
    public ScrollRect chatScrollRect;       // Main Scroll View tracking component

    [Header("Choice Interaction Elements")]
    public GameObject choicePanelContainer; // Drag the bottom horizontal bar container here!
    public Button[] choiceButtons;          // Array matching your choice buttons on screen

    [Header("Prefab Template")]
    public GameObject messagePrefab;        // Your ChatMessagePrefab asset reference

    [Header("Chat Visual Customization")]
    public Sprite npcAvatarSprite;          // Drag your NPC headshot asset here
    public Sprite playerAvatarSprite;       // Drag your Player headshot asset here
    public Color npcBubbleColor = new Color(0.9f, 0.9f, 0.9f, 1f); // Modern gray bubble
    public Color playerBubbleColor = new Color(0.1f, 0.5f, 0.9f, 1f); // Modern blue bubble

    public void StartDialogue(DialogueData dialogue)
    {
        if (dialogue == null)
        {
            Debug.LogWarning("No dialogue assigned.");
            return;
        }

        currentDialogue = dialogue;
        currentNodeIndex = 0;

        //chatUI.ClearChat();

        // Clean out leftover historical prefabs from previous conversations cleanly
        foreach (Transform child in chatContentContainer)
        {
            Destroy(child.gameObject);
        }

        ShowCurrentNode();
    }

    private void ShowCurrentNode()
    {
        //if (currentDialogue == null)
        //    return;

        //if (currentNodeIndex < 0 ||
        //    currentNodeIndex >= currentDialogue.nodes.Count)
        //{
        //    EndDialogue();
        //    return;
        //}

        if (currentDialogue == null || currentNodeIndex < 0 || currentNodeIndex >= currentDialogue.nodes.Count)
        {
            EndDialogue();
            return;
        }

        DialogueNode node =
            currentDialogue.nodes[currentNodeIndex];

        // Spawn NPC Message Node Entry
        //SpawnMessageBubble($"<b>{node.speakerName}:</b> {node.speakerMessage}");
        SpawnMessageRow(node.speakerName, node.speakerMessage, false);

        // Refresh Choice Buttons Layout
        RenderChoices(node);

        //chatUI.ShowNode(node);
    }

    public void SelectChoice(int choiceIndex)
    {
        if (currentDialogue == null) return;
        DialogueNode node = currentDialogue.nodes[currentNodeIndex];

        if (choiceIndex < 0 || choiceIndex >= node.choices.Count) return;
        DialogueChoice choice = node.choices[choiceIndex];

        // Spawn Player Message Choice Bubble Entry
        //SpawnMessageBubble($"<b>You:</b> {choice.playerResponseBubbleText}");
        SpawnMessageRow("You", choice.playerResponseBubbleText, true);

        if (choice.nextNodeIndex == -1)
        {
            EndDialogue();
            return;
        }

        currentNodeIndex = choice.nextNodeIndex;
        ShowCurrentNode();
    }

    private void RenderChoices(DialogueNode node)
    {
        // Safety Clear: If there are no player responses left, turn off the panel completely
        if (node == null || node.choices == null || node.choices.Count == 0)
        {
            if (choicePanelContainer != null) choicePanelContainer.SetActive(false);
            foreach (Button btn in choiceButtons) btn.gameObject.SetActive(false);
            return;
        }

        // Otherwise, reveal our footer workstation workspace
        if (choicePanelContainer != null) choicePanelContainer.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].onClick.RemoveAllListeners();

            if (i < node.choices.Count)
            {
                int capturedIndex = i;
                DialogueChoice choice = node.choices[i];

                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].onClick.AddListener(() => SelectChoice(capturedIndex));

                TMP_Text buttonText = choiceButtons[i].GetComponentInChildren<TMP_Text>();
                if (buttonText != null) buttonText.text = choice.choiceButtonText;
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }
    private void SpawnMessageRow(string speakerName, string textContent, bool isPlayer)
    {
        if (messagePrefab == null || chatContentContainer == null) return;

        // 1. Instantiate the master layout row
        GameObject newMsgRow = Instantiate(messagePrefab, chatContentContainer);

        // 2. Locate our child layout containers safely by hierarchy path strings
        HorizontalLayoutGroup horizontalGroup = newMsgRow.GetComponentInChildren<HorizontalLayoutGroup>();
        Image avatarImage = null;
        Image bubbleImage = null;
        TMP_Text textComp = newMsgRow.GetComponentInChildren<TMP_Text>();

        if (horizontalGroup != null)
        {
            // Find children explicitly by their names to avoid layout engine confusion
            Transform avatarTransform = horizontalGroup.transform.Find("Avatar Icon");
            Transform bubbleTransform = horizontalGroup.transform.Find("Bubble Background");

            if (avatarTransform != null) avatarImage = avatarTransform.GetComponent<Image>();
            if (bubbleTransform != null) bubbleImage = bubbleTransform.GetComponent<Image>();
        }

        //// 3. Dynamically configure the text
        //if (textComp != null)
        //{
        //    textComp.text = $"<b>{speakerName}:</b> {textContent}";
        //    textComp.color = isPlayer ? Color.white : new Color(0.15f, 0.15f, 0.15f, 1f);
        //}

        // 3. Dynamically configure the text
        if (textComp != null)
        {
            textComp.text = $"<b>{speakerName}:</b> {textContent}";
            textComp.color = isPlayer ? Color.white : new Color(0.15f, 0.15f, 0.15f, 1f);

            // 1. Force the text mesh to wrap cleanly inside a strict width boundary lane
            textComp.textWrappingMode = TextWrappingModes.Normal;
            textComp.rectTransform.sizeDelta = new Vector2(180f, textComp.rectTransform.sizeDelta.y);

            // 2. Force the text mesh to calculate its true visual dimensions instantly
            textComp.ForceMeshUpdate();

            // 3. Manually map those exact text dimensions directly onto the bubble background container
            if (bubbleImage != null)
            {
                RectTransform bubbleRect = bubbleImage.GetComponent<RectTransform>();
                if (bubbleRect != null)
                {
                    // Calculate the required bubble size based on the text bounds, adding 24 pixels of padding
                    float packedWidth = Mathf.Min(textComp.renderedWidth + 24f, 200f);
                    float packedHeight = textComp.renderedHeight + 24f;

                    // Hard override: Apply the exact pixel dimensions to the bubble graphic
                    bubbleRect.sizeDelta = new Vector2(packedWidth, packedHeight);
                }
            }
        }




        // 4. Handle Visual Alignment, Avatars, and Bubble Colors dynamically
        if (horizontalGroup != null)
        {
            // HARD CLAMP THE BUBBLE GRAPHIC WIDTH SO IT CANNOT INFLATE
            if (bubbleImage != null)
            {
                LayoutElement bubbleLayout = bubbleImage.gameObject.GetComponent<LayoutElement>();
                if (bubbleLayout == null)
                {
                    bubbleLayout = bubbleImage.gameObject.AddComponent<LayoutElement>();
                }
                bubbleLayout.preferredWidth = 220f;
                bubbleLayout.flexibleWidth = 0f;
            }

            // UNIFIED ROW ALIGNMENT: Everyone is locked to UpperLeft to prevent monitor-wide stretching
            horizontalGroup.childAlignment = TextAnchor.UpperLeft;
            horizontalGroup.spacing = 10f; // Standard clean spacing between avatar and text bubble

            if (isPlayer)
            {
                if (avatarImage != null) avatarImage.sprite = playerAvatarSprite;
                if (bubbleImage != null) bubbleImage.color = playerBubbleColor;

                // NORMAL PLACEMENT: Keep the player avatar locked cleanly as the first child on the left
                if (avatarImage != null) avatarImage.transform.SetAsFirstSibling();

                // ENSURE NORMAL SCALE: Stop any mirror flips from altering layout tracking
                if (bubbleImage != null) bubbleImage.transform.localScale = Vector3.one;
                if (textComp != null) textComp.transform.localScale = Vector3.one;
            }
            else
            {
                if (avatarImage != null) avatarImage.sprite = npcAvatarSprite;
                if (bubbleImage != null) bubbleImage.color = npcBubbleColor;
                
                // NORMAL PLACEMENT: Keep the Guard avatar locked cleanly as the first child on the left
                if (avatarImage != null) avatarImage.transform.SetAsFirstSibling();

                if (bubbleImage != null) bubbleImage.transform.localScale = Vector3.one;
                if (textComp != null) textComp.transform.localScale = Vector3.one;
            }
        }
        // 5. Force UI refresh cycle to snap our scroll position down instantly
        Canvas.ForceUpdateCanvases();
        if (chatScrollRect != null) chatScrollRect.verticalNormalizedPosition = 0f;

        // HARD OVERRIDE: Force the spawned message block to stay small inside World Space
        RectTransform rowRect = newMsgRow.GetComponent<RectTransform>();
        if (rowRect != null)
        {
            rowRect.localScale = Vector3.one; // Forces Scale to (1, 1, 1)
            rowRect.sizeDelta = new Vector2(400f, rowRect.sizeDelta.y); // Locks width to a neat 400 pixels
        }

    }
    private void EndDialogue()
    {
        currentDialogue = null;
        if (choicePanelContainer != null) choicePanelContainer.SetActive(false);

        foreach (Button btn in choiceButtons) btn.gameObject.SetActive(false);

        //chatUI.HideChoices();
    }
}
