namespace PartTimeKamikaze.KrakJam2022 {
    public class InsanitySystem : BaseGameSystem {
        public override void OnCreate() {
            
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
            GameSystems.GetSystem<EnemiesSystem>().StartEnemySpawning();
            
            //todo start spawning
            //todo start tracking killed enemies
        }

        void EndInsanity() {
            //todo
            GameSystems.GetSystem<EnemiesSystem>().StopEnemySpawning();
        }
    }
}