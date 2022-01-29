using UnityEngine;
using UnityEngine.UI;

namespace PartTimeKamikaze.KrakJam2022.UI {
    public class MainMenuScreen : UiScreenBase {
        [SerializeField] Button newGameButton;
        [SerializeField] Button exitButton;
        [SerializeField] Button creditsButton;


        protected override void OnInitialise() {
            newGameButton.onClick.AddListener(HandleNewGame);
            creditsButton.onClick.AddListener(HandleCredits);
            exitButton.onClick.AddListener(HandleExit);
        }

        protected override void OnShow() {
            newGameButton.Select();
        }

        void HandleNewGame() {
            GameSystems.GetSystem<GameplaySystem>().StartNewGame();
        }

        void HandleCredits() {
            //todo show credits screen 
        }

        void HandleExit() {
            Application.Quit();
        }
    }
}
