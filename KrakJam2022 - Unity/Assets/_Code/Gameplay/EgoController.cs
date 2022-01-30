using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EgoController : MonoBehaviour, IInteractable {
        void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Player")) {
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
            }
        }

        public void Interact() {
            Debug.Log("Interacting with EGO");
            var gameplaySys = GameSystems.GetSystem<GameplaySystem>();
            var currentState = GameSystems.GetSystem<GameStateSystem>().runtimeGameState.GetStateForEmotion(gameplaySys.CurrentEmotionLevel);
            if (currentState.CanStartInsanity && !currentState.insanityStarted)
                gameplaySys.BeginInsanityStage();
        }
    }
}
