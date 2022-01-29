using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EmotionLevelArea : MonoBehaviour {
        [SerializeField] Emotion emotion;
        [SerializeField] Transform playerSpawnPoint;

        public Emotion Emotion => emotion;
        public Transform PlayerSpawnPoint => playerSpawnPoint;


        // przypisac minigierki
        // zrobic brame do wyjscia
    }
}
