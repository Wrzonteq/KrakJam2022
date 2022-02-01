using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022.UI {
    public class HUDScreen : UiScreenBase {
        [SerializeField] InsanityDisplay insanityDisplay;

        protected override float FadeInDuration => 0f;
        protected override float FadeOutDuration => 0f;

        protected override void OnInitialise() {
            var gameStateSystem = GameSystems.GetSystem<GameStateSystem>();
            gameStateSystem.Insanity.ChangedValue += SetProgress;
            gameStateSystem.Stage.ChangedValue += HandleStageChanged;
            insanityDisplay.gameObject.SetActive(false);
        }

        void OnDestroy() {
            var gameStateSystem = GameSystems.GetSystem<GameStateSystem>();
            gameStateSystem.Insanity.ChangedValue -= SetProgress;
            gameStateSystem.Stage.ChangedValue -= HandleStageChanged;
        }

        void HandleStageChanged(GameStage stage) {
            insanityDisplay.gameObject.SetActive(stage == GameStage.Insanity);
        }

        void SetProgress(int percentage) {
            insanityDisplay.SetFill(percentage / 100f);
        }
    }
}
