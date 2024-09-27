using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GridGenerator gridGen;
    public List<List<string>> themes = new List<List<string>> { };
    public List<string> currentTheme;
    public int difficulty = 1;
    public TextMeshProUGUI wordListText;
    public string selectedWord;
    public TextMeshProUGUI selectedWordDisplay;
    public Sprite picnicBG;
    public Sprite cityBG;
    public Sprite gamesBG;
    public Image background;

    public List<string> picnic =
            new List<string> {
                "food", "ants", "milk", "jam", "cake", "corn", "soda", "buns", "nap", "fork",
                "dish", "pie", "cook", "park", "sun", "wine", "chips", "cup", "rug", "tent",
                "seat", "grass", "tree", "plum", "rain", "pack", "bag", "ice", "grill", "mayo",
                "snack", "egg", "ham", "soup", "fire", "tent", "camp", "fly", "hive", "cold",
                "warm", "wind", "sand", "hat", "roll", "hike", "lake", "boat", "bug", "fun"
                };


    public List<string> city =
       new List<string> {
                 "bus", "cab", "map", "road", "sign", "shop", "park", "mall", "cafe", "rush",
                "bank", "club", "taxi", "bike", "lane", "grid", "loud", "gate", "walk", "flat",
                "stop", "post", "tram", "view", "dome", "grid", "work", "exit", "tour", "film",
                "bar", "gym", "coin", "cash", "metro", "fare", "zone", "step", "flag", "fast",
                "mall", "ride", "port", "rent", "lane", "news", "rail", "bus", "rush"
            };


    public List<string> games =
       new List<string> {
                "card", "move", "race", "dice", "goal", "jump", "ball", "pawn", "club", "roll",
                "grid", "shot", "play", "life", "quiz", "coin", "king", "quiz", "dice", "dart",
                "draw", "hand", "pool", "pong", "deck", "team", "ball", "wave", "fire", "pass",
                "roll", "maze", "duel", "bet", "cash", "cast", "luck", "word", "spin", "jump",
                "flip", "chip", "boom", "dart", "solo", "free", "slot", "time", "clap", "tap"
            };

    public List<string> currentRound = new List<string>();

    void Start()
    {
        themes.Add(picnic); themes.Add(city); themes.Add(games);



        PickNewTheme();




    }

    public bool checkWord()
    {
        if (currentRound.Contains(selectedWord.ToLower()))
        {
            print("y");
            return true;

        }
        else
        {

            print("n");
            return false;
        }

    }

    public void PickNewTheme()
    {
        int themeSelect = Random.Range(0, themes.Count);
        currentTheme = themes[themeSelect];

        if(themeSelect== 0)
        {
            background.sprite = picnicBG;

        } else if(themeSelect== 1)
        {
            background.sprite = cityBG;
        } else
        {
            background.sprite = gamesBG;
        }

        int numberOfWordsToPick = difficulty + 2;

        // Shuffle the original list
        currentRound = currentTheme.OrderBy(a => Random.value).ToList();

        // Take 'count' number of words from the shuffled list
        currentRound = currentRound.Take(numberOfWordsToPick).ToList();



        DisplayWordList();
        gridGen.GenerateGrid(currentRound);
    }

    public void SetDifficulty(int diff)
    {
        difficulty = diff;
        switch (difficulty)
        {
            case 1:
                gridGen.gridSize = 6;
                gridGen.cellSize = 110;
                gridGen.fontSize = 60;
                gridGen.size = 105;
                break;
            case 2:
                gridGen.gridSize = 8;
                gridGen.cellSize = 80;
                gridGen.fontSize = 50;
                gridGen.size = 82;
                break;
            case 3:
                gridGen.gridSize = 10;
                gridGen.cellSize = 60;
                gridGen.fontSize = 40;
                gridGen.size = 60;
                break;

        }
        PickNewTheme();

    }

    void DisplayWordList()
    {
        wordListText.text = ""; // Clear the text

        foreach (string word in currentRound)
        {
            wordListText.text += word.ToUpper() + ", ";

        }
    }
}
