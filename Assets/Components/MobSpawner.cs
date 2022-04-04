using UnityEngine;
using UnityEngine.Tilemaps;

public class MobSpawner : MonoBehaviour {

    [System.Serializable]
    public struct MobDifficulty {
        public GameObject mob;
        public float minDifficulty;
        public float spawningRate;
    }

    public MobDifficulty[] mobs;
    public AnimationCurve difficultyCurve;
    public float spawnMinDistance = 10f;
    public float spawnMaxDistance = 20f;
    public float difficultyGain = 1/60f;
    public Tilemap floor;

    public UIController diffDisplay;

    private float spawningThreshold = 0.0f;
    public float difficulty = 0.0f;

    void Start()
    {
    }

    void Update() {
        difficulty += difficultyGain * Time.deltaTime;

        diffDisplay.SetCrystalLevel(10.0f * difficulty);

        spawningThreshold -= Time.deltaTime * difficultyCurve.Evaluate(difficulty);
        while (spawningThreshold <= 0) {
            float totalWeight = 0f;
            foreach (MobDifficulty mob in mobs) {
                if (mob.minDifficulty <= difficulty) {
                    totalWeight += mob.spawningRate;
                }
            }
            if (totalWeight == 0f) {
                // No mob can be selected, just do nothing I guess
                Debug.Log("shit1");
                spawningThreshold = 0f;
                break;
            } else {
                float r = Random.value * totalWeight;
                bool worked = false;
                foreach (MobDifficulty mob in mobs) {
                    if (mob.minDifficulty <= difficulty) {
                        r -= mob.spawningRate;
                        if (r <= 0) {
                            spawningThreshold += 1 / mob.spawningRate;
                            Spawn(mob.mob);
                            worked = true;
                            break;
                        }
                    }
                }
                if (!worked) {
                    // No mob was selected, just do nothing I guess
                    Debug.Log("shit");
                    spawningThreshold = 0f;
                    break;
                }
            }
        }
    }

    private void Spawn(GameObject mob) {
        // 20 tries
        for (int i = 0; i < 20; i++) {
            GameObject player = GameObject.FindWithTag("Player");
            float dist = Random.value * (spawnMaxDistance - spawnMinDistance) + spawnMinDistance;
            float spawnAngle = Random.value * Mathf.PI * 2;
            Vector3 position = new Vector3(Mathf.Cos(spawnAngle), 0, Mathf.Sin(spawnAngle)) * dist + player.transform.position;
            Vector3Int tilePosition = floor.WorldToCell(position);
            tilePosition.z = 0; // Yes it is z and not y
            if (floor.GetTile(tilePosition) == null) {
                continue;
            }
            position = floor.CellToWorld(tilePosition);
            position.y = 1f;
            Instantiate(mob, position, Quaternion.identity);
            return;
        }
    }
}
