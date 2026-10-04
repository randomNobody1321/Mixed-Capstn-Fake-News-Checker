using UnityEngine;
using UnityEngine.UI;

public class LeftMonitorTabManager : MonoBehaviour
{
    [System.Serializable]
    public class TabWindowPair
    {
        public Button tabButton;       // The sidebar document button
        public GameObject popUpWindow; // The matching central window pop-up
        public Button closeButton;     // The "X" button inside that pop-up

        public ScrollRect associatedScrollRect; // OPTIONAL: Drag the Scroll View here to reset scroll position on click

        public DialogueData dialogueToLoad;     // OPTIONAL: Dialogue Asset to load if this is a chatroom link

    }

    [Header("Tab & Window Configuration")]
    public TabWindowPair[] tabs;

    private GameObject activeWindow = null;

    void Start()
    {
        foreach (var pair in tabs)
        {
            if (pair.tabButton != null && pair.popUpWindow != null)
            {
                // Watch for when a left sidebar tab is clicked
                //pair.tabButton.onClick.AddListener(() => OpenWindow(pair.popUpWindow, pair.associatedScrollRect));
                pair.tabButton.onClick.AddListener(() => OpenWindow(pair.popUpWindow, pair.associatedScrollRect, pair.dialogueToLoad));

            }

            if (pair.closeButton != null && pair.popUpWindow != null)
            {
                // Watch for when the X button on the center monitor is clicked
                pair.closeButton.onClick.AddListener(() => CloseActiveWindow());
            }

            if (pair.popUpWindow != null)
            {
                pair.popUpWindow.SetActive(false);
            }
        }
    }

    public void OpenWindow(GameObject targetWindow, ScrollRect targetScrollRect, DialogueData dialogueData)
    {
        if (activeWindow == targetWindow) return;

        CloseActiveWindow();

        if (targetWindow != null)
        {
            targetWindow.SetActive(true);
            activeWindow = targetWindow;

            if (targetScrollRect != null) targetScrollRect.verticalNormalizedPosition = 1f;

            // Automatically check if the window contains an operational Dialogue Engine
            DialogueManager dialogueEngine = targetWindow.GetComponent<DialogueManager>();
            if (dialogueEngine != null && dialogueData != null)
            {
                dialogueEngine.StartDialogue(dialogueData);
            }
        }
        //if (targetWindow != null)
        //{
        //    targetWindow.SetActive(true);
        //    activeWindow = targetWindow;

        //    // Automatically scroll back to the top of the text/list when opened
        //    if (targetScrollRect != null)
        //    {
        //        targetScrollRect.verticalNormalizedPosition = 1f;
        //    }
        //}
    }

    public void CloseActiveWindow()
    {
        if (activeWindow != null)
        {
            activeWindow.SetActive(false);
            activeWindow = null;
        }
    }
}
