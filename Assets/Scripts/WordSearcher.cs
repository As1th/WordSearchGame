using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordSearcher : MonoBehaviour
{
    private Vector2Int startPos;
    private List<GameObject> selectedLetters = new List<GameObject>();
    private bool isDragging = false;
    public GameManager gameManager;
    // Store a reference to the letter grid (set this when generating the grid)
    public Dictionary<GameObject, Vector2Int> letterGridPositions;


    void Awake()
    {
        // Initialize the dictionary in case it's not done elsewhere
        if (letterGridPositions == null)
        {
            letterGridPositions = new Dictionary<GameObject, Vector2Int>();
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Convert mouse position to grid position
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null)
            {
                startPos = GetGridPositionFromLetter(hit.collider.gameObject);
                selectedLetters.Clear();
                selectedLetters.Add(hit.collider.gameObject);
                isDragging = true; // Start dragging
            }
            else { print("nope"); 
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            // Detect dragging and add letters to the selection
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null && !selectedLetters.Contains(hit.collider.gameObject))
            {
                selectedLetters.Add(hit.collider.gameObject);
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            CheckSelectedWord();
            isDragging = false; // End dragging
        }
    }

    void CheckSelectedWord()
    {
        string selectedWord = "";
        foreach (GameObject letterObj in selectedLetters)
        {
            selectedWord += letterObj.GetComponent<TextMeshProUGUI>().text;
        }

        if (IsWordInList(selectedWord))
        {
            HighlightWord(selectedLetters);
        }
        else
        {
            // Clear selection if the word isn't valid
            selectedLetters.Clear();
        }
    }

    bool IsWordInList(string word)
    {
        return gameManager.currentTheme.Contains(word); // Assuming GameManager holds the current theme and its words
    }

    void HighlightWord(List<GameObject> letters)
    {
        Color highlightColor = Random.ColorHSV(); // Different color for each word
        foreach (GameObject letterObj in letters)
        {
            letterObj.GetComponent<TextMeshProUGUI>().color = highlightColor;
        }
    }

    Vector2Int GetGridPositionFromLetter(GameObject letter)
    {
        if (letterGridPositions.ContainsKey(letter))
        {
            return letterGridPositions[letter]; // Get the grid position from the dictionary
        }
        else
        {
            Debug.LogError("Letter position not found in grid.");
            return Vector2Int.zero; // Return a default value in case of error
        }
    }
}
