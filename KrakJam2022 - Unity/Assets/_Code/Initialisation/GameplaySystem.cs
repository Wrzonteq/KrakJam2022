using Cysharp.Threading.Tasks;
using PartTimeKamikaze.KrakJam2022.UI;

namespace PartTimeKamikaze.KrakJam2022 {
    public class GameplaySystem : BaseGameSystem {

        public override void OnCreate() { }
        public override void Initialise() { }

        public void StartNewGame() {
            LoadGameplaySceneAndShowProgress().Forget();

            GameSystems.GetSystem<GameStateSystem>().ResetGameState();

            //todo load map, player etc. using GameStateSystem's runtimeGameState
        }

        async UniTaskVoid LoadGameplaySceneAndShowProgress() {
            GameSystems.GetSystem<UISystem>().GetScreen<MainMenuScreen>().Hide(true).Forget();
            var sceneLoadingSystem = GameSystems.GetSystem<SceneLoadingSystem>();
            GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Show(true).Forget();
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue += DisplayProgress;
            await sceneLoadingSystem.LoadSceneAsync(Consts.ScenesNames.Gameplay);
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue -= DisplayProgress;
            await GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Hide();

            void DisplayProgress(float progress) {
                GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().SetProgress(progress);
            }
        }
    }
}
