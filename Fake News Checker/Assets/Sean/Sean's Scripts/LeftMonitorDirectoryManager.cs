using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeftMonitorDirectoryManager : MonoBehaviour
{
    [Header("UI Containers")]
    public Transform mainTopicButtonContainer; // Horizontal or Vertical layout group for Topic/Case buttons
    public Transform dynamicDirectoryContainer; // Vertical layout group where Category Zones load

    [Header("Templates")]
    public GameObject directoryButtonPrefab; // Simple UI Button with a TMP text component child

    [Header("Center Screen Router")]
    public CenterDocumentViewer centerDocumentViewer; // Links directly to the center screen engine

    public void LoadCaseTopic(CaseTopicData caseData)
    {
        if (caseData == null) return;

        // 1. Wipe out any active buttons in the current left monitor directory panel
        foreach (Transform child in dynamicDirectoryContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. Generate Category Zones (A, B, C) recursively
        foreach (var node in caseData.directoryRoot)
        {
            SpawnDirectoryButton(node, dynamicDirectoryContainer, 0);
        }
    }

    private void SpawnDirectoryButton(CaseTopicData.SubMenuNode node, Transform parent, int indentLevel)
    {
        GameObject btnObj = Instantiate(directoryButtonPrefab, parent);
        Button btn = btnObj.GetComponent<Button>();
        TMP_Text txt = btnObj.GetComponentInChildren<TMP_Text>();

        if (txt != null)
        {
            // Indent text based on depth layer (e.g., Button C vs Button C-1-1)
            string indentation = new string(' ', indentLevel * 3);
            txt.text = $"{indentation}• {node.nodeName}";
        }

        // Create an empty panel right underneath this button to act as its hidden sub-menu box
        GameObject childPanel = new GameObject("SubMenuPanel", typeof(RectTransform), typeof(VerticalLayoutGroup));
        childPanel.transform.SetParent(parent, false);

        // Match settings to fit the parent container lane cleanly
        VerticalLayoutGroup vGroup = childPanel.GetComponent<VerticalLayoutGroup>();
        vGroup.childControlWidth = true; vGroup.childControlHeight = false;
        vGroup.childForceExpandWidth = false; vGroup.childForceExpandHeight = false;
        childPanel.SetActive(false);

        // Recursively build children buttons inside that hidden panel
        if (node.childNodes != null && node.childNodes.Count > 0)
        {
            foreach (var childNode in node.childNodes)
            {
                SpawnDirectoryButton(childNode, childPanel.transform, indentLevel + 1);
            }
        }

        btn.onClick.AddListener(() => {
            // Action 1: If it has sub-buttons, toggle their visibility frame (Accordion drop-down effect)
            if (node.childNodes != null && node.childNodes.Count > 0)
            {
                childPanel.SetActive(!childPanel.activeSelf);
                LayoutRebuilder.ForceRebuildLayoutImmediate(dynamicDirectoryContainer.GetComponent<RectTransform>());
            }

            // Action 2: If it has an article assigned, instantly stream it into the middle screen frame
            if (node.centerArticleToLoad != null && centerDocumentViewer != null)
            {
                centerDocumentViewer.DisplayArticle(node.centerArticleToLoad);
            }
        });
    }
}
