using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Theme
{
    public string name;
    public List<string> words;

    public Theme(string name, List<string> words)
    {
        this.name = name;
        this.words = words;
    }
}

public class GameManager : MonoBehaviour
{

    public GridGenerator gridGen;
    public List<Theme> themes=null;
    public Theme currentTheme;
    public TextMeshProUGUI wordListText; // Link to the UI element displaying words
    public int gridSize = 20; // Example grid size, could be dynamic

   public Theme picnic = new Theme("Picnic",
           new List<string> {
                "food", "ants", "milk", "jam", "cake", "corn", "soda", "buns", "nap", "fork",
                "dish", "pie", "cook", "park", "sun", "wine", "chips", "cup", "rug", "tent",
                "seat", "grass", "tree", "plum", "rain", "pack", "bag", "ice", "grill", "mayo",
                "snack", "egg", "ham", "soup", "fire", "tent", "camp", "fly", "hive", "cold",
                "warm", "wind", "sand", "hat", "roll", "hike", "lake", "boat", "bug", "fun"
               }
           );

    public Theme city = new Theme("City",
       new List<string> {
                 "bus", "cab", "map", "road", "sign", "shop", "park", "mall", "cafe", "rush",
                "bank", "club", "taxi", "bike", "lane", "grid", "loud", "gate", "walk", "flat",
                "stop", "post", "tram", "view", "dome", "grid", "work", "exit", "tour", "film",
                "bar", "gym", "coin", "cash", "metro", "fare", "zone", "step", "flag", "fast",
                "mall", "ride", "port", "rent", "lane", "news", "rail", "bus", "rush"
            }
       );

    public Theme games = new Theme("Games",
       new List<string> {
                "card", "move", "race", "dice", "goal", "jump", "ball", "pawn", "club", "roll",
                "grid", "shot", "play", "life", "quiz", "coin", "king", "quiz", "dice", "dart",
                "draw", "hand", "pool", "pong", "deck", "team", "ball", "wave", "fire", "pass",
                "roll", "maze", "duel", "bet", "cash", "cast", "luck", "word", "spin", "jump",
                "flip", "chip", "boom", "dart", "solo", "free", "slot", "time", "clap", "tap"
            }
       );
    public List<string> currentRound = new List<string>();

    void Start()
    {


         currentRound = new List<string> {
                "card", "move", "race", "dice", "goal"
            };


        print(themes);
        // Select a theme (this could be randomized or chosen by the player)
        currentTheme = games;
        DisplayWordList();
        gridGen.GenerateGrid(currentRound);
    }

    void DisplayWordList()
    {
        wordListText.text = ""; // Clear the text
        foreach (string word in currentRound)
        {
            wordListText.text += word.ToUpper() + "\n";
        }
    }
}
