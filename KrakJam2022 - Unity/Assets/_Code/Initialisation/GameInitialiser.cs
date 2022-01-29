using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PartTimeKamikaze.KrakJam2022.UI;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class GameInitialiser : MonoBehaviour {
        [SerializeField] BaseGameSystem[] systemPrefabs;
        [SerializeField] Transform systemsRoot;
        [SerializeField] LoadingScreen gameStartLoadingScreen;


        void Awake() {
            gameStartLoadingScreen.Initialise(null);
            gameStartLoadingScreen.Show(true).Forget();
            gameStartLoadingScreen.SetProgress(0);
            InitialiseGameSystems().Forget();
        }

        async UniTaskVoid InitialiseGameSystems() {
            var systemsInstances = new List<BaseGameSystem>();
            DontDestroyOnLoad(systemsRoot.gameObject);
            var loadingProgressCounter = 0f;
            var stuffToLoad = systemPrefabs.Length * 2;
            for (var i = 0; i < systemPrefabs.Length; i++) {
                var prefab = systemPrefabs[i];
                var instance = Instantiate(prefab, systemsRoot, false);
                Debug.Log($"Created System: {instance.name}");
                instance.OnCreate();
                systemsInstances.Add(instance);
                loadingProgressCounter++;
                gameStartLoadingScreen.SetProgress(loadingProgressCounter / stuffToLoad);
                await UniTask.Delay(100);
            }

            GameSystems.Init(systemsInstances);

            foreach (var instance in systemsInstances) {
                Debug.Log($"Initialising System: {instance.name}");
                instance.Initialise();
                loadingProgressCounter++;
                gameStartLoadingScreen.SetProgress(loadingProgressCounter / stuffToLoad);
            }

            await UniTask.Delay(200);
            await GameSystems.GetSystem<UISystem>().ShowScreen<MainMenuScreen>(true);
            await gameStartLoadingScreen.Hide();
            Destroy(gameStartLoadingScreen.transform.parent.gameObject);
        }
    }
}

