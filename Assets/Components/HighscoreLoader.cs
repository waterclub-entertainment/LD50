using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreLoader : MonoBehaviour
{
    public TextMeshProUGUI scores;
    public TextMeshProUGUI names;

    // Start is called before the first frame update
    void Start()
    {
        string text = "";
        int[] scoreArray = HighScores.scores();
        for(int i = 0; i < 10; i++)
        {
            text += scoreArray[i] + "\n";
        }
        scores.text = text;
        text = "";
        string[] nameArray = HighScores.names();
        for(int i = 0; i < 10; i++)
        {
            text += nameArray[i] + "\n";
        }
        names.text = text;
    }

    // Update is called once per frame
    void Update()
    {

    }   
}

public static class HighScores
{
    private static int[] scoreArray = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
    private static string[] nameArray = {"Alice", "Bob", "Carol", "Dave", "Eve",
            "Mallet", "Oscar", "Peggy", "Trudy", "Trent"};

    public static int[] scores()
    {
        return scoreArray;
    }

    public static string[] names()
    {
        return nameArray;
    }
}