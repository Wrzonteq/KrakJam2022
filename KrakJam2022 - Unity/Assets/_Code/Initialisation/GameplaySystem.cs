using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PartTimeKamikaze.KrakJam2022.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {
    public class GameplaySystem : BaseGameSystem {
        public event Action<PlayerController> PlayerInstantiatedEvent;
        public event Action ReturnedToMainMenu;

        [SerializeField] PlayerController playerPrefab;

        Dictionary<Emotion, EmotionLevelArea> areasDict;
        Dictionary<Emotion, LevelGate> gatesDict;

        public Emotion CurrentEmotionLevel => (Emotion)GameSystems.GetSystem<GameStateSystem>().ClosedGatesCount.Value;
        public PlayerController PlayerInstance { get; private set; }

        bool isInGameplay;
        public bool IsInGameplay { get =>  isInGameplay;
            private set {
                isInGameplay = value;
                if (value)
                    OnGameplayStart();
                else
                    OnGameplayEnd();
            }
        }

        public override void OnCreate() {
            areasDict = new Dictionary<Emotion, EmotionLevelArea>();
            gatesDict = new Dictionary<Emotion, LevelGate>();
        }

        public override void Initialise() { }

        void OnGameplayStart() {
            GameSystems.GetSystem<InputSystem>().Bindings.Gameplay.OpenPauseMenu.performed += OpenPauseScreen;
        }

        void OnGameplayEnd() {
            GameSystems.GetSystem<InputSystem>().Bindings.Gameplay.OpenPauseMenu.performed -= OpenPauseScreen;
        }

        public void StartNewGame() {
            GameSystems.GetSystem<GameStateSystem>().ResetGameState();
            LoadSavedGame(GameSystems.GetSystem<GameStateSystem>().runtimeGameState).Forget();
        }

        public async UniTaskVoid LoadSavedGame(GameStateDataAsset gameState) {
            GameSystems.GetSystem<InputSystem>().SwitchToGameplayInput();
            await LoadGameplaySceneAndShowProgress();
            LoadGame(gameState);
        }

        async UniTask LoadGameplaySceneAndShowProgress() {
            GameSystems.GetSystem<UISystem>().GetScreen<MainMenuScreen>().Hide(true).Forget();
            var sceneLoadingSystem = GameSystems.GetSystem<SceneLoadingSystem>();
            GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Show(true).Forget();
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue += DisplayProgress;
            await sceneLoadingSystem.LoadSceneAsync(Consts.ScenesNames.Gameplay);
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue -= DisplayProgress;
            GameSystems.GetSystem<UISystem>().ShowScreen<HUDScreen>().Forget();
            await GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Hide();
        }

        void LoadGame(GameStateDataAsset gameState) {
            GameSystems.GetSystem<GameStateSystem>().LoadCurrentStateToProperties();
            SpawnPlayer();
            GameSystems.GetSystem<CameraSystem>().SetupCrosshairFollowTarget(PlayerInstance.CrosshairFollowTarget);
            gatesDict.Clear();
            var gates = FindObjectsOfType<LevelGate>();
            foreach (var gate in gates) {
                gatesDict[gate.Emotion] = gate;
                gate.InitialiseFromSavedState(gameState.GetStateForEmotion(gate.Emotion));
            }
            areasDict.Clear();
            var levels = FindObjectsOfType<EmotionLevelArea>();
            foreach (var area in levels) {
                areasDict[area.Emotion] = area;
                area.Initialise();
                area.LoadState(gameState.GetStateForEmotion(area.Emotion));
            }
            FindObjectOfType<MapController>().Init();

            OpenNextUnopenedGate();

            IsInGameplay = true;
        }

        void SpawnPlayer() {
            PlayerInstance = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            PlayerInstance.Initialise();
            PlayerInstantiatedEvent?.Invoke(PlayerInstance);
        }

        void OpenNextUnopenedGate() {
            gatesDict[CurrentEmotionLevel].Activate();
        }

        void OpenPauseScreen(InputAction.CallbackContext _) {
            Cursor.visible = true;
            GameSystems.GetSystem<UISystem>().GetScreen<PauseMenuScreen>().Show().Forget();
        }

        void DisplayProgress(float progress) {
            GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().SetProgress(progress);
        }

        public void BeginInsanityStage() {
            var gameStateSystem = GameSystems.GetSystem<GameStateSystem>();
            gameStateSystem.Stage.Value = GameStage.Insanity;
            gameStateSystem.runtimeGameState.GetStateForEmotion(CurrentEmotionLevel).insanityStarted = true;

            foreach (var gate in gatesDict)
            {
                gate.Value.GoInsane();
            }
        }

        public void EndInsanityStage() {
            var gameStateSystem = GameSystems.GetSystem<GameStateSystem>();
            gameStateSystem.Stage.Value = GameStage.Sanity;
            gameStateSystem.runtimeGameState.GetStateForEmotion(CurrentEmotionLevel).insanitySurvived = true;
            gameStateSystem.ClosedGatesCount.Value++;
            if (gameStateSystem.ClosedGatesCount.Value < Consts.TOTAL_GATES_COUNT)
                HandleInsanityCompleted();
            else
                HandleAllGatesClosed();
        }

        void HandleInsanityCompleted() {
            foreach (var gate in gatesDict)
            {
                gate.Value.Close();
            }

            OpenNextUnopenedGate();
        }

        void HandleAllGatesClosed() {
            GameSystems.GetSystem<GameStateSystem>().Stage.Value = GameStage.Sanity;
            Debug.LogError($"YOU WIN MAI BOI!!");
            GameSystems.GetSystem<InputSystem>().SwitchToInterfaceInput();
            GameSystems.GetSystem<UISystem>().ShowScreen<VictoryScreen>();
        }

        public void HandleGameOver() {
            GameSystems.GetSystem<GameStateSystem>().Stage.Value = GameStage.Sanity;
            Debug.LogError("YOU LOOZE NOOB");
            GameSystems.GetSystem<InputSystem>().SwitchToInterfaceInput();
            GameSystems.GetSystem<UISystem>().ShowScreen<GameOverScreen>();
            Cursor.visible = true;
        }

        public EmotionLevelArea GetLevelArea(Emotion emotion) {
            return areasDict[emotion];
        }

        public LevelGate GetGate(Emotion emotion) {
            return gatesDict[emotion];
        }

        public EmotionLevelState GetCurrentEmotionState() {
            return GameSystems.GetSystem<GameStateSystem>().runtimeGameState.GetStateForEmotion(CurrentEmotionLevel);
        }

        public async UniTaskVoid ReturnToMenu() {
            IsInGameplay = false;
            GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Show(true).Forget();
            GameSystems.GetSystem<UISystem>().HideScreen<HUDScreen>().Forget();
            var sceneLoadingSystem = GameSystems.GetSystem<SceneLoadingSystem>();
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue += DisplayProgress;
            await sceneLoadingSystem.UnloadSceneAsync(Consts.ScenesNames.Gameplay);
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue -= DisplayProgress;
            GameSystems.GetSystem<UISystem>().GetScreen<MainMenuScreen>().Show(true).Forget();
            await GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Hide();
            ReturnedToMainMenu?.Invoke();
        }
    }
}
