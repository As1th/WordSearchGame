using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class LetterTileController : MonoBehaviour
{
    public List<LetterTileController> selectedTiles = new List<LetterTileController>();
    private bool isSelected = false;
    public string selectedWord;
    public GameObject manager;
    public GameManager gm;
    public InputManager input;
    private bool saveColor;
    public int selectedCount = 0; // Counter for selected tiles
    private Animator animator;

    private void Start()
    {
        manager = GameObject.Find("GameManager");
        gm = manager.GetComponent<GameManager>();
        input = manager.GetComponent<InputManager>();
        animator = transform.parent.gameObject.GetComponent<Animator>();
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
            
            animator.SetTrigger("Spin");
            saveColor = true;
            animator.SetBool("isDragging", false);
        }
        else
        {
            // Deselect and reset tiles if the word is incorrect
            foreach (LetterTileController letter in input.currentlySelectedTiles)
            {
                letter.DeselectTile(); // Deselect tiles instead of coloring them directly
            }
            input.currentlySelectedTiles.Clear();
        }
    }
    public void DeselectTile()
    {
        if (selectedTiles.Contains(this))
        {
            selectedTiles.Remove(this);
            isSelected = false; // Reset selection
            animator.SetBool("isDragging", false);
            if (!saveColor)
            {
                transform.parent.GetComponent<Image>().color = Color.grey; // Reset to original color
            }
        }
    }
    public void SelectTile()
    {
        if (!selectedTiles.Contains(this))
        {
            selectedTiles.Add(this);
            animator.SetBool("isDragging", true);
            isSelected = true;
            if (!saveColor)
            {
                transform.parent.GetComponent<Image>().color = gm.selectedColors[gm.correctGuesses]; // Highlight the tile
            }
            //selectedWord += this.gameObject.GetComponent<TextMeshProUGUI>().text;
           // gm.selectedWord = selectedWord;
           // gm.selectedWordDisplay.text = selectedWord;
        }
    }
}
