using System;
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
            DontDestroyOnLoad(systemsRoot.gameObject);
            foreach (var prefab in systemPrefabs) {
                var systemInstance = Instantiate(prefab, systemsRoot, false);
                systemInstance.Initialise();
            }
        }

        async UniTaskVoid LoadMainMenuScene() {
            var loadingScreen = GameSystems.ui.LoadingScreen;
            loadingScreen.Show();
            GameSystems.sceneLoading.SceneLoadingProgress.ChangedValue += OnProgressChanged;
            await GameSystems.sceneLoading.LoadSceneAsync(Consts.ScenesNames.MainMenu);
            GameSystems.sceneLoading.SceneLoadingProgress.ChangedValue -= OnProgressChanged;
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

