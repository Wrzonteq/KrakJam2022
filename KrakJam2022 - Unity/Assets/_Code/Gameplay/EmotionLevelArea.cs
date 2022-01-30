using Cysharp.Threading.Tasks;
using PartTimeKamikaze.KrakJam2022.UI;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EmotionLevelArea : MonoBehaviour {
        [SerializeField] Emotion emotion;
        [SerializeField] Transform playerSpawnPoint;
        [SerializeField] LevelExitGate exit;
        [SerializeField] CollectibleMemory positiveMemory;
        [SerializeField] CollectibleMemory negativeMemory;
        [SerializeField] Minigame[] minigames;

        EmotionLevelState loadedState;
        public Emotion Emotion => emotion;
        public bool IsCompleted => loadedState.negativeMemoryCollected && loadedState.positiveMemoryCollected;
        public bool IsPlayerInside { get; private set; }


        public void LoadState(EmotionLevelState state) {
            loadedState = state;
            positiveMemory.gameObject.SetActive(!state.positiveMemoryCollected);
            negativeMemory.gameObject.SetActive(!state.negativeMemoryCollected);
            InitialiseMinigames();
        }

        void InitialiseMinigames() {
            foreach (var mg in minigames)
                mg.Initialise();
        }

        public void Initialise() {
            positiveMemory.MemoryCollectedEvent += HandleMemoryCollected;
            negativeMemory.MemoryCollectedEvent += HandleMemoryCollected;
            exit.GateEnteredEvent += ReturnPlayerToGate;
        }

        void HandleMemoryCollected(MemoryData memory) {
            if (memory.type == MemoryType.Positive)
                loadedState.positiveMemoryCollected = true;
            else if (memory.type == MemoryType.Negative)
                loadedState.negativeMemoryCollected = true;
            if (loadedState.positiveMemoryCollected && loadedState.negativeMemoryCollected)
                HandleLevelComplete();
        }

        void HandleLevelComplete() {
            exit.Open();
        }

        void ReturnPlayerToGate() {
            var transitionScreen = GameSystems.GetSystem<UISystem>().GetScreen<TransitionScreen>();
            transitionScreen.Show().Forget();
            transitionScreen.FadeInAndOut(TeleportPlayer, OnTransitionDone).Forget();

            void TeleportPlayer() {
                var gatePosition = GameSystems.GetSystem<GameplaySystem>().GetGate(emotion).transform.position;
                GameSystems.GetSystem<GameplaySystem>().PlayerInstance.Teleport(gatePosition);
                IsPlayerInside = false;
            }

            void OnTransitionDone() {
                transitionScreen.Hide().Forget();
            }
        }

        public void TeleportPlayerToArea() {
            GameSystems.GetSystem<GameplaySystem>().PlayerInstance.Teleport(playerSpawnPoint.position);
            exit.Close();
            IsPlayerInside = true;
        }
    }
}
