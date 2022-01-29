using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Time = UnityEngine.Time;

namespace PartTimeKamikaze.KrakJam2022.UI {
    public class PauseMenuScreen : UiScreenBase {
        [SerializeField] Button continueButton;
        [SerializeField] Button returnToMenuButton;

        protected override float FadeInDuration => 0;
        protected override float FadeOutDuration => 0;

        protected override void OnInitialise() {
            continueButton.onClick.AddListener(HandleContinueButton);
            returnToMenuButton.onClick.AddListener(HandleReturnToMenuButton);
        }

        void HandleContinueButton() {
            Hide().Forget();
        }

        void HandleReturnToMenuButton() {
            Hide().Forget();
            GameSystems.GetSystem<GameplaySystem>().ReturnToMenu().Forget();
        }

        protected override void OnShow() {
            continueButton.Select();
            Time.timeScale = 0;
            //todo subscribe to interface cancel input
        }

        protected override void OnHide() {
            Time.timeScale = 1;
            //todo unsubscribe from interface cancel input
        }
    }
}
