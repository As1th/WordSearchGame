using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int gridSize = 10; // Define your grid size
    public GameObject letterPrefab; // The letter GameObject prefab (with TextMeshPro)
    public Transform gridParent; // Parent to contain the letter objects
    public float cellSize = 30f; // The size of each cell in the grid
    private char[,] letterGrid;
    public WordSearcher wordSearcher;
    public void GenerateGrid(List<string> words)
    {
        letterGrid = new char[gridSize, gridSize];

        // Initialize grid with random letters (fill grid)
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                letterGrid[i, j] = '\0'; // Initialize with null character
            }
        }

        // Insert words into the grid
        foreach (string word in words)
        {
            PlaceWordInGrid(word.ToUpper());
        }

        // Fill remaining empty cells with random letters
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (letterGrid[i, j] == '\0') // If cell is still empty
                {
                    letterGrid[i, j] = (char)('A' + Random.Range(0, 26));
                }
            }
        }

        // Instantiate letter objects with proper positioning
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject letterObj = Instantiate(letterPrefab, gridParent);

                // Set the text of the letter
                letterObj.GetComponent<TextMeshProUGUI>().text = letterGrid[i, j].ToString();

                // Position the letter in the grid
                Vector3 position = new Vector3(i * cellSize, -j * cellSize, 0);  // Grid positioning
                letterObj.transform.localPosition = position;
                // Add the letter's position to the dictionary
               wordSearcher.letterGridPositions.Add(letterObj, new Vector2Int(i, j));
            }
        }
    }
    void PlaceWordInGrid(string word)
    {
        int maxAttempts = 100; // Limit number of attempts to place the word
        int attempt = 0;       // Keep track of attempts
        bool wordPlaced = false;

        while (!wordPlaced && attempt < maxAttempts)
        {
            attempt++; // Increment the attempt counter

            // Randomly choose a starting point
            int startRow = Random.Range(0, gridSize);
            int startCol = Random.Range(0, gridSize);

            // Randomly choose a direction: 0 = horizontal, 1 = vertical, 2 = diagonal
            int direction = Random.Range(0, 3);

            // Calculate ending points based on the direction
            int endRow = startRow;
            int endCol = startCol;

            switch (direction)
            {
                case 0: // Horizontal (left to right)
                    endCol = startCol + word.Length - 1;
                    break;
                case 1: // Vertical (top to bottom)
                    endRow = startRow + word.Length - 1;
                    break;
                case 2: // Diagonal (top-left to bottom-right)
                    endRow = startRow + word.Length - 1;
                    endCol = startCol + word.Length - 1;
                    break;
            }

            // Check if the word fits in the grid
            if (endRow < gridSize && endCol < gridSize)
            {
                // Check if the cells are empty or contain matching letters
                bool canPlaceWord = true;
                for (int i = 0; i < word.Length; i++)
                {
                    int row = startRow;
                    int col = startCol;

                    switch (direction)
                    {
                        case 0: // Horizontal
                            col = startCol + i;
                            break;
                        case 1: // Vertical
                            row = startRow + i;
                            break;
                        case 2: // Diagonal
                            row = startRow + i;
                            col = startCol + i;
                            break;
                    }

                    // Check if the cell is already occupied by a different letter
                    if (letterGrid[row, col] != '\0' && letterGrid[row, col] != word[i])
                    {
                        canPlaceWord = false;
                        break;
                    }
                }

                // If the word can be placed, assign letters to the grid
                if (canPlaceWord)
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        int row = startRow;
                        int col = startCol;

                        switch (direction)
                        {
                            case 0: // Horizontal
                                col = startCol + i;
                                break;
                            case 1: // Vertical
                                row = startRow + i;
                                break;
                            case 2: // Diagonal
                                row = startRow + i;
                                col = startCol + i;
                                break;
                        }

                        letterGrid[row, col] = word[i];


                       /* //DEBUG: CREATE ADDITIONAL RED LETTERS
                        // Color the placed word red
                        GameObject letterObj = Instantiate(letterPrefab, gridParent);
                        letterObj.GetComponent<TextMeshProUGUI>().text = word[i].ToString();

                        // Set the letter to red if it's part of the word
                        letterObj.GetComponent<TextMeshProUGUI>().color = Color.red;

                        // Position the letter in the grid
                        Vector3 position = new Vector3(row * cellSize, -col * cellSize, 0);
                        letterObj.transform.localPosition = position;
                        //END DEBUG*/
                        
                    }

                    wordPlaced = true; // Mark word as placed
                }
            }
        }

        // If we failed to place the word after maxAttempts, we skip placing it
        if (!wordPlaced)
        {
            Debug.LogWarning($"Failed to place word: {word}");
        }
    }
}
