using UnityEngine;
using UnityEngine.UI;

namespace PartTimeKamikaze.KrakJam2022.UI {
    public class HUDScreen : UiScreenBase {
        [SerializeField] SanityDisplay sanityDisplay;

        protected override float FadeInDuration => 0f;
        protected override float FadeOutDuration => 0f;

        protected override void OnInitialise() {
            var playerDataSystem = GameSystems.GetSystem<PlayerDataSystem>();
            playerDataSystem.Sanity.ChangedValue += SetProgress;

        }

        void SetProgress(int percentage) {
            sanityDisplay.SetFill(percentage / 100f);
        }
    }

    public class SanityDisplay : MonoBehaviour {
        [SerializeField] Image fillImage;


        public void SetFill(float value) {
            fillImage.fillAmount = value;
        }
    }
}
