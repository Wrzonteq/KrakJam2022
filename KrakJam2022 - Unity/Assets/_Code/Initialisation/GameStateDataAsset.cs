using System;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    [CreateAssetMenu(fileName = "GameStateData", menuName = "KrakJam2022/GameStateAsset")]
    public class GameStateDataAsset : ScriptableObject {
        public GameStage stage;
        public EmotionLevelState angerState;
        public EmotionLevelState fearState;
        public EmotionLevelState sadnessState;
        public EmotionLevelState lonelinessState;
        public EmotionLevelState despairState;

        //tutaj bedziemy tez trzymac info o tym ile mamy "amunicji" w danym momencie etc.
    }

    [Serializable]
    public class EmotionLevelState {
        public bool doorUnlocked;
        public int minigamesCount;
        public int minigamesCompleted; // jak juz bedziemy miec level controller to sie tu cos lepszego wymysli, zeby wiedziec ktore konkretnie sa zrobione
        public bool insanityStarted;
        public bool insanitySurvived;
    }

    public enum GameStage {
        Unset = -1,
        Sanity,
        Insanity
    }
}
