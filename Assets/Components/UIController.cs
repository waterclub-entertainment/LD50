using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Image health;
    public Image[] crystals;
    public Material material;
    public Color darkBlood;
    public Color darkBloodDark;
    public Sprite cracked;

    private bool isCracked = false;

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
        }
        for (int i = 0; i < crystals.Length; i++) {
            if (i <= level) {
                crystals[i].enabled = true;
                if (i == (int)level) {
                    crystals[i].material.SetFloat("level", level - (int) level);
                } else {
                    crystals[i].material.SetFloat("level", 1.5f);
                }
            } else {
                crystals[i].enabled = false;
            }
        }
    }

}
