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
                instance.Initialise();
                systemsInstances.Add(instance);
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

    public static class GameEvents {
        public static event Action MainMenuLoadedEvent = delegate { };
    }

    // nie jest abstract, zeby mozna bylo stworzyc serializowana liste, ale po tej klasie dziedziczymy wszystkie inne systemy
    // ale dziala jak abstract, bo nie mozna podpiac jako komponent, jesli nie jest w pliku o tej samej nazwie, BANG
    public class BaseGameSystem : MonoBehaviour {
        public bool IsInitialised { get; private set; }

        public void Initialise() {
            if (IsInitialised)
                return;
            OnInitialise();
            IsInitialised = true;
        }

        protected virtual void OnInitialise() { }
    }
}

