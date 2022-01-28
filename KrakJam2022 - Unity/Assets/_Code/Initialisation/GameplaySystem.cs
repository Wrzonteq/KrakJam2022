using Cysharp.Threading.Tasks;
using PartTimeKamikaze.KrakJam2022.UI;

namespace PartTimeKamikaze.KrakJam2022 {
    public class GameplaySystem : BaseGameSystem {
        public void StartNewGame() {
            GameSystems.GetSystem<UISystem>().GetScreen<MainMenuScreen>().Hide(true).Forget();

        }

        public override void OnCreate() { }
        public override void Initialise() { }
    }
}
