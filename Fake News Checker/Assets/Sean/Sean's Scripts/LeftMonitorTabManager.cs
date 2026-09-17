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
    }

    [Header("Tab & Window Configuration")]
    public TabWindowPair[] tabs;

    private GameObject activeWindow = null;

    void Start()
    {
        // Set up button listeners automatically
        foreach (var pair in tabs)
        {
            if (pair.tabButton != null && pair.popUpWindow != null)
            {
                // Watch for when a sidebar tab is clicked
                pair.tabButton.onClick.AddListener(() => OpenWindow(pair.popUpWindow));
            }

            if (pair.closeButton != null && pair.popUpWindow != null)
            {
                // Watch for when the X button is clicked
                pair.closeButton.onClick.AddListener(() => CloseActiveWindow());
            }

            // Double check everything starts cleanly hidden
            if (pair.popUpWindow != null)
            {
                pair.popUpWindow.SetActive(false);
            }
        }
    }

    public void OpenWindow(GameObject targetWindow)
    {
        // If the clicked window is already open, do nothing
        if (activeWindow == targetWindow) return;

        // Rule: Safely close the previously opened tab window first
        CloseActiveWindow();

        // Rule: Open the tab where the assigned button points to
        if (targetWindow != null)
        {
            targetWindow.SetActive(true);
            activeWindow = targetWindow;
        }
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
