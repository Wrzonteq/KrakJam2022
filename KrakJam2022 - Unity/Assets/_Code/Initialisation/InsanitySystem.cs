using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class InsanitySystem : BaseGameSystem {
        public override void OnCreate() { }

        void HandleInsanityValueChanged(int insanity) {
            if (insanity <= 0) {
                GameSystems.GetSystem<GameplaySystem>().EndInsanityStage();
            } else if (insanity >= 100) {
                GameSystems.GetSystem<GameplaySystem>().HandleGameOver();
            }
        }

        public override void Initialise() {
            var stateSystem = GameSystems.GetSystem<GameStateSystem>();
            stateSystem.Stage.ChangedValue += HandleStageChanged;
            stateSystem.Insanity.ChangedValue += HandleInsanityValueChanged;
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
            Debug.Log($"Insanity started!");
        }

        void EndInsanity() {
            Debug.Log($"Insanity over!");
            GameSystems.GetSystem<EnemiesSystem>().StopEnemySpawning();
        }
    }
}
