using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour {

    public HighscoreData highscoreData;
    public Text scoreText;
    public InputField nameInput;

    void Start() {
        scoreText.text = "Score: " + (int) highscoreData.score;
    }

    public void OnSubmitScore() {
        string name = nameInput.text;
        Debug.Log(name + " " + (int) highscoreData.score);
        SceneManager.LoadScene("Main");
    }

}
