using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EmotionLevelArea : MonoBehaviour {
        [SerializeField] Emotion emotion;
        [SerializeField] Transform playerSpawnPoint;
        [SerializeField] GameObject exit;
        [SerializeField] CollectibleMemory positiveMemory;
        [SerializeField] CollectibleMemory negativeMemory;

        EmotionLevelState loadedState;
        public Emotion Emotion => emotion;


        public void Initialise() {
            positiveMemory.MemoryCollectedEvent += HandleMemoryCollected;
            negativeMemory.MemoryCollectedEvent += HandleMemoryCollected;
        }

        void HandleMemoryCollected(MemoryData memory) {
            if (memory.type == MemoryType.Positive)
                loadedState.positiveMemoryCollected = true;
            else if (memory.type == MemoryType.Negative)
                loadedState.negativeMemoryCollected = true;
            if (loadedState.positiveMemoryCollected && loadedState.negativeMemoryCollected)
                EnableExitDoor(true);
        }

        public void LoadState(EmotionLevelState state) {
            loadedState = state;
            positiveMemory.gameObject.SetActive(!state.positiveMemoryCollected);
            negativeMemory.gameObject.SetActive(!state.negativeMemoryCollected);
        }

        public void TeleportPlayerToArea() {
            GameSystems.GetSystem<GameplaySystem>().PlayerInstance.transform.position = playerSpawnPoint.position;
            EnableExitDoor(false);
            InitialiseMinigames();
        }

        void InitialiseMinigames() {
            //todo
        }

        void EnableExitDoor(bool canExit) {
            exit.SetActive(canExit);
        }
    }
}
