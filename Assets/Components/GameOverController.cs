using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GameOverController : MonoBehaviour
{

    public HighscoreData highscoreData;
    public Text scoreText;
    public InputField nameInput;

    private int score;
    private string name;
    private static string URL = "v2202001110922105851.happysrv.de:42069/highscores";

    void Start()
    {
        scoreText.text = "Score: " + (int)highscoreData.score;
    }

    public void OnSubmitScore()
    {
        name = nameInput.text;
        score = (int)highscoreData.score;

        StartCoroutine("Upload");
    }

    public IEnumerator Upload()
    {
        string tosend = "{\"name\": \"" + name + "\", \"score\": " + score + "}";
        Debug.Log("Sending JSON: " + tosend);
        using UnityWebRequest webRequest = new UnityWebRequest(URL, "POST");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] rawUploadData = Encoding.UTF8.GetBytes(tosend);
        webRequest.uploadHandler = new UploadHandlerRaw(rawUploadData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();
    }

}
