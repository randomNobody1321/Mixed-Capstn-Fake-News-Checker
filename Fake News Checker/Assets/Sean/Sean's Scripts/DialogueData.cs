using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue")]
public class DialogueData : ScriptableObject
{
    public string dialogueName;
    public List<DialogueNode> nodes = new List<DialogueNode>();
}

[Serializable]
public class DialogueNode
{
    public string speakerName = "NPC";
    [TextArea(2, 5)]
    public string speakerMessage;
    public List<DialogueChoice> choices = new List<DialogueChoice>();
}

[Serializable]
public class DialogueChoice
{
    public string choiceButtonText;          // What shows up on the option button selection row
    [TextArea(2, 5)]
    public string playerResponseBubbleText;  // What gets printed as a chat bubble when selected
    public int nextNodeIndex = -1;           // Index of the node to load next (-1 triggers End)
}

//{
//    public string dialogueName;

//    public List<DialogueNode> nodes = new List<DialogueNode>();
//}

//[Serializable]
//public class DialogueNode
//{
//    [TextArea(2, 5)]
//    public string speakerMessage;

//    public List<DialogueChoice> choices = new List<DialogueChoice>();
//}

//[Serializable]
//public class DialogueChoice
//{
//    public string choiceText;

//    [TextArea(2, 5)]
//    public string playerResponse;

//    public int nextNodeIndex = -1;
//}
