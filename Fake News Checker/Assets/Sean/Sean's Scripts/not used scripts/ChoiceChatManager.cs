using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ChoiceChatManager : MonoBehaviour
{
    [Header("UI Scroll Container")]
    public Transform chatContentContainer;  // The 'Content' object inside Center Scroll View
    public ScrollRect chatScrollRect;       // The Scroll View tracking component

    [Header("Choice Buttons")]
    public Button choiceButtonA;            // The 'Choice A' button on screen
    public Button choiceButtonB;            // The 'Choice B' button on screen
    public TMP_Text textChoiceA;            // TMP text inside Button A
    public TMP_Text textChoiceB;            // TMP text inside Button B

    [Header("Prefab Template")]
    public GameObject messagePrefab;        // Drop your 'ChatMessagePrefab' here

    private int conversationStage = 0;      // Tracks where you are in the conversation tree

    void Start()
    {
        // 1. Initialize Choice Button Text Strings
        SetUpChoices("Hello, anyone there?", "Log out of terminal.");

        // 2. Add listeners to check which choice the player picks
        if (choiceButtonA != null) choiceButtonA.onClick.AddListener(() => SelectChoice(true));
        if (choiceButtonB != null) choiceButtonB.onClick.AddListener(() => SelectChoice(false));
    }

    public void SetUpChoices(string optionA, string optionB)
    {
        if (textChoiceA != null) textChoiceA.text = optionA;
        if (textChoiceB != null) textChoiceB.text = optionB;

        // Re-enable choice selection buttons cleanly
        SetButtonsInteractable(true);
    }

    public void SelectChoice(bool choseA)
    {
        string playerMessage = choseA ? textChoiceA.text : textChoiceB.text;

        // 1. Print the player's choice on screen immediately
        SpawnMessageBubble($"<b>Player:</b> {playerMessage}");

        // 2. Shut off buttons while the recipient "types" an entry
        SetButtonsInteractable(false);

        // 3. Process the branching response tree logic
        StartCoroutine(ProcessConversationTree(choseA));
    }

    private IEnumerator ProcessConversationTree(bool choseA)
    {
        // Add a slight realistic typing delay (1.5 seconds)
        yield return new WaitForSeconds(1.5f);

        if (conversationStage == 0)
        {
            if (choseA)
            {
                SpawnMessageBubble("<b>Unknown:</b> Loud and clear. What's your status?");
                SetUpChoices("I am stuck in the control room.", "Everything is fine.");
                conversationStage = 1;
            }
            else
            {
                SpawnMessageBubble("<b>System:</b> Logout failed. Error code 403.");
                SetUpChoices("Try logging out again.", "Hello, anyone there?");
                // Stays at stage 0 to let them cycle back
            }
        }
        else if (conversationStage == 1)
        {
            if (choseA)
            {
                SpawnMessageBubble("<b>Unknown:</b> Hold tight. I'm searching for an override code.");
                HideChoiceButtons(); // End of conversation branch demo
            }
            else
            {
                SpawnMessageBubble("<b>Unknown:</b> Don't lie to me, I can see your terminal metrics spikes.");
                SetUpChoices("Fine, help me out.", "Leave me alone.");
                conversationStage = 2;
            }
        }
    }

    private void SpawnMessageBubble(string textContent)
    {
        if (messagePrefab == null || chatContentContainer == null) return;

        // Instantiate message asset directly into the vertical layout structure
        GameObject newMsg = Instantiate(messagePrefab, chatContentContainer);
        TMP_Text textComp = newMsg.GetComponentInChildren<TMP_Text>();

        if (textComp != null)
        {
            textComp.text = textContent;
        }

        // Snap vertical scroll view cleanly straight down to show new text
        Canvas.ForceUpdateCanvases();
        if (chatScrollRect != null)
        {
            chatScrollRect.verticalNormalizedPosition = 0f;
        }
    }

    private void SetButtonsInteractable(bool interactable)
    {
        if (choiceButtonA != null) choiceButtonA.interactable = interactable;
        if (choiceButtonB != null) choiceButtonB.interactable = interactable;
    }

    private void HideChoiceButtons()
    {
        if (choiceButtonA != null) choiceButtonA.gameObject.SetActive(false);
        if (choiceButtonB != null) choiceButtonB.gameObject.SetActive(false);
    }
}
