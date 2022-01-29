using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EmotionLevelArea : MonoBehaviour {
        [SerializeField] Emotion emotion;
        [SerializeField] Transform playerSpawnPoint;
        [SerializeField] GameObject entrance;


        public Emotion Emotion => emotion;

        int completedMinigames;


        public void LoadState(EmotionLevelState state) {
            //todo load state - ustawic minigierki odpowiednio wg state
            completedMinigames = state.minigamesCompleted;
        }

        public void TeleportPlayerToArea() {
            GameSystems.GetSystem<GameplaySystem>().PlayerInstance.transform.position = playerSpawnPoint.position;
            EnableExitDoor(false);
            InitialiseMinigames();
        }

        void InitialiseMinigames() {

        }

        void EnableExitDoor(bool canExit) {
//            exitDoor.SetActive(canExit);
        }
    }
}
