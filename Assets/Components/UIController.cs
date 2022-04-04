using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Image health;
    public Image[] crystals;
    public Material material;
    public Color darkBlood;
    public Color darkBloodDark;
    public Sprite cracked;
    public Text score;
    public HighscoreData highscoreData;
    public AudioClip breakSound;

    private bool isCracked = false;
    private int lastMainCrystalIndex = -1;
    private Vector2 lastAnchor = Vector2.zero;

    void Start() {
        health.material = new Material(material);
        health.material.SetInt("light_mix", 1);
        foreach (Image crystal in crystals) {
            crystal.material = new Material(material);
            crystal.enabled = false;
            crystal.material.SetColor("light_color", darkBlood);
            crystal.material.SetColor("dark_color", darkBloodDark);
        }
    }

    void Update() {
        if (highscoreData.score > 0) {
            score.text = "" + ((int) highscoreData.score) * 100;
        } else {
            score.text = "";
        }
    }

    public void SetHealthLevel(float level) {
        float min = 0.165f;
        float max = 0.85f;
        level = level * (max - min) + min;
        health.material.SetFloat("level", level);
    }

    public void SetCrystalLevel(float level) {
        if (!isCracked) {
            health.sprite = cracked;
            isCracked = true;
            if (level == 0) {
                GetComponent<AudioSource>().PlayOneShot(breakSound);
            }
        }
        for (int i = 0; i < crystals.Length; i++) {
            if (i <= level) {
                crystals[i].enabled = true;
                if (i == (int)level) {
                    if (i != lastMainCrystalIndex) {
                        if (lastMainCrystalIndex != -1) {
                            crystals[lastMainCrystalIndex].rectTransform.anchoredPosition = lastAnchor;
                        }
                        lastAnchor = crystals[i].rectTransform.anchoredPosition;
                        crystals[i].rectTransform.anchoredPosition = new Vector2(53, -113);
                    }
                    crystals[i].material.SetFloat("level", level - (int) level);
                    lastMainCrystalIndex = i;
                } else {
                    crystals[i].material.SetFloat("level", 1.5f);
                }
            } else {
                crystals[i].enabled = false;
            }
        }
    }

}
