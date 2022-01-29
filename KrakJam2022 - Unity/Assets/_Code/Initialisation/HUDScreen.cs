using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022.UI {
    public class HUDScreen : UiScreenBase {
        [SerializeField] SanityDisplay sanityDisplay;

        protected override float FadeInDuration => 0f;
        protected override float FadeOutDuration => 0f;

        protected override void OnInitialise() {
            var gameStateSystem = GameSystems.GetSystem<GameStateSystem>();
            gameStateSystem.Insanity.ChangedValue += SetProgress;
        }

        void SetProgress(int percentage) {
            sanityDisplay.SetFill(percentage / 100f);
        }
    }
}
