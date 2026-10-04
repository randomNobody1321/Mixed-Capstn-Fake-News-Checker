using UnityEngine;

public class DirectoryTestHook : MonoBehaviour
{
    [Header("Manager Reference")]
    public LeftMonitorDirectoryManager directoryManager;

    [Header("Data Asset File")]
    public CaseTopicData testCaseData; // Drag your 'Case_01_ManilaDog' asset here!

    void Start()
    {
        if (directoryManager != null && testCaseData != null)
        {
            // Instantly compile the directory tree right on startup!
            directoryManager.LoadCaseTopic(testCaseData);
        }
    }
}
