using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Image health;
    public Image[] crystals;
    public Material material;
    public Color darkBlood;
    public Color darkBloodDark;

    void Start() {
        health.material = new Material(material);
        health.material.SetInt("light_mix", 1);
        foreach (Image crystal in crystals) {
            crystal.material = new Material(material);
            crystal.enabled = false;
            crystal.material.SetColor("light_color", darkBlood);
            crystal.material.SetColor("dark_color", darkBloodDark);
        }
        SetHealthLevel(0.8f);
        SetCrystalLevel(2.4f);
    }

    public void SetHealthLevel(float level) {
        health.material.SetFloat("level", level);
    }

    public void SetCrystalLevel(float level) {
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
