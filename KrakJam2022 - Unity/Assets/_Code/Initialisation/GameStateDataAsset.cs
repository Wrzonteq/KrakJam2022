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

        public int currentSanity = Consts.DefaultValues.DefaultSanity;
        public int completedLevelsCount;

        //tutaj bedziemy tez trzymac info o tym ile mamy "amunicji" w danym momencie etc.

        public EmotionLevelState GetStateForEmotion(Emotion emotion) {
            switch (emotion) {
                case Emotion.Anger:
                    return angerState;
                case Emotion.Fear:
                    return fearState;
                case Emotion.Sadness:
                    return sadnessState;
                case Emotion.Loneliness:
                    return lonelinessState;
                case Emotion.Despair:
                    return despairState;
                default:
                    throw new ArgumentOutOfRangeException(nameof(emotion), emotion, null);
            }
        }
    }

    [Serializable]
    public class EmotionLevelState {
        public bool gateUnlocked;
        public bool negativeMemoryCollected;
        public bool positiveMemoryCollected;
        public bool insanityStarted;
        public bool insanitySurvived;

        public bool CanStartInsanity => negativeMemoryCollected && positiveMemoryCollected;
    }

    public enum Emotion {
        Fear = 0,
        Sadness = 1,
        Anger = 2,
        Loneliness = 3,
        Despair = 4,
    }

    public enum GameStage {
        Unset = -1,
        Sanity,
        Insanity
    }
}
