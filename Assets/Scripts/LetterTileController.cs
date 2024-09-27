using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class LetterTileController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static List<LetterTileController> selectedTiles = new List<LetterTileController>();
    private bool isDragging = false;
    private Vector2 startDragPosition;
    private Vector2 endDragPosition;
    public string selectedWord;
    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;
    public GameManager gm;
    public int selectedCount = 0; // Counter for selected tiles

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        selectedCount = 0; // Reset the selected tile count
        startDragPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : Vector2.zero; // Store start position from touch input
        SelectTile(); // Select the initial tile on pointer down
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        selectedWord = "";

        bool check = gm.checkWord();
        if (check)
        {
            // Handle word found logic
        }
        else
        {
            // Deselect and reset tiles if the word is incorrect
            foreach (LetterTileController letter in selectedTiles)
            {
                letter.GetComponent<TextMeshProUGUI>().color = Color.white;
            }
            selectedTiles.Clear();
        }
    }

    private void Update()
    {
        if (isDragging && Input.touchCount > 0) // Check if dragging and touch is active
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                endDragPosition = touch.position;

                // Use PointerEventData to raycast and select tiles
                PointerEventData pointerData = new PointerEventData(eventSystem)
                {
                    position = endDragPosition
                };

                List<RaycastResult> results = new List<RaycastResult>();
                graphicRaycaster.Raycast(pointerData, results);

                // Select tiles based on the drag
                SelectTiles(results);
            }
        }
    }

    private void SelectTiles(List<RaycastResult> results)
    {
        foreach (var result in results)
        {
            LetterTileController tile = result.gameObject.GetComponent<LetterTileController>();
            if (tile != null && !selectedTiles.Contains(tile))
            {
                tile.SelectTile();
                selectedCount++;

                // Add the tile to selected tiles
                selectedTiles.Add(tile);

                selectedWord += tile.gameObject.GetComponent<TextMeshProUGUI>().text;
                gm.selectedWord = selectedWord;
                gm.selectedWordDisplay.text = selectedWord;

                // Limit the selection to a certain count
                if (selectedCount >= 4)
                {
                    isDragging = false;
                    break;
                }
            }
        }
    }

    public void SelectTile()
    {
        if (!selectedTiles.Contains(this))
        {
            selectedTiles.Add(this);
            GetComponent<TextMeshProUGUI>().color = Color.yellow; // Highlight selected tile
            selectedWord += this.gameObject.GetComponent<TextMeshProUGUI>().text;
            gm.selectedWord = selectedWord;
            gm.selectedWordDisplay.text = selectedWord;
        }
    }
}
