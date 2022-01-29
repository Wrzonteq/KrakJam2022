using System;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    [Serializable]
    public class MemoryData {
        public Sprite sprite;
        public MemoryType type;
        public string description;
    }

    public enum MemoryType {
        Positive,
        Negative
    }
}
