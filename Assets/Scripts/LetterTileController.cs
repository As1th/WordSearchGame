using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime;

public class LetterTileController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public static List<LetterTileController> selectedTiles = new List<LetterTileController>();
    private bool isDragging = false;
    private Vector2 startDragPosition;
    private Vector2 endDragPosition;
    public string selectedWord;
    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;
    public int selectedCount = 0; // Counter for selected tiles
    void Start()
    {
        // Get the GraphicRaycaster and EventSystem components
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Handle single click
        SelectTile();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        selectedCount = 0; // Counter for selected tiles = 1;
        startDragPosition = Input.mousePosition; // Store the start position
        SelectTile(); // Also select the initial tile on pointer down
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        selectedWord = "";
        // Optional: Keep selected letters or clear
        // selectedTiles.Clear(); // Uncomment this if you want to clear on release
    }

    private void Update()
    {
        if (isDragging && Input.GetMouseButton(0))
        {
            endDragPosition = Input.mousePosition;

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

    private void SelectTiles(List<RaycastResult> results)
    {


        foreach (var result in results)
        {
            LetterTileController tile = result.gameObject.GetComponent<LetterTileController>();
            if (tile != null && !selectedTiles.Contains(tile))
            {
                // Select the tile and increase the count
                tile.SelectTile();
                selectedCount++;

                // Add the tile to selected tiles
                selectedTiles.Add(tile);

                selectedWord += tile.gameObject.GetComponent<TextMeshProUGUI>().text;
                print(selectedWord);
                // Check if the selected count has reached the limit
                if (selectedCount >= 4)
                {
                    isDragging = false;
                    break; // Stop selecting tiles if limit reached
                }
            }
        }
    }

    public void SelectTile()
    {
        if (!selectedTiles.Contains(this))
        {
            selectedTiles.Add(this);
            // Highlight the selected tile (you can change color or material)
            GetComponent<TextMeshProUGUI>().color = Color.yellow; // Example highlight color
            selectedWord += this.gameObject.GetComponent<TextMeshProUGUI>().text;


        }
    }
}
