using System;
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
            gameStartLoadingScreen.Initialise();
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
                instance.OnCreate();
                systemsInstances.Add(instance);
                loadingProgressCounter++;
                gameStartLoadingScreen.SetProgress(loadingProgressCounter / stuffToLoad);
                await UniTask.Delay(500);
            }

            foreach (var instance in systemsInstances) {
                instance.Initialise();
                loadingProgressCounter++;
                gameStartLoadingScreen.SetProgress(loadingProgressCounter / stuffToLoad);
            }

            GameSystems.Init(systemsInstances);
            await UniTask.Delay(500);
            await gameStartLoadingScreen.Hide(true);
            Destroy(gameStartLoadingScreen.transform.parent.gameObject);
            await GameSystems.GetSystem<UISystem>().ShowScreen<MainMenuScreen>(true);
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

