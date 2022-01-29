using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PartTimeKamikaze.KrakJam2022.UI;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {
    public class GameplaySystem : BaseGameSystem {

//        List<LevelGate>


        public bool IsInGameplay { get; private set; }

        public override void OnCreate() { }

        public override void Initialise() { }

        public async UniTaskVoid StartNewGame() {
            await LoadGameplaySceneAndShowProgress();
            GameSystems.GetSystem<InputSystem>().SwitchToGameplayInput();
            GameSystems.GetSystem<GameStateSystem>().ResetGameState();
            LoadGame(GameSystems.GetSystem<GameStateSystem>().runtimeGameState);
        }

        async UniTask LoadGameplaySceneAndShowProgress() {
            GameSystems.GetSystem<UISystem>().GetScreen<MainMenuScreen>().Hide(true).Forget();
            var sceneLoadingSystem = GameSystems.GetSystem<SceneLoadingSystem>();
            GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Show(true).Forget();
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue += DisplayProgress;
            await sceneLoadingSystem.LoadSceneAsync(Consts.ScenesNames.Gameplay);
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue -= DisplayProgress;
            GameSystems.GetSystem<UISystem>().ShowScreen<HUDScreen>().Forget();
            await GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Hide();
        }

        void LoadGame(GameStateDataAsset gameState) {
            IsInGameplay = true;
            //todo load map, player etc. using gameState
            GameSystems.GetSystem<InputSystem>().Bindings.Gameplay.OpenPauseMenu.performed += OpenPauseScreen;
        }

        void OpenPauseScreen(InputAction.CallbackContext obj) {
            GameSystems.GetSystem<UISystem>().GetScreen<PauseMenuScreen>().Show().Forget();
        }

        public async UniTaskVoid ReturnToMenu() {
            GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Show(true).Forget();
            GameSystems.GetSystem<UISystem>().HideScreen<HUDScreen>().Forget();
            var sceneLoadingSystem = GameSystems.GetSystem<SceneLoadingSystem>();
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue += DisplayProgress;
            await sceneLoadingSystem.UnloadSceneAsync(Consts.ScenesNames.Gameplay);
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue -= DisplayProgress;
            GameSystems.GetSystem<UISystem>().GetScreen<MainMenuScreen>().Show(true).Forget();
            await GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Hide();
        }

        void DisplayProgress(float progress) {
            GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().SetProgress(progress);
        }

        public void BeginInsanityStage() {
            GameSystems.GetSystem<GameStateSystem>().Stage.Value = GameStage.Insanity;
            //todo
        }

        public void EndInsanityStage() {
            var gameStateSystem = GameSystems.GetSystem<GameStateSystem>();
            gameStateSystem.Stage.Value = GameStage.Sanity;
            gameStateSystem.ClosedGatesCount.Value++;
            if (gameStateSystem.ClosedGatesCount.Value < Consts.TOTAL_GATES_COUNT)
                HandleInsanityCompleted();
            else
                HandleAllGatesClosed();
        }

        void HandleInsanityCompleted() {

        }

        void HandleAllGatesClosed() {
            //TODO - WIN, End game
        }


        //TODO - WARUNKI PRZEGRANEJ
    }
}
