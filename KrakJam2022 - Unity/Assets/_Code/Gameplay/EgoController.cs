<<<<<<< HEAD
using PartTimeKamikaze.KrakJam2022.UI;
=======
using System;
>>>>>>> af47e45 (change ego on phase)
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EgoController : MonoBehaviour, IInteractable {
        [SerializeField] MessagePopup message;

        void Awake() {
            message.Hide();
        }

        [SerializeField] GameObject saneVersion;
        [SerializeField] GameObject insaneVersion;
        
        void Start() {
            GameSystems.GetSystem<GameStateSystem>().Stage.ChangedValue += HandleStageChanged;
        }

        void HandleStageChanged(GameStage stage) {
            if (stage == GameStage.Insanity) {
                saneVersion.SetActive(false);
                insaneVersion.SetActive(true);
            } else {
                saneVersion.SetActive(true);
                insaneVersion.SetActive(false);
            }
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
