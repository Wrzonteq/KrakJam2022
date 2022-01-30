#if UNITY_EDITOR

using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace PartTimeKamikaze.KrakJam2022 {
    public class TestingWindow : EditorWindow {


        [MenuItem("KrakJam2022/Test Window", false, 0)]
        static void OpenTheWindow() {
            GetWindow<TestingWindow>("Testing Window");
        }

        void OnGUI() {
            if (!Application.isPlaying || !GameSystems.IsInitialised) {
                GUILayout.Label("Enter playmode to see tests.");
            } else {
                DrawWindow();
                DrawHotWaterHaxxes();
            }
        }

        void DrawWindow() {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Start enemies spawning")) {
                GameSystems.GetSystem<EnemiesSystem>().Test();
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Move to Escape Game 1")) {
                EscapeGame1 escapeGame1 = FindObjectOfType<EscapeGame1>();
                escapeGame1.Initialise();
            }

            if (GUILayout.Button("Move to Escape Game 2")) {
                EscapeGame2 escapeGame2 = FindObjectOfType<EscapeGame2>();
                escapeGame2.Initialise();
            }
        }

        void DrawHotWaterHaxxes() {
            var gameplaySystem = GameSystems.GetSystem<GameplaySystem>();
            if (!gameplaySystem.IsInGameplay) {
                GUILayout.Label($"Start game to see haxxz");
                return;
            }
            var currentArea = gameplaySystem.GetLevelArea(gameplaySystem.CurrentEmotionLevel);
            GUILayout.Label($"Current emotion: {gameplaySystem.CurrentEmotionLevel}");
            if (!currentArea.IsPlayerInside) {
                GUILayout.Label($"Player is not inside current area");
                return;
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Skip puzzle")) {
                var minigames = (Minigame[]) typeof(EmotionLevelArea).GetField("minigames", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(currentArea);
                var puzzle = (SlidingPuzzleController)minigames[0];
                puzzle.EnableReward();
            }
            GUILayout.EndHorizontal();
        }


    }
}

#endif
