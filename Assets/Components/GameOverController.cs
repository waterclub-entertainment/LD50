using UnityEngine;
using UnityEngine.SceneManagement;
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

    void Start()
    {
        score = (int)highscoreData.score * 100;
        scoreText.text = "Score: " + score;
    }

    public void OnBack()
    {
        highscoreData.SetScore(0);
        SceneManager.LoadScene("Main");
    }
    private int score;
    private string name;
    private static string URL = "v2202001110922105851.happysrv.de:42069/highscores";

    public void OnSubmitScore()
    {
        name = nameInput.text;

        StartCoroutine("Upload");
        OnBack();
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
