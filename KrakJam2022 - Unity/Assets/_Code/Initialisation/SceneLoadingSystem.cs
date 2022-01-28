using System;
using Cysharp.Threading.Tasks;
using PartTimeKamikaze.KrakJam2022.Utils;
using UnityEngine.SceneManagement;

namespace PartTimeKamikaze.KrakJam2022 {
    public class SceneLoadingSystem : BaseGameSystem {
        public Observable<float> SceneLoadingProgress;

        /// From docs as a reminder:
        /// "The active Scene is the Scene which will be used as the target for new GameObjects instantiated by scripts and from what Scene the lighting settings are used.
        /// When you add a Scene additively (see LoadSceneMode.Additive), the first Scene is still kept as the active Scene."


        public override void OnCreate() {
            SceneLoadingProgress = new Observable<float>();
        }

        public override void Initialise() { }

        public async UniTask LoadSceneAsync(string sceneName, Action onCompleteCallback = null) {
            var loadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneLoadingProgress.Set(0, true);
            while (!loadingOperation.isDone) {
                SceneLoadingProgress.Value = loadingOperation.progress;
                UnityEngine.Debug.Log($"LOAD PROGRESS: {SceneLoadingProgress.Value}");
                await UniTask.DelayFrame(1);
            }
            await UniTask.Delay(500); //tymczasowo - zeby bylo widac, ze sie w ogole pokazuje loading screen
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            onCompleteCallback?.Invoke();
        }
    }
}
