using UnityEngine;

[CreateAssetMenu(fileName = "HighscoreData", menuName = "Highscore Data")]
public class HighscoreData : ScriptableObject {
    public float _score = 0f;
    public float score
    {
        get { return _score; }
    }
    public float multiplier = 1.0f;

    public void addScore(float score)
    {
        _score += score * multiplier;
    }

    public void SetScore(float score)
    {
        _score = score;
    }
}
