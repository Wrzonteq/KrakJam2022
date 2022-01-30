using PartTimeKamikaze.KrakJam2022.Utils;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class GameStateSystem : BaseGameSystem {
        [SerializeField] GameStateDataAsset defaultGameState;

        public GameStateDataAsset runtimeGameState { get; private set; }

        public Observable<int> Insanity { get; private set; }
        public Observable<GameStage> Stage { get; private set; }
        public Observable<int> ClosedGatesCount { get; private set; }


        public override void OnCreate() {
            runtimeGameState = ScriptableObject.CreateInstance<GameStateDataAsset>();
            Insanity = new Observable<int>();
            Stage = new Observable<GameStage>();
            ClosedGatesCount = new Observable<int>();

            Insanity.ChangedValue += x => runtimeGameState.currentSanity = x;
            Stage.ChangedValue += x => runtimeGameState.stage = x;
            ClosedGatesCount.ChangedValue += x => runtimeGameState.completedLevelsCount = x;
        }

        public override void Initialise() { }

        public void ResetGameState() {
            var json = JsonUtility.ToJson(defaultGameState);
            JsonUtility.FromJsonOverwrite(json, runtimeGameState);
        }

        public void LoadCurrentStateToProperties() {
            Insanity.SilentSet(runtimeGameState.currentSanity);
            Stage.SilentSet(runtimeGameState.stage);
            ClosedGatesCount.SilentSet(runtimeGameState.completedLevelsCount);
        }
    }
}
