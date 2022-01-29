#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
namespace PartTimeKamikaze.KrakJam2022 {
    public class TestingWindow : EditorWindow {


        [MenuItem("KrakJam2022/Test Window", false, 0)]
        static void OpenTheWindow() {
            GetWindow<TestingWindow>("Testing Window");
        }

        void OnGUI() {
            if (!Application.isPlaying) {
                GUILayout.Label("Enter playmode to see tests.");
            } else
                DrawWindow();
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
        }
    }
}

#endif
