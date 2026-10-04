using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCaseTopic", menuName = "Case System/Case Topic")]
public class CaseTopicData : ScriptableObject
{
    public string topicName; // e.g., "Case 01: Manila Dog Incident"

    [System.Serializable]
    public class SubMenuNode
    {
        public string nodeName; // e.g., "Articles Pending", "Puppy Play Date", "About Us"
        public ArticlePageData centerArticleToLoad; // If assigned, clicking this opens the article
        public List<SubMenuNode> childNodes; // Nested sub-buttons
    }

    public List<SubMenuNode> directoryRoot; // Holds Zone A, Zone B, Zone C
}
