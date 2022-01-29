using PartTimeKamikaze.KrakJam2022.Utils;

namespace PartTimeKamikaze.KrakJam2022 {
    public class InsanitySystem : BaseGameSystem {
        public Observable<float> InsanityLevel { get; private set; }

        public override void OnCreate() {
            InsanityLevel = new Observable<float>();
        }

        public override void Initialise() {
            GameSystems.GetSystem<GameStateSystem>().Stage.ChangedValue+= HandleStageChanged;
        }

        void HandleStageChanged(GameStage stage) {
            if(stage == GameStage.Insanity)
                InitInsanity();
            else
                EndInsanity();
        }

        void InitInsanity() {
            GameSystems.GetSystem<EnemiesSystem>().StartEnemySpawning().Forget();
            InsanityLevel.Value = 40 + 10 * GameSystems.GetSystem<GameStateSystem>().ClosedGatesCount.Value;

        }

        void EndInsanity() {
            //todo
            GameSystems.GetSystem<EnemiesSystem>().StopEnemySpawning();
        }
    }
}
