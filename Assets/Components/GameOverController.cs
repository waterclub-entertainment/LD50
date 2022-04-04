using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour {

    public HighscoreData highscoreData;
    public Text scoreText;
    public InputField nameInput;

    void Start() {
        scoreText.text = "Score: " + ((int) highscoreData.score * 100);
    }

    public void OnSubmitScore() {
        string name = nameInput.text;
        Debug.Log(name + " " + ((int) highscoreData.score) * 100);
        OnBack();
    }

    public void OnBack() {
        highscoreData.SetScore(0);
        SceneManager.LoadScene("Main");
    }

}
