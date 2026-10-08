using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleChatRoom : MonoBehaviour
{
    [Header("Chat Display")]
    public TMP_Text chatText;

    [Header("Choice Buttons")]
    public Button choiceAButton;
    public Button choiceBButton;

    [Header("Choice Button Text")]
    public TMP_Text choiceAText;
    public TMP_Text choiceBText;

    private int conversationStep = 0;

    void Start()
    {
        // Start the conversation
        ShowConversationStart();

        choiceAButton.onClick.AddListener(ChooseA);
        choiceBButton.onClick.AddListener(ChooseB);
    }

    void ShowConversationStart()
    {
        chatText.text = "NPC: Hey, are you there?";

        choiceAText.text = "Yeah, I'm here.";
        choiceBText.text = "Who is this?";
    }

    void ChooseA()
    {
        if (conversationStep == 0)
        {
            chatText.text += "\n\nYOU: Yeah, I'm here.";

            chatText.text += "\n\nNPC: Good. I wasn't sure you'd answer.";

            choiceAText.text = "What happened?";
            choiceBText.text = "Never mind.";

            conversationStep = 1;
        }
        else if (conversationStep == 1)
        {
            chatText.text += "\n\nYOU: What happened?";

            chatText.text += "\n\nNPC: Something is wrong. I need you to listen.";

            choiceAText.text = "I'm listening.";
            choiceBText.text = "This sounds strange.";

            conversationStep = 2;
        }
        else if (conversationStep == 2)
        {
            chatText.text += "\n\nYOU: I'm listening.";

            chatText.text += "\n\nNPC: Good. Then we can finally talk.";

            choiceAButton.gameObject.SetActive(false);
            choiceBButton.gameObject.SetActive(false);
        }
    }

    void ChooseB()
    {
        if (conversationStep == 0)
        {
            chatText.text += "\n\nYOU: Who is this?";

            chatText.text += "\n\nNPC: You really don't recognize me?";

            choiceAText.text = "No, I don't.";
            choiceBText.text = "Wait... I remember.";

            conversationStep = 3;
        }
        else if (conversationStep == 1)
        {
            chatText.text += "\n\nYOU: Never mind.";

            chatText.text += "\n\nNPC: No. You need to hear this.";

            choiceAText.text = "Fine. Tell me.";
            choiceBText.text = "I'm leaving.";

            conversationStep = 4;
        }
        else if (conversationStep == 3)
        {
            chatText.text += "\n\nYOU: Wait... I remember.";

            chatText.text += "\n\nNPC: Then you know why I'm contacting you.";

            choiceAButton.gameObject.SetActive(false);
            choiceBButton.gameObject.SetActive(false);
        }
        else if (conversationStep == 4)
        {
            chatText.text += "\n\nYOU: I'm leaving.";

            chatText.text += "\n\nNPC: Wait! Don't go.";

            choiceAButton.gameObject.SetActive(false);
            choiceBButton.gameObject.SetActive(false);
        }
    }
}
