using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class InsanitySystem : BaseGameSystem {
        public override void OnCreate() { }

        void HandleInsanityValueChanged(int insanity) {
            if (insanity <= 0)
                GameSystems.GetSystem<GameplaySystem>().EndInsanityStage();
            else if (insanity >= 100)
                GameSystems.GetSystem<GameplaySystem>().HandleGameOver();
        }

        public override void Initialise() {
            GameSystems.GetSystem<GameStateSystem>().Stage.ChangedValue += HandleStageChanged;
        }

        void HandleStageChanged(GameStage stage) {
            if(stage == GameStage.Insanity)
                InitInsanity();
            else
                EndInsanity();
        }

        void InitInsanity() {
            GameSystems.GetSystem<EnemiesSystem>().StartEnemySpawning().Forget();
            GameSystems.GetSystem<GameStateSystem>().Insanity.Value = 40 + 10 * GameSystems.GetSystem<GameStateSystem>().ClosedGatesCount.Value;
            GameSystems.GetSystem<GameStateSystem>().Insanity.ChangedValue += HandleInsanityValueChanged;
            Debug.Log($"Insanity started!");
        }

        void EndInsanity() {
            Debug.Log($"Insanity over!");
            GameSystems.GetSystem<GameStateSystem>().Insanity.ChangedValue -= HandleInsanityValueChanged;
            GameSystems.GetSystem<EnemiesSystem>().StopEnemySpawning();
        }
    }
}
