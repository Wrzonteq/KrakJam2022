using PartTimeKamikaze.KrakJam2022.UI;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EgoController : MonoBehaviour, IInteractable {
        [SerializeField] MessagePopup message;
        [SerializeField] GameObject saneVersion;
        [SerializeField] GameObject insaneVersion;


        void Awake() {
            message.Hide();
        }

        void OnDestroy() {
            Debug.Log("Ego Destroyed");
            GameSystems.GetSystem<GameStateSystem>().Stage.ChangedValue -= HandleStageChanged;
        }

        void Start() {
            GameSystems.GetSystem<GameStateSystem>().Stage.ChangedValue += HandleStageChanged;
        }

        void HandleStageChanged(GameStage stage) {
            saneVersion.SetActive(stage == GameStage.Sanity);
            insaneVersion.SetActive(stage == GameStage.Insanity);
        }

        void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Player")) {
                ShowMessageAccordingToState();
                GameSystems.GetSystem<GameplaySystem>().PlayerInstance.RegisterInteractable(this);
            } else if (col.CompareTag("Enemy")) {
                var enemy = col.GetComponent<Enemy>();
                GameSystems.GetSystem<GameStateSystem>().Insanity.Value += enemy.Damage;
                enemy.Kill().Forget();
            }
        }

        void OnTriggerExit2D(Collider2D col) {
            if (col.CompareTag("Player")) {
                GameSystems.GetSystem<GameplaySystem>().PlayerInstance.UnregisterInteractable(this);
                message.Hide();
            }
        }

        public void Interact() {
            Debug.Log("Interacting with EGO");
            var gameplaySys = GameSystems.GetSystem<GameplaySystem>();
            var currentState = gameplaySys.GetCurrentEmotionState();
            if (currentState.CanStartInsanity && !currentState.insanityStarted) {
                gameplaySys.BeginInsanityStage();
                message.Hide();
            }
        }

        void ShowMessageAccordingToState() {
            var gameplaySys = GameSystems.GetSystem<GameplaySystem>();
            var currentState = gameplaySys.GetCurrentEmotionState();
            if (currentState.CanStartInsanity && !currentState.insanityStarted) {
                message.SetText("[E] Go insane and fight your Traumas!");
            } else {
                message.SetText("Go through Gates to collect memories");
            }
            message.Show();
        }
    }
}
