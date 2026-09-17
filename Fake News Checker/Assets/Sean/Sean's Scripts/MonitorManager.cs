using UnityEngine;
using UnityEngine.InputSystem; // Added for the New Input System
using TMPro;

public class MonitorManager : MonoBehaviour
{
    [Header("Camera & Zoom Settings")]
    public Camera mainCamera;
    public float defaultOrthographicSize = 7f; // Zoom size out (sees everything)
    public float zoomedOrthographicSize = 3f;    // Zoom size in (close up)
    public float zoomSpeed = 5f;

    [Header("Monitors and Positions")]
    public Transform monitorLeft;
    public Transform monitorCenter;
    public Transform monitorRight;

    [Header("UI System")]
    public TMP_InputField notesInputField;

    private Vector3 targetCameraPosition;
    private float targetCameraSize;
    private Vector3 startingCameraPosition;
    private bool isZoomedIn = false;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        // Remember where the camera starts so we can always go back
        startingCameraPosition = mainCamera.transform.position;
        targetCameraPosition = startingCameraPosition;
        targetCameraSize = defaultOrthographicSize;

        // Turn off notes typing when zoomed out
        if (notesInputField != null) notesInputField.interactable = false;
    }

    void Update()
    {
        // Smoothly slide and zoom the camera over time
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, Time.deltaTime * zoomSpeed);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetCameraSize, Time.deltaTime * zoomSpeed);

        // New Input System: Check for Left Mouse Click
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleMouseClick();
        }

        // New Input System: Press Escape or Right-Click to zoom back out
        if (isZoomedIn)
        {
            bool escPressed = Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
            bool rightClicked = Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame;

            if (escPressed || rightClicked)
            {
                ZoomOut();
            }
        }
    }

    void HandleMouseClick()
    {
        // Don't register monitor clicks if the player is actively typing in the notepad
        if (notesInputField != null && notesInputField.isFocused) return;

        // New Input System: Get mouse position safely
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 worldMousePos = mainCamera.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldMousePos, Vector2.zero);

        if (hit.collider != null)
        {
            Transform clickedObject = hit.transform;

            if (clickedObject == monitorLeft) ZoomIn(monitorLeft.position);
            else if (clickedObject == monitorCenter) ZoomIn(monitorCenter.position);
            else if (clickedObject == monitorRight) ZoomIn(monitorRight.position);
        }
    }

    void ZoomIn(Vector3 monitorPosition)
    {
        targetCameraPosition = new Vector3(monitorPosition.x, monitorPosition.y, startingCameraPosition.z);
        targetCameraSize = zoomedOrthographicSize;
        isZoomedIn = true;

        // Enable typing if zooming into the right monitor
        if (monitorPosition == monitorRight.position && notesInputField != null)
        {
            notesInputField.interactable = true;
        }
    }

    void ZoomOut()
    {
        targetCameraPosition = startingCameraPosition;
        targetCameraSize = defaultOrthographicSize;
        isZoomedIn = false;

        // Disable typing and clear focus when zoomed out
        if (notesInputField != null)
        {
            notesInputField.DeactivateInputField();
            notesInputField.interactable = false;
        }
    }
}
