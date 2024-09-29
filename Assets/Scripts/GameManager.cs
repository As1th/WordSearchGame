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
    int themeSelect = 1;
    public Image background;
    public bool isDragging;
    public List<Color> selectedColors = new List<Color>();
    public int correctGuesses = 0;
    public Animator nextThemeButton;

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

    public void NudgeNewRound()
    {
        nextThemeButton.SetTrigger("Grow");
    }

    public void PickNewTheme()
    {
        correctGuesses = 0;
        if(themeSelect == 2)
        {
            themeSelect = 0;
        } else
        {
            themeSelect++;
        }
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
                gridGen.gridSize = 5;
                gridGen.cellSize = 145;
                gridGen.fontSize = 60;
                gridGen.size = 90;
                break;
            case 2:
                gridGen.gridSize = 7;
                gridGen.cellSize = 95;
                gridGen.fontSize = 50;
                gridGen.size = 68;
                break;
            case 3:
                gridGen.gridSize = 8;
                gridGen.cellSize = 85;
                gridGen.fontSize = 50;
                gridGen.size = 53;
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
