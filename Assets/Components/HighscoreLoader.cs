using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using SimpleJSON;

public class HighscoreLoader : MonoBehaviour
{
    public TextMeshProUGUI scores;
    public TextMeshProUGUI names;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRequest();
        Invoke("GenerateRequest", 10f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private string URL = "v2202001110922105851.happysrv.de:42069/highscores";

    public void GenerateRequest()
    {
        StartCoroutine(ProcessRequest(URL));
    }
    private int N = 10;

    private IEnumerator ProcessRequest(string uri)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                JSONNode data =
                    JSON.Parse(request.downloadHandler.text);
                string namestext = "";
                string scorestext = "";
                for (int i = 0; i < N; i++)
                {
                    namestext += data[i]["name"] + "\n";
                    scorestext += data[i]["score"] + "\n";
                }
                names.text = namestext;
                scores.text = scorestext;
            }
        }
    }
}