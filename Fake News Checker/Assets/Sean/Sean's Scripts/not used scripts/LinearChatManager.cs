using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LinearChatManager : MonoBehaviour
{
    [Header("UI Component Wireup")]
    public TMP_InputField chatInputField;   // Type box component
    public Button sendButton;               // Send submission button
    public Transform chatContentContainer;  // The 'Content' object inside your Scroll View
    public ScrollRect chatScrollRect;       // The Scroll View tracking component

    [Header("Prefab Template")]
    public GameObject messagePrefab;        // Drop your 'ChatMessagePrefab' here

    void Start()
    {
        if (sendButton != null)
        {
            sendButton.onClick.AddListener(SubmitMessage);
        }

        // Enable pressing "Enter" on your keyboard to instantly submit messages
        if (chatInputField != null)
        {
            chatInputField.onSubmit.AddListener(delegate { SubmitMessage(); });
        }
    }

    public void SubmitMessage()
    {
        // Don't send anything if the input box is completely blank
        if (chatInputField == null || string.IsNullOrWhiteSpace(chatInputField.text)) return;

        // 1. Spawn a clean message instance directly nested under your Scroll View Content
        GameObject newMsg = Instantiate(messagePrefab, chatContentContainer);

        // 2. Locate text nodes inside the instantiated prefab and update them
        TMP_Text[] textComponents = newMsg.GetComponentsInChildren<TMP_Text>();

        // Setup Assumption: Text component 0 is Sender Name, Component 1 is Message Body
        if (textComponents.Length >= 2)
        {
            textComponents[0].text = "USER"; // Alter dynamically if you track sender names
            textComponents[1].text = chatInputField.text;
        }
        else if (textComponents.Length == 1)
        {
            // Fallback: If you only have one single combined block
            textComponents[0].text = $"<b>USER:</b> {chatInputField.text}";
        }

        // 3. Wipe out text fields clean for the next typed entry
        chatInputField.text = "";
        chatInputField.ActivateInputField(); // Keep focus active so you don't have to re-click the input

        // 4. Force the Scroll View to slide down to showcase the fresh block
        Canvas.ForceUpdateCanvases(); // Mandatory: Forces Unity UI to compute the size change immediately
        ForceScrollToBottom();
    }

    private void ForceScrollToBottom()
    {
        if (chatScrollRect != null)
        {
            // Normalized Position 0f represents the bottom row of a vertical scroll area
            chatScrollRect.verticalNormalizedPosition = 0f;
        }
    }

}
