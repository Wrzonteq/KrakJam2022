using System;
using System.Collections.Generic;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class MemoryStorageSystem : BaseGameSystem {
        [SerializeField] Memory[] memories;

        public Dictionary<Emotion, Memory> MemoriesDict { get; private set; }


        public override void OnCreate() {
            MemoriesDict = new Dictionary<Emotion, Memory>();
            foreach (var mem in memories) {
                MemoriesDict[mem.associatedEmotion] = mem;
            }
        }

        public override void Initialise() { }

        [Serializable]
        public class Memory {
            public string name;
            public Sprite sprite;
            public Emotion associatedEmotion;
            public MemoryType type;
            public string description;

            public Memory() {
                name = associatedEmotion.ToString();
            }
        }
    }

    public enum MemoryType {
        Positive,
        Negative
    }
}
