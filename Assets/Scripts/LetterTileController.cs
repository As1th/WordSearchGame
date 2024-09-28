using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class LetterTileController : MonoBehaviour
{
    public static List<LetterTileController> selectedTiles = new List<LetterTileController>();
    private bool isSelected = false;
    public string selectedWord;
    public GameManager gm;
    public int selectedCount = 0; // Counter for selected tiles

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnDragStart()
    {
        if (!isSelected)
        {
            SelectTile();
        }
    }

    public void OnDrag()
    {
        if (!isSelected)
        {
            SelectTile();
        }
    }

    public void OnDragEnd()
    {
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
                transform.parent.GetComponent<Image>().color = Color.blue;
            }
            selectedTiles.Clear();
        }
    }

    public void SelectTile()
    {
        if (!selectedTiles.Contains(this))
        {
            selectedTiles.Add(this);
            isSelected = true;
            transform.parent.GetComponent<Image>().color = Color.green; // Highlight the tile
            //selectedWord += this.gameObject.GetComponent<TextMeshProUGUI>().text;
           // gm.selectedWord = selectedWord;
           // gm.selectedWordDisplay.text = selectedWord;
        }
    }
}
