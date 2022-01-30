using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PartTimeKamikaze.KrakJam2022.UI {
    public class GameOverScreen : EndingScreenBase { }

    public abstract class EndingScreenBase : UiScreenBase {
        [SerializeField] CanvasGroup content;
        [SerializeField] Button backToMenuButton;


        protected override void OnInitialise() {
            backToMenuButton.onClick.AddListener(HandleBackToMenuButton);
        }

        protected override void OnShow() {
            base.OnShow();
            content.DOFade(0, 0);
            content.interactable = false;
        }

        protected override void OnFadeInComplete() {
            content.DOFade(1, 1).OnComplete(() => {
                                                content.interactable = true;
                                                backToMenuButton.Select();
                                            });
        }

        void HandleBackToMenuButton() {
            GameSystems.GetSystem<GameplaySystem>().ReturnToMenu().Forget();
            Hide(true).Forget();
        }
    }
}
