using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class GameInitialiser : MonoBehaviour {
        [SerializeField] BaseGameSystem[] systemPrefabs;
        [SerializeField] Transform systemsRoot;


        void Awake() {
            InitialiseGameSystems();
            LoadMainMenuScene().Forget();
        }

        void InitialiseGameSystems() {
            var systemsInstances = new List<BaseGameSystem>();
            DontDestroyOnLoad(systemsRoot.gameObject);
            foreach (var prefab in systemPrefabs) {
                var instance = Instantiate(prefab, systemsRoot, false);
                instance.OnCreate();
                systemsInstances.Add(instance);
            }

            foreach (var instance in systemsInstances) {
                instance.Initialise();
            }

            GameSystems.Init(systemsInstances);
        }

        async UniTaskVoid LoadMainMenuScene() {
            var loadingScreen = GameSystems.GetSystem<UISystem>().LoadingScreen;
            loadingScreen.Show();
            var sceneLoadingSystem = GameSystems.GetSystem<SceneLoadingSystem>();
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue += OnProgressChanged;
            await sceneLoadingSystem.LoadSceneAsync(Consts.ScenesNames.MainMenu);
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue -= OnProgressChanged;
            loadingScreen.Hide();
            void OnProgressChanged(float progress) => loadingScreen.SetProgress(progress);
        }
    }

    // klasa do dodawania eventów - do kazdego eventu jest potrzebna funkcja NotifyEventName, ktora go wywola
    public static class GameEvents {
        public static event Action ExampleGameLogicEvent;

        public static void ExampleNotifyGameLogicEvent() {
            ExampleGameLogicEvent?.Invoke();
        }
    }
}

