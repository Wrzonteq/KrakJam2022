using PartTimeKamikaze.KrakJam2022.Utils;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class GameStateSystem : BaseGameSystem {
        [SerializeField] GameStateDataAsset defaultGameState;

        public GameStateDataAsset runtimeGameState { get; private set; }

        public Observable<int> Sanity { get; private set; }


        public override void OnCreate() {
            runtimeGameState = ScriptableObject.CreateInstance<GameStateDataAsset>();
            Sanity = new Observable<int>(100);
            Sanity.ChangedValue += HandleSanityChanged;
        }

        void HandleSanityChanged(int sanityValue) {
            runtimeGameState.currentSanity = sanityValue;
        }

        public override void Initialise() {
            
        }

        public void ResetGameState() {
            var json = JsonUtility.ToJson(defaultGameState);
            JsonUtility.FromJsonOverwrite(json, runtimeGameState);
        }
    }
}
