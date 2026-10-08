using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatUI : MonoBehaviour
{
    [Header("Chat")]
    public TMP_Text chatText;

    [Header("Choice Buttons")]
    public Button[] choiceButtons;

    private DialogueManager dialogueManager;

    void Awake()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int buttonIndex = i;

            choiceButtons[i].onClick.AddListener(() =>
            {
                OnChoiceClicked(buttonIndex);
            });
        }
    }

    public void ShowNode(DialogueNode node)
    {
        if (node == null)
            return;

        AddNPCMessage(node.speakerMessage);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < node.choices.Count)
            {
                DialogueChoice choice = node.choices[i];

                choiceButtons[i].gameObject.SetActive(true);

                TMP_Text text =
                    choiceButtons[i].GetComponentInChildren<TMP_Text>();

                if (text != null)
                    //    text.text = choice.choiceText;
                    text.text = choice.choiceButtonText;
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnChoiceClicked(int index)
    {
        dialogueManager.SelectChoice(index);
    }

    public void AddNPCMessage(string message)
    {
        if (chatText.text == "")
            chatText.text = "NPC: " + message;
        else
            chatText.text += "\n\nNPC: " + message;
    }

    public void AddPlayerMessage(string message)
    {
        chatText.text += "\n\nYOU: " + message;
    }

    public void ClearChat()
    {
        chatText.text = "";
    }

    public void HideChoices()
    {
        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
    }
}
