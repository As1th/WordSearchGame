using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private bool isDragging = false;
    private int activeTouchID = -1;
    private Vector2 startDragPosition;
    private Vector2 endDragPosition;

    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;

    public string selectedWord = ""; // Now part of the InputManager
    private const int maxTilesInDrag = 5; // Set maximum drag length to 5 tiles
    public GameManager gm;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        if (graphicRaycaster == null)
        {
            Debug.LogError("GraphicRaycaster is not assigned or not found on the Canvas or parent object!");
        }

        eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            Debug.LogError("EventSystem is missing from the scene. Please add one.");
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (activeTouchID == -1 || touch.fingerId == activeTouchID)
            {
                if (activeTouchID == -1)
                {
                    activeTouchID = touch.fingerId;
                    isDragging = true;
                    selectedWord = ""; // Reset the selected word at the start of dragging
                    startDragPosition = touch.position;
                    NotifyTileOfStartDrag(startDragPosition);
                }

                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    endDragPosition = touch.position;
                    NotifyTileOfDrag(endDragPosition);
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isDragging = false;
                    activeTouchID = -1;
                    NotifyTileOfEndDrag(endDragPosition);
                }
            }
        }
    }

    public void NotifyTileOfStartDrag(Vector2 startPosition)
    {
        List<RaycastResult> results = PerformUIRaycast(startPosition);
        if (results.Count > 0)
        {
            LetterTileController tile = results[0].gameObject.GetComponent<LetterTileController>();
            if (tile != null)
            {
                tile.OnDragStart();
                AppendToSelectedWord(tile); // Add the letter to selectedWord on drag start
            }
        }
    }

    public void NotifyTileOfDrag(Vector2 dragPosition)
    {
        if (LetterTileController.selectedTiles.Count >= maxTilesInDrag)
        {
            // Stop the drag if the max limit of tiles is reached
            return;
        }

        List<RaycastResult> results = PerformUIRaycast(dragPosition);
        if (results.Count > 0)
        {
            LetterTileController tile = results[0].gameObject.GetComponent<LetterTileController>();
            if (tile != null && !LetterTileController.selectedTiles.Contains(tile))
            {
                tile.OnDrag();
                AppendToSelectedWord(tile); // Add the letter to selectedWord while dragging
            }
        }
    }

    public void NotifyTileOfEndDrag(Vector2 endPosition)
    {
        List<RaycastResult> results = PerformUIRaycast(endPosition);
        if (results.Count > 0)
        {
            LetterTileController tile = results[0].gameObject.GetComponent<LetterTileController>();
            if (tile != null)
            {
                tile.OnDragEnd();
            }
        }
    }

    private List<RaycastResult> PerformUIRaycast(Vector2 screenPosition)
    {
        if (graphicRaycaster == null || eventSystem == null)
        {
            Debug.LogError("GraphicRaycaster or EventSystem is missing!");
            return new List<RaycastResult>(); // Return empty list if components are missing
        }

        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerData, results);

        return results;
    }

    private void AppendToSelectedWord(LetterTileController tile)
    {
        selectedWord += tile.gameObject.GetComponent<TextMeshProUGUI>().text; // Append the letter
        gm.selectedWordDisplay.text = selectedWord;
        gm.selectedWord = selectedWord;
        tile.OnDrag(); // Change the tile's color or any other visual effect
    }
}
