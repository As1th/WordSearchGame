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

    public  List<LetterTileController> currentlySelectedTiles = new List<LetterTileController>(); // Track tiles in the current drag

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
                    currentlySelectedTiles.Clear(); // Clear the currently selected tiles
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
                    bool check = gm.checkWord();
                    if (check)
                    {
                        gm.correctGuesses++;
                        if(gm.correctGuesses == gm.difficulty+2)
                        {
                            gm.NudgeNewRound();
                        }
                    }
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
            if (results[0].gameObject != null)
            {
                LetterTileController tile = results[0].gameObject.GetComponent<LetterTileController>();
                if (tile != null)
                {
                    tile.OnDragStart();
                    AppendToSelectedWord(tile); // Add the letter to selectedWord on drag start
                }
            }
        }
    }

    public void NotifyTileOfDrag(Vector2 dragPosition)
    {
        if (currentlySelectedTiles.Count >= maxTilesInDrag)
        {
            // Stop the drag if the max limit of tiles is reached
            return;
        }

        List<RaycastResult> results = PerformUIRaycast(dragPosition);
        if (results.Count > 0)
        {
            if (results[0].gameObject != null)
            {
                LetterTileController tile = results[0].gameObject.GetComponent<LetterTileController>();
                if (tile != null && !currentlySelectedTiles.Contains(tile)) // Check against the current selection
                {
                    tile.OnDrag();
                    AppendToSelectedWord(tile); // Add the letter to selectedWord while dragging
                }
            }
        }
    }

    public void NotifyTileOfEndDrag(Vector2 endPosition)
    {
        foreach (var tile in currentlySelectedTiles)
        {
            tile.OnDragEnd(); // Call OnDragEnd for each selected tile
        }

        // Clear the currently selected tiles
        currentlySelectedTiles.Clear();
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
        currentlySelectedTiles.Add(tile); // Track the tile for the current drag
        selectedWord += tile.gameObject.GetComponent<TextMeshProUGUI>().text; // Append the letter
        gm.selectedWordDisplay.text = selectedWord;
        gm.selectedWord = selectedWord;
        tile.OnDrag(); // Change the tile's color or any other visual effect
    }
}
